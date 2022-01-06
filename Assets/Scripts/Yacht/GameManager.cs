using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


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

        private bool initializeTrigger = false;
        private bool readyTrigger = false;
        private float posX = 1.4f;
        private float posY = 7.0f;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }

            rotArray[0] = Quaternion.Euler(90f, 0f, 0f);
            rotArray[1] = Quaternion.Euler(0f, 0f, 0f);
            rotArray[2] = Quaternion.Euler(0f, 90f, 90f);
            rotArray[3] = Quaternion.Euler(0f, 0f, -90f);
            rotArray[4] = Quaternion.Euler(180f, 0f, 0f);
            rotArray[5] = Quaternion.Euler(-90f, 90f, 0f);
        }

        // Start is called before the first frame update
        void Start()
        {
            initializeTrigger = true;

        }

        // Update is called once per frame
        void Update()
        {
            // 주사위 및 기타 변수 초기화 후 ready로 이동
            if (currentGameState == GameState.initializing)
            {
                SetGameState(GameState.ready);
                turnCount = 1;
                onInitialize.Invoke();
                initializeTrigger = false;
                readyTrigger = true;
                // ShownSlot 초기화도 onInitialize 이벤트에 추가해야 함
            }

            // ready
            if (currentGameState == GameState.ready && readyTrigger)
            {
                onReadyStart.Invoke();
                readyTrigger = false;
            }

            // ready에서 X 누르면 selecting으로 전환. 이건 첫 번째 주사위 굴릴 때는 불가능
            if (Input.GetKey(KeyCode.X) && currentGameState == GameState.ready && turnCount > 1 && CupManager.playingAnim == false)
            {
                CupManager.playingAnim = true;
                SetGameState(GameState.selecting);
                onReadyToSelect.Invoke();
                Debug.Log("ready to selecting");
            }

            // ready에서 스페이스바 누르면 shaking으로 전환.
            if (Input.GetKeyDown(KeyCode.Space) && currentGameState == GameState.ready && CupManager.playingAnim == false)
            {
                bool moreThanOne = DiceScript.diceInfoList.Any(x => x.keeping == false);

                if (moreThanOne)
                {
                    SetGameState(GameState.shaking);
                    onShakingStart.Invoke();
                }

            }

            // shaking에서 스페이스바 떼면 pouring으로 전환.
            if (Input.GetKeyUp(KeyCode.Space) && currentGameState == GameState.shaking)
            {
                SetGameState(GameState.pouring);
                onPouringStart.Invoke();
            }

            // rolling으로 바뀌면 실행
            if (currentGameState == GameState.rolling && rollTrigger == true)
            {
                rollTrigger = false;
                onRollingStart.Invoke();
            }


            bool rollingFinished = !DiceScript.diceInfoList.Any(x => x.diceNumber == 0);

            // 모든 주사위가 rolling이 끝나면 selecting으로 전환
            if (currentGameState == GameState.rolling && rollingFinished)
            {
                SetGameState(GameState.selecting);
                onRollingFinish.Invoke();
                turnCount += 1;
            }

            // 3번 다 던지고 난 후에는 selecting에서 finish로 전환
            if (currentGameState == GameState.selecting && turnCount > 3)
            {
                SetGameState(GameState.finish);
                onFinish.Invoke();
            }

            // selecting 단계에서 X 누르면 ready 단계로 전환. 이건 주사위 세 번 다 굴리면 불가능
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
            }
        }
    }
}

