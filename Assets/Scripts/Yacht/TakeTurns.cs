using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
namespace XReal.XTown.Yacht
{
    public class TakeTurns : MonoBehaviourPunCallbacks, IOnEventCallback
    {

        /* what will listen to this? */
        public GameManager TurnListener;

        /* list of finished players */
        private readonly HashSet<Player> finishedPlayers = new HashSet<Player>();
        


        /* the turn property -> a custom room property shared inside the room */
        public int Turn
        {
            get { return PhotonNetwork.CurrentRoom.GetTurn(); }
            private set
            {
                PhotonNetwork.CurrentRoom.SetTurn(value, true);
            }
        }

        public bool IsFinishedByMe
        {
            get { return this.finishedPlayers.Contains(PhotonNetwork.LocalPlayer); }
        }







        // event codes
        public const byte TakeTurnsEventOffset = 0;
        // this is called when a the other player's score hints update
        public const byte EvMove = 1 + TakeTurnsEventOffset;
        // this is called when the other player completes his turn, either
        // used up all three chances or chose a category.
        public const byte EvFinalMove = 2 + TakeTurnsEventOffset;


        /* Turn management functions - either increments turns or raises events */
        public void BeginTurn()
        {
            Turn = this.Turn + 1; // note: this will set a property in the room, which is available to the other players.
        }

        public void SendMove(object move, bool finished) // set finished bit if 
        {
            if (IsFinishedByMe) // not my turn anymore.
            {
                Debug.LogWarning("Yacht/TakeTurns: Can't SendMove. Turn Finished");
                return;
            }

            // along with the actual move, we have to send which turn this move belongs to
            Hashtable content = new Hashtable();
            content.Add("turn", Turn);
            content.Add("move", move);

            byte evCode = (finished) ? EvFinalMove : EvMove;
            PhotonNetwork.RaiseEvent(evCode, content, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
            if (finished)
            {
                PhotonNetwork.LocalPlayer.SetFinishedTurn(Turn);
            }

            // the server won't send the event back to the origin (by default). to get the event, call it locally
            // (note: the order of events might be mixed up as we do this locally)
            ProcessOnEvent(evCode, content, PhotonNetwork.LocalPlayer.ActorNumber);
        }


        /* callbacks */

        public void OnEvent(EventData photonEvent)
        {
            ProcessOnEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
        }

        Player sender;

        void ProcessOnEvent(byte eventCode, object content, int senderId)
        {
            if (senderId == -1)
            {
                return;
            }

            sender = PhotonNetwork.CurrentRoom.GetPlayer(senderId);

            switch (eventCode)
            {
                case EvMove:
                    { // any action that does not end turn
                        Hashtable ev = content as Hashtable;
                        int turn = (int)ev["turn"];
                        object move = ev["move"];
                        this.TurnListener.OnPlayerMove(sender, turn, move);

                        break;
                    }
                case EvFinalMove:
                    { // player's turn ends
                        Hashtable ev = content as Hashtable;
                        int turn = (int)ev["turn"];
                        object move = ev["move"];

                        if (turn == this.Turn)
                        {
                            finishedPlayers.Add(sender);
                            TurnListener.OnPlayerFinished(sender, turn, move);

                        }

                        if (Turn > 0 && this.finishedPlayers.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                        {
                            TurnListener.OnTurnCompleted(this.Turn);
                        }
                        break;
                    }
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesList)
        {
            //Debug.Log("OnRoomPropertiesUpdate: " + propertiesList.ToStringFull());
            // if the turn property has been update === new turn!
            if (propertiesList.ContainsKey("Turn"))
            {
                finishedPlayers.Clear();
                TurnListener.OnTurnBegins(this.Turn);
            }
        }

    }


    /* interface defining the callbacks */
    public interface IPunTurnManagerCallbacks // must be implemented
    {
        void OnTurnBegins(int turn);
        void OnTurnCompleted(int turn);
        void OnPlayerMove(Player player, int turn, object move);

        // When a player finishes a turn (includes the move of that player)
        void OnPlayerFinished(Player player, int turn, object move);
    }

    /* photon turns extension properties */
    #region extension
    public static class TurnExtensions
    {
        /// <summary>
        /// currently ongoing turn number
        /// </summary>
        public static readonly string TurnPropKey = "Turn";

        /// <summary>
        /// start (server) time for currently ongoing turn (used to calculate end)
        /// </summary>
        public static readonly string TurnStartPropKey = "TStart";

        /// <summary>
        /// Finished Turn of Actor (followed by number)
        /// </summary>
        public static readonly string FinishedTurnPropKey = "FToA";

        /// <summary>
        /// Sets the turn.
        /// </summary>
        /// <param name="room">Room reference</param>
        /// <param name="turn">Turn index</param>
        /// <param name="setStartTime">If set to <c>true</c> set start time.</param>
        public static void SetTurn(this Room room, int turn, bool setStartTime = false)
        {
            if (room == null || room.CustomProperties == null)
            {
                return;
            }

            Hashtable turnProps = new Hashtable();
            turnProps[TurnPropKey] = turn;
            if (setStartTime)
            {
                turnProps[TurnStartPropKey] = PhotonNetwork.ServerTimestamp;
            }

            room.SetCustomProperties(turnProps);
        }

        /// <summary>
        /// Gets the current turn from a RoomInfo
        /// </summary>
        /// <returns>The turn index </returns>
        /// <param name="room">RoomInfo reference</param>
        public static int GetTurn(this RoomInfo room)
        {
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnPropKey))
            {
                return 0;
            }

            return (int)room.CustomProperties[TurnPropKey];
        }


        /// <summary>
        /// Returns the start time when the turn began. This can be used to calculate how long it's going on.
        /// </summary>
        /// <returns>The turn start.</returns>
        /// <param name="room">Room.</param>
        public static int GetTurnStart(this RoomInfo room)
        {
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnStartPropKey))
            {
                return 0;
            }

            return (int)room.CustomProperties[TurnStartPropKey];
        }

        /// <summary>
        /// gets the player's finished turn (from the ROOM properties)
        /// </summary>
        /// <returns>The finished turn index</returns>
        /// <param name="player">Player reference</param>
        public static int GetFinishedTurn(this Player player)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnPropKey))
            {
                return 0;
            }

            string propKey = FinishedTurnPropKey + player.ActorNumber;
            return (int)room.CustomProperties[propKey];
        }

        /// <summary>
        /// Sets the player's finished turn (in the ROOM properties)
        /// </summary>
        /// <param name="player">Player Reference</param>
        /// <param name="turn">Turn Index</param>
        public static void SetFinishedTurn(this Player player, int turn)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room == null || room.CustomProperties == null)
            {
                return;
            }

            string propKey = FinishedTurnPropKey + player.ActorNumber;
            Hashtable finishedTurnProp = new Hashtable();
            finishedTurnProp[propKey] = turn;

            room.SetCustomProperties(finishedTurnProp);
        }
    }

    #endregion

}