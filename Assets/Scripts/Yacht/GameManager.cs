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
    public enum GameState
    {
        initializing,
        ready,
        shaking,
        pouring,
        rolling,
        selecting,
        waiting,
        finish
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static Quaternion[] rotArray = new Quaternion[6];
        public static int turnCount = 1;
        public static bool rollTrigger = false;
        public static GameState currentGameState = GameState.initializing;

        public UnityEvent onInitialize;
        public UnityEvent onReadyStart;
        public UnityEvent onReadyToSelect;
        public UnityEvent onShakingStart;
        public UnityEvent onPouringStart;
        public UnityEvent onRollingStart;
        public UnityEvent onRollingFinish;
        public UnityEvent onFinish;


        protected bool readyTrigger = false;


        protected void Awake()
        {
            if (instance == null)
            {
                instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            /*
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
            */

            /* dice rotatons */
            rotArray[0] = Quaternion.Euler(90f, 0f, 0f);
            rotArray[1] = Quaternion.Euler(0f, 0f, 0f);
            rotArray[2] = Quaternion.Euler(0f, 90f, 90f);
            rotArray[3] = Quaternion.Euler(0f, 0f, -90f);
            rotArray[4] = Quaternion.Euler(180f, 0f, 0f);
            rotArray[5] = Quaternion.Euler(-90f, 90f, 0f);
        }

        
        // Update is called once per frame
        protected virtual void Update()
        {
            if (currentGameState == GameState.initializing)
            {
                SetGameState(GameState.ready);
                turnCount = 1;

                onInitialize.Invoke();
                readyTrigger = true;
            }

            // ready
            if (currentGameState == GameState.ready && readyTrigger)
            {
                onReadyStart.Invoke();
                readyTrigger = false;
            }

            if (Input.GetKey(KeyCode.X) && currentGameState == GameState.ready && turnCount > 1 && CupManager.playingAnim == false)
            {
                CupManager.playingAnim = true;
                SetGameState(GameState.selecting);
                onReadyToSelect.Invoke();
                Debug.Log("ready to selecting");
            }

            if (Input.GetKeyDown(KeyCode.Space) && currentGameState == GameState.ready && CupManager.playingAnim == false)
            {
                bool moreThanOne = DiceScript.diceInfoList.Any(x => x.keeping == false);

                if (moreThanOne)
                {
                    SetGameState(GameState.shaking);
                    onShakingStart.Invoke();
                }

            }

            // shaking���� �����̽��� ���� pouring���� ��ȯ.
            if (Input.GetKeyUp(KeyCode.Space) && currentGameState == GameState.shaking)
            {
                SetGameState(GameState.pouring);
                onPouringStart.Invoke();
            }

            // rolling���� �ٲ�� ����
            if (currentGameState == GameState.rolling && rollTrigger)
            {
                rollTrigger = false;
                onRollingStart.Invoke();
            }


            bool rollingFinished = !DiceScript.diceInfoList.Any(x => x.diceNumber == 0);

            if (currentGameState == GameState.rolling && rollingFinished)
            {
                SetGameState(GameState.selecting);
                onRollingFinish.Invoke();
                turnCount += 1;
            }

            if (currentGameState == GameState.selecting && turnCount > 4)
            {
                SetGameState(GameState.finish);
                onFinish.Invoke();
            }

            if (Input.GetKey(KeyCode.X) && currentGameState == GameState.selecting && turnCount <= 3 && CupManager.playingAnim == false)
            {
                bool moreThanOne = DiceScript.diceInfoList.Any(x => x.keeping == false);

                if (moreThanOne)
                {
                    SetGameState(GameState.ready);
                    readyTrigger = true;
                }
            }
        }
        
        

        public static void SetGameState(GameState newGameState)
        {
            if (Enum.IsDefined(typeof(GameState), newGameState))
            {
                currentGameState = newGameState;
                Debug.Log("game state update:" + newGameState);
            }
            
        }


    }
}

