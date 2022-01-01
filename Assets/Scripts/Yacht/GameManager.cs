using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;

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

    public class GameManager : MonoBehaviour, IPunTurnManagerCallbacks
    {
        public static GameManager instance;
        public static Quaternion[] rotArray = new Quaternion[6];
        public static int turnCount = 1;
        public static bool rollTrigger = false;
        public static GameState currentGameState = GameState.initializing;


        

        /* events: set callbacks in inspector */
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

        /* multiplay */
        // set this to true to enable multiplay. you must build again. 
        // still in experimental stage.
        public static bool multiplayMode = false;

        public static int currentTurn = 0;
        public TakeTurns turnManager;

        [SerializeField]
        private NetworkDebugger _debugger;
        [SerializeField]
        private GameObject _waitingPanel;


        /* Monobehaviour callbacks */


        void Awake()
        {
            /*
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
            */
            rotArray[0] = Quaternion.Euler(90f, 0f, 0f);
            rotArray[1] = Quaternion.Euler(0f, 0f, 0f);
            rotArray[2] = Quaternion.Euler(0f, 90f, 90f);
            rotArray[3] = Quaternion.Euler(0f, 0f, -90f);
            rotArray[4] = Quaternion.Euler(180f, 0f, 0f);
            rotArray[5] = Quaternion.Euler(-90f, 90f, 0f);
        }

        /* MonoBehaviour callbacks */
        void Start()
        {
            /* multiplay */
            if (multiplayMode && PhotonNetwork.IsMasterClient)
            {
                initializeTrigger = true;
            }
            else if(!multiplayMode)
            {
                _waitingPanel.SetActive(false);
            }
        }



        void Update()
        {
            /* multiplayer testing */
            // check for my turn
            if (multiplayMode && (!IsMyTurn || currentTurn <= 0)) return;

            if (Input.GetKeyDown(KeyCode.F)) // a special event
            {
                int rand = UnityEngine.Random.Range(0, 100);
                turnManager.SendMove(rand, true);
            }
            if (Input.GetKeyDown(KeyCode.S)) // an ordinary event
            {
                int rand = UnityEngine.Random.Range(0, 100);
                turnManager.SendMove(rand, false);
            }

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


            //shaking에서 스페이스바 떼면 pouring으로 전환.
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

            // 모든 주사위가 rolling이 끝나면 selecting으로 전환
            bool rollingFinished = !DiceScript.diceInfoList.Any(x => x.diceNumber == 0);

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


        /* Multiplayer only */
        void OnEnable()
        {
            
            if (!multiplayMode) return;
            Debug.Log("Yacht/GameManager: my actorNum" + PhotonNetwork.LocalPlayer.ActorNumber);
            _debugger.NetworkDebug("Yacht/GameManager: my actorNum" + PhotonNetwork.LocalPlayer.ActorNumber);
            //CupManager.DisableCupAnims();
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                Debug.Log("Yacht/GameManager: It's my turn at beginning!");
                IsMyTurn = true;
            }
            else
            {
                Debug.Log("Yacht/GameManager: It's other player's turn at beginning!");
                IsMyTurn = false;
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                _debugger.NetworkDebug("starting game!");
                turnManager.BeginTurn();
            }
            
        }

        /* public methods */
        public static void SetGameState(GameState newGameState)
        {
            if (Enum.IsDefined(typeof(GameState), newGameState))
            {
                currentGameState = newGameState;
                Debug.Log("state: " + newGameState);
            }
            Debug.Log(IsMyTurn ? "and still my turn" : " not my turn");
        }

        /* Turn Listener callbacks */
        public void OnTurnBegins(int turn)
        {
            if (turn == 1)
            {
                _waitingPanel.SetActive(false);
                if(IsMyTurn) CupManager.EnableCupAnims();
            }
            Debug.Log("Turn " + turn + "begins!");
            currentTurn = turn;
        }

        public void OnTurnCompleted(int turn)
        {
            Debug.Log("Turn " + turn + "ends");
            if (PhotonNetwork.IsMasterClient)
                turnManager.BeginTurn();
        }

        public void OnPlayerMove(Player player, int turn, object move)
        {
            int mv = (int)move;
            Debug.Log("player" + player.ActorNumber + "has made a move " + mv);
        }

        /* my turn script */

        public static bool IsMyTurn;

        // When a player finishes a turn (includes the move of that player)
        public void OnPlayerFinished(Player player, int turn, object move)
        {
            if (player.ActorNumber < 0)
            {
                Debug.LogError("invalid actor number");
                return;
            }
            int mv = (int)move;
            
            if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log("(my)player: " + player.ActorNumber + "'s turn has ended!"  + mv);
                _debugger.NetworkDebug("(my)player: " + player.ActorNumber + "'s turn has ended!" + mv);
                IsMyTurn = false;
            }
            else
            {
                Debug.Log("(other)player: " + player.ActorNumber + "'s turn has ended!" + mv);
                _debugger.NetworkDebug("(other)player: " + player.ActorNumber + "'s turn has ended!" + mv);
                IsMyTurn = true;
            }
        }

    }
}

