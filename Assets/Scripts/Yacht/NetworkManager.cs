using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using XReal.XTown.Yacht;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace XReal.XTown.Yacht
{
    public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        /* singleton */
        public static NetworkManager Instance { get; private set; }
        [SerializeField]
        private GameObject _shownSlots;
        [SerializeField]
        private GameObject _pickedSlots;
        private NetworkManager() { }

        // set in function BeginTurn and SendFinishTurn.
        private bool _meDone = true;
        private bool _isYachtLeaving = false;
        public bool MeDone
        {
            get => _meDone;
            private set { _meDone = value; }
        }
        // multiplay?
        public ITurnCallbacks TurnListener;
        public bool networked = false;
        /// Monobehaviour callbacks
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            networked = PhotonNetwork.IsConnected;
            Debug.Log(networked ? "----networked----" : "----not networked----");
            if (networked)
            {
                TurnListener = GetComponent<GameManagerMulti>();
                ScoreTableMulti.InitMultiTable();
                // destroy local version of mugcup. will instantiate fresh over the network.
                Destroy(GameObject.Find("MugCup"));
                // destroy local version of dices, also clear the list.
                Debug.Log("DiceManager/Start : Destroying dices");
                foreach (DiceScript oldDice in DiceManager.dices)
                {
                    Destroy(oldDice.gameObject);
                }
                DiceScript.diceInfoList.Clear();
                DiceManager.dices.Clear();
            }
            else
            {
                Debug.LogWarning("NOT CONNECTED TO NETWORK: singleplay mode");
                ScoreTable.InitSingleTable();
                // No synchronization!
                CupManagerMulti.DisableCupView();
                return;
            }

        }

        /// UI callbacks
        public void OnClick_LeaveRoom()
        {
            Debug.Log("NetworkManager YachtLeave / Button Clicked!");
            PlayerPrefs.SetString("prevScene","Yacht");
            _isYachtLeaving = true;
            TestConnect.Instance._isLeavingYacht = true;
            PhotonNetwork.LeaveRoom();
        }

        public override void OnJoinedLobby()
        {
            if(TestConnect.Instance._isLeavingYacht)
            {
                Debug.Log("isLeaving");
                RoomOptions options = new RoomOptions();
                options.BroadcastPropsChangeToAll = true;
                options.MaxPlayers = 20;
                options.EmptyRoomTtl = 20000;
                options.PlayerTtl = 30000;
                PhotonNetwork.JoinOrCreateRoom("MainWorld", options, TypedLobby.Default);
                TestConnect.Instance._isLeavingYacht= false;
            }
        }


        /// Photon callbacks
        public override void OnJoinedRoom()
        {
            if(!_isYachtLeaving && !TestConnect.Instance._isStarting)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    Debug.Log("OnJoinedRoom/ alone in the room.");
                }
                else
                {
                    Debug.Log("OnJoinedRoom/Two players: Starting game");
                    // the second player will instantiate the cup and the dices.
                    PhotonNetwork.Instantiate("MugCup", new Vector3(7.14f, 1.31f, 0f), Quaternion.Euler(0f, 90f, 0f));
                    // spawn dices
                    DiceManager.instance.GetComponent<DiceManagerMulti>().SpawnDices();
                    // only second player will start the game
                    BeginTurn();
                }
            }
        }
        /// turn passing methods
        // defining turn
        public int Turn
        {
            get { return PhotonNetwork.CurrentRoom.GetTurn(); }
            private set { PhotonNetwork.CurrentRoom.SetTurn(value); }
        }

        // Eventcodes: an integer range 0 - 255
        public const byte TurnEventOffset = 0;
        public const byte EvDiceResult = 1 + TurnEventOffset;
        public const byte EvStrategySelected = 2 + TurnEventOffset;
        public const byte EvFinishTurn = 3 + TurnEventOffset;
        public const byte EvEndGame = 4 + TurnEventOffset;

        // public methods
        public void BeginTurn()
        {
            Turn = Turn + 1;
            MeDone = false;
            // callback for this inside OnRoomPropertiesUpdate method
        }

        public void SendDiceResult(int[] diceResults)
        {
            if (MeDone)
            {
                Debug.LogWarning("player" + PhotonNetwork.LocalPlayer.ActorNumber + "trying to send move against turn.");
                return;
            }

            // send info
            Hashtable ht = new Hashtable();
            ht.Add("turn", Turn);
            ht.Add("move", System.Array.ConvertAll(diceResults, itm => (object)itm));


            PhotonNetwork.RaiseEvent(EvDiceResult, ht, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
            // will I listen to this event as well? if so, this must be called.
            // a local call like this may disrupt order, but, yeah.
            ProcessOnEvent(EvDiceResult, ht, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        public void SendStrategySelected(int strategy, int score)
        {
            if (MeDone)
            {
                Debug.LogWarning("player" + PhotonNetwork.LocalPlayer.ActorNumber + "trying to send move against turn.");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("turn", Turn);
            ht.Add("move", (object)strategy);
            ht.Add("score",(object)score);

            PhotonNetwork.RaiseEvent(EvStrategySelected, ht, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
            ProcessOnEvent(EvStrategySelected, ht, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        public void SendFinishTurn()
        {
            if (MeDone)
            {
                return;
            }

            // I retire..
            MeDone = true;
            Debug.Log("NetworkManager:event/ player" + PhotonNetwork.LocalPlayer.ActorNumber + "finished turn sent");

            // send finishing message
            Hashtable ht = new Hashtable();
            ht.Add("turn", Turn);
            if(Turn == 24)
            {
                PhotonNetwork.RaiseEvent(EvEndGame, ht, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
                ProcessOnEvent(EvEndGame, ht, PhotonNetwork.LocalPlayer.ActorNumber);
                return;
            }
            PhotonNetwork.RaiseEvent(EvFinishTurn, ht, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
            ProcessOnEvent(EvFinishTurn, ht, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        /* event parsing methods */
        public void ProcessOnEvent(byte evCode, object data, int senderActorNumber)
        {
            if(PhotonNetwork.CurrentRoom == null)
            return;
            Player sender = PhotonNetwork.CurrentRoom.GetPlayer(senderActorNumber);
            switch (evCode)
            {
                case EvDiceResult:
                    {
                        // conversion party!!! events need object type arguments!!
                        Hashtable evTable = data as Hashtable;
                        object[] objArr = (object[])evTable["move"];
                        int[] diceResults = System.Array.ConvertAll(objArr, obj => (int)obj);
                        int turn = (int)evTable["turn"];
                        TurnListener.OnPlayerDiceResult(sender, turn, diceResults);

                        break;
                    }
                case EvStrategySelected:
                    {
                        Hashtable evTable = data as Hashtable;
                        int turn = (int)evTable["turn"];
                        int strategy = (int)evTable["move"];
                        int score = (int)evTable["score"];
                        TurnListener.OnPlayerStrategySelected(sender, turn, strategy, score);
                        break;
                    }
                case EvFinishTurn:
                    {
                        Hashtable evTable = data as Hashtable;
                        int turn = (int)evTable["turn"];

                        TurnListener.OnPlayerFinished(sender, turn);
                        break;
                    }
                case EvEndGame:
                    {
                         Hashtable evTable = data as Hashtable;
                         int turn = (int)evTable["turn"];
                         TurnListener.OnGameEnd(sender,turn);
                         break;
                    }
            }
        }


        /* event callbacks */

        // custom event
        public void OnEvent(EventData photonEvent)
        {
            ProcessOnEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
        }
        // Turn - room property event
        public override void OnRoomPropertiesUpdate(Hashtable props)
        {

            if (props != null && props.ContainsKey("Turn"))
            {
                // both players hear this event.
                TurnListener.OnTurnBegins(Turn);
                Debug.Log($"NetworkManager Room properties updated/ Turn #{Turn}");
            }
        }
    }



    public interface ITurnCallbacks
    {
        void OnTurnBegins(int turn);

        void OnPlayerDiceResult(Player player, int turn, int[] results);

        void OnPlayerStrategySelected(Player player, int turn, int move, int score);

        void OnPlayerFinished(Player player, int turn);

        void OnGameEnd(Player player, int turn);
    }

    public static class TurnExtensions
    {
        public static readonly string TurnPropKey = "Turn";


        public static void SetTurn(this Room room, int turn)
        {
            if (room == null || room.CustomProperties == null)
            {
                Debug.LogError("NetworkManager: Cannot set turn information to custorm properties");
                return;
            }
            Hashtable turnProps = new Hashtable();
            turnProps[TurnPropKey] = turn;

            room.SetCustomProperties(turnProps);
        }

        public static int GetTurn(this RoomInfo room)
        {
            if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnPropKey))
            {
                return 0;
            }

            return (int)room.CustomProperties[TurnPropKey];
        }
    }

}
