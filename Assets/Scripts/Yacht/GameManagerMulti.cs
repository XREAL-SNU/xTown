using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace XReal.XTown.Yacht
{


    public class GameManagerMulti : GameManager, ITurnCallbacks
    {


        // Update is called once per frame
        protected override void Update()
        {
            if (NetworkManager.Instance is null) return;
            if (!NetworkManager.Instance.networked)
            { // call base function if not networked.
                base.Update();
                return;
            }
            if (NetworkManager.Instance.MeDone || NetworkManager.Instance.Turn < 1) return;
            base.Update();
        }


        // called by selectScore once score selected.
        public static void TurnFinish()
        {
            if (NetworkManager.Instance.MeDone)
            {
                return;
            }
            // this will set MeDone.
            NetworkManager.Instance.SendFinishTurn();
            
            // you must not reinitialize gameState. That'll cause animator collision!
        }
        /* 
=

        public void Onclick_endTurn()
        {
            NetworkManager.Instance.SendFinishTurn();
            if (NetworkManager.Instance.MeDone)
            {
                diceBtn.interactable = false;
                endBtn.interactable = false;
            }
        }

        */
        public Text turnText;

        
        
        public void SetTurnText(int turn)
        {
            turnText.text = "turn " + turn;
        }


        /// <summary>
        /// Interface ITurnCallbacks impl
        /// </summary>
        public void OnTurnBegins(int turn)
        {
            if (NetworkManager.Instance.MeDone) return;
            // request ownership
            CupManagerMulti.RequestCupOwnership();
            SetGameState(GameState.initializing);
            Debug.Log("GameManager/OnTurnBegins Turn #" + turn);
            SetTurnText(turn);
        }

        public void OnPlayerDiceResult(Player player, int turn, int[] results)
        {
            string msg = "on turn #" + turn + ", player " + player.ActorNumber + " rolled: ";
            foreach (int num in results) msg += num;
            Debug.Log(msg);
        }

        public void OnPlayerStrategySelected(Player player, int turn, int move)
        {
            string msg = "on turn #" + turn + ", player " + player.ActorNumber + " selected: " + move;
        }

        public void OnPlayerFinished(Player player, int turn)
        {
            if (NetworkManager.Instance.Turn != turn)
            {
                Debug.LogError("Turn miss sync");
                return;
            }

            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) // me finished
            {
                return;
            }
            // other's turn finished
            Debug.Log("player #" + player.ActorNumber + "'s turn end synced");
            Debug.Log("player #" + PhotonNetwork.LocalPlayer.ActorNumber + " beginning turn");
            // if I'm not finished, I guess it's my turn -> will invoke OnBeginTurn
            NetworkManager.Instance.BeginTurn();
        }
    }
}

