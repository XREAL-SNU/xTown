using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
namespace JK
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState
        {
            Rolling,
            Stopped
        }

        public static GameState currentGameState;
        public GameObject A_Text;
        public GameObject B_Text;
        public GameObject Turn_Panel;
        public GameObject GameOver_Panel;
        public GameObject GameOver_A_Win;
        public GameObject GameOver_B_Win;
        public GameObject WhiteBall;
        public GameObject A_Turn;
        public GameObject B_Turn;
        public CueScript _cueScript;
        public BallMovement[] _ballMovement;
        public HoleScript[] _holeScript;
        public WhiteBallMovement _whiteBallMovement;
        public DrawTrajectory _trajectory;
        public CameraLocation _cameraLocation;
        public PhotonView _view;
        public static Vector3 whitePosition;
        public static int[] isBall= new int[16];
        public static int[] isBallStop = new int[16];

        public static int ballChoice = 0; //첫 공 들어가서 공 색깔 정해졌는지 생각
        
        public static bool CamBool;
        public static bool NothingBool; // 아무것도 안 들어감
        public static bool OtherBool; // 다른 공 or 흰공

        public static bool AorB= true; //현재 A의 턴인지 B의 턴인지 판단하는 bool 
        public static bool CorL= true; //띠 공인지 색 공인지 -- A가 색공이면 true(1회용)

        public static int countA=8;
        public static int countB=8;

        public static bool Started = false;
        public static LineRenderer line;
        int i=0;
        CameraScript _cameraScript;
        // Start is called before the first frame update
        void Start()
        {
            currentGameState = GameState.Stopped;
            isBallStop = new int[16];
            Physics.bounceThreshold = 0.2f;
            Physics.sleepThreshold = 0.015f;
            _cameraScript = GetComponent<CameraScript>();
            _cameraScript.SetCameraCue();
            //CamBool = true;
            NothingBool = true;
            OtherBool = false;
            line = WhiteBall.GetComponent<LineRenderer>();
            _whiteBallMovement.enabled =false;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(isBallStop.Sum());

            //GameOver 8번이 충돌 bool=true;
            if(i==0 && Started)
            {
                if(isBall[8]==1 && countA !=0 && countB !=0)
                {
                    GameOver_Panel.SetActive(true);
                    if(AorB)
                    {
                        GameOver_B_Win.SetActive(true);
                    }
                    else
                    {
                        GameOver_A_Win.SetActive(true);
                    }
                }
                else if(countA ==0 && isBall[8]==1)
                {
                    GameOver_Panel.SetActive(true);
                    GameOver_A_Win.SetActive(true);
                }
                else if(countB == 0 && isBall[8] == 1)
                {
                    GameOver_Panel.SetActive(true);
                    GameOver_B_Win.SetActive(true);
                }
            }
        }
        void FixedUpdate()
        {
            //UI
            if(Started && isBallStop.Sum() == 16 && currentGameState == GameState.Rolling && !FreeBallScript.FreeBallBool)
            {
                if(PocketDyeNetworkManager.Instance.networked && _view.IsMine)
                _view.RPC("CueCameraSetting",RpcTarget.All);
                else if(!PocketDyeNetworkManager.Instance.networked)
                CueCameraSetting();
                //아무것도 안 들어갔을 때 턴 넘겨짐 or 다른 팀(넣더라도 자기거 먼저넣으면 okay) or 흰공
                if(NothingBool||OtherBool)
                {
                    if(PocketDyeNetworkManager.Instance.networked && _view.IsMine)
                    {
                        AorB = !AorB;
                        _view.RPC("SwitchOwnerShip",RpcTarget.Others);
                        //Debug.Log("SwitchOwnership");
                    }
                    else if(!PocketDyeNetworkManager.Instance.networked)
                    {
                        AorB = !AorB;
                    }
                }
                if(!PocketDyeNetworkManager.Instance.networked)
                AfterOwnerShip();
                else if(_view.IsMine)
                {
                    _view.RPC("AfterOwnerShip",RpcTarget.All);
                }
                //CamBool = true;                
                //Debug.Log("A: "+countA+", B: "+countB);            
            }
        }

        IEnumerator SetActiveObjInSecond(GameObject obj, float second)
        {
            obj.SetActive(true);

            yield return new WaitForSeconds(second);
            obj.SetActive(false);
        }
        [PunRPC]
        public void GameStart()
        {
            _whiteBallMovement.enabled = true;
            StartCoroutine(SetActiveObjInSecond(Turn_Panel, 2f));
            //지금은 무조건 A부터인데, 가위바위보(?)를 넣어서 순서 정하게 하기. 여기에 적용.
            StartCoroutine(SetActiveObjInSecond(A_Text, 2f));
            line.enabled = true;
            WhiteBallMovement.CueBool = true;
            _view = GetComponent<PhotonView>();
            Started = true;
        }


        [PunRPC]
        void CueCameraSetting()
        {
            currentGameState = GameState.Stopped;
            _cameraScript.SetCameraCue();   
        }

        [PunRPC]
        void SwitchOwnerShip()
        {
            AorB = !AorB;
            _cueScript._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            for(int i=0; i<6; i++)
            {
                _holeScript[i]._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber); 
            }
            for(int i=0; i<15; i++)
            {
                _ballMovement[i]._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber); 
            }           
            _whiteBallMovement._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            _cameraLocation._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            _trajectory._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            _view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        [PunRPC]
        void AfterOwnerShip()
        {
            NothingBool = true;
            OtherBool = false;
                //라인 렌더러 & 큐 활성화
            line.enabled = true;
            WhiteBallMovement.CueBool = true;
            WhiteBallMovement.press_time = 0;
            StartCoroutine(SetActiveObjInSecond(Turn_Panel, 2f));
            if(AorB)
            {
                StartCoroutine(SetActiveObjInSecond(A_Text, 2f));
                A_Turn.SetActive(true);
                B_Turn.SetActive(false);
            }
            else
            {
                StartCoroutine(SetActiveObjInSecond(B_Text, 2f));
                A_Turn.SetActive(false);
                B_Turn.SetActive(true);
            }        
        }    
    }
}