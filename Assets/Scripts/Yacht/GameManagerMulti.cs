using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XReal.XTown.Yacht
{


    public class GameManagerMulti : GameManager, ITurnCallbacks
    {


        /// <summary>
        /// Monobehaviour event callbacks
        /// </summary>
        protected override void Update()
        {
            if(NetworkManager.Instance is null)
            return;
            if (!NetworkManager.Instance.networked)
            { // call base function if not networked.
                base.Update();
                return;
            }
            if (NetworkManager.Instance.MeDone || !IsReady || NetworkManager.Instance.Turn < 1) return;
            base.Update();
        }

        // public methods
        public static bool IsReady = false;
        public static int MyTotalPoints = 0;
        public static int OtherTotalPoints = 0;

        // called by selectScore once score selected.
        public static void TurnFinish()
        {
            if (NetworkManager.Instance.MeDone)
            {
                return;
            }
            // this will set MeDone.
            NetworkManager.Instance.SendFinishTurn();
        }
        public Text turnText;

        
        
        public void SetTurnText(int turn)
        {
            turn++;
            int round = (turn)/2;
            int turnplayer = (turn % 2 + 1); 
            turnText.text = "Turn : " + round + "-" + turnplayer;
        }


        /// <summary>
        /// Interface ITurnCallbacks impl
        /// </summary>
        public void OnTurnBegins(int turn)
        {
            SetTurnText(turn);
            SetGameState(GameState.initializing);

            if (NetworkManager.Instance.MeDone)
            {
                Debug.Log($"GameManager/I'm done, it's other's turn" + turn);
                return;
            }

            // request ownership
            if (CheckAllMine()) return;
            // these are non-blocking. wait for callback.
            CupManagerMulti.RequestCupOwnership();
            DiceManagerMulti.RequestDiceOwnership();
            Debug.Log("GameManager/My turn begins: #" + turn);


        }

        public static bool CheckAllMine()
        {
            bool isMine = CupManagerMulti.instance.GetComponent<CupManagerMulti>().IsMine;
            foreach (DiceScriptMulti dice in DiceManager.dices)
            {
                isMine = isMine & dice.IsMine;
            }
            IsReady = isMine;
            Debug.Log("GameManager/CheckAllMine:" + isMine);
            return isMine;
        }
        public void OnPlayerDiceResult(Player player, int turn, int[] results)
        {
            string msg = "on turn #" + turn + ", player " + player.ActorNumber + " rolled: ";
            foreach (int num in results) msg += num;
            Debug.Log(msg);
        }

        public void OnPlayerStrategySelected(Player player, int turn, int move, int score)
        {
            string msg = "on turn #" + turn + ", player " + player.ActorNumber + " selected: " + move;
            if(NetworkManager.Instance.MeDone)
            {
                OtherTotalPoints = ScoreTableMulti.UpdateOtherScoreTable(move,score);
            }
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
            // other's turn finished I take control
            NetworkManager.Instance.BeginTurn();
        }

        public void OnGameEnd(Player player, int turn)
        {
            int result = 1;
            if(MyTotalPoints>OtherTotalPoints) result = 2;
            else if(MyTotalPoints<OtherTotalPoints) result = 0;
            string msg;
            if(result == 2)
                msg = "You WIN";
            else if(result == 1)
                msg = "DRAW";
            else
                msg = "You LOSE";
            turnText.text = msg;
        }
    }
}

