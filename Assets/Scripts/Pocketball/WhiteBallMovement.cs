using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
namespace JK
{
    public class WhiteBallMovement : MonoBehaviour
    {
        Rigidbody rb;
        public Vector3 whiteBallVelocity;
        public Vector3 whiteBallDirection;
        public Vector3 powerPosition;
        public CameraScript _cameraScript;
        public CameraLocation _cameraLocation;
        public static float TimePress;
        public static bool CueBool;
        public PhotonView _view;
        private float power;
        private float start_time;
        private float bar_time;
        public static float press_time;
        private float end_time;

        int BallNum;
        //private bool trigger = true;

        // Start is called before the first frame update
        void Start()
        {
            rb=GetComponent<Rigidbody>();
            _view = GetComponent<PhotonView>();
            BallNum=0;
            power=0;
            CueBool = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            whiteBallVelocity=rb.velocity;
            if(Mathf.Abs(whiteBallVelocity.x) <= 0.0015 && Mathf.Abs(whiteBallVelocity.z) <= 0.0015) // 공이 멈췄을 때
            {
                //Debug.Log("hi"+BallNum.ToString());
                GameManager.isBallStop[BallNum]=1;
            }
            else
            {
                GameManager.isBallStop[BallNum]=0;
            }
            GameManager.whitePosition = transform.position;
            TimePress=press_time;
        }

        void OnMouseDown()
        {
            power=0;
            start_time = Time.time;
        }

        void OnMouseDrag()
        {
            bar_time = Time.time;
            press_time=bar_time-start_time;
            if(press_time>1)
            {
                press_time=1;
            }
            if(!PocketDyeNetworkManager.Instance.networked)
            {
                SetBarWidth(press_time);
            }
            else if(PocketDyeNetworkManager.Instance.networked && _view.IsMine)
            {
                _view.RPC("SetBarWidth",RpcTarget.All,press_time);
            }
        }


        void OnMouseUp() //마우스를 눌렀다가 뗐을 때
        {
            if(!FreeBallScript.FreeBallBool)
            {
                //Debug.Log(GameManager.isBallStop[0]); // 공이 없어지면서 뭔가 문제가 생긴듯
                if(GameManager.isBallStop.Sum()==16) // 모든 공이 완전히 멈췄을 때
                {
                    CueBool = false;
                    whiteBallDirection=transform.position-_cameraLocation.transform.position;
                    whiteBallDirection.y=0;
                    whiteBallDirection.x=Mathf.Round(whiteBallDirection.x);
                    whiteBallDirection.z=Mathf.Round(whiteBallDirection.z);                
                    end_time = Time.time;
                    power=500*(end_time - start_time);
                    power=Mathf.Round(power);
                    if(power>500)
                    {
                        power=500;
                    }
                    if(PocketDyeNetworkManager.Instance.networked && _view.IsMine)
                    {
                        BallForce(whiteBallDirection,power);
                        _view.RPC("WaitCamera",RpcTarget.All);
                    }
                    else if(!PocketDyeNetworkManager.Instance.networked)
                    {
                        BallForce(whiteBallDirection,power);
                        WaitCamera();
                    }
                    StartCoroutine(Wait(1f)); 
                    //아무것도 들어가지 않았을 때 거르기 위함 or 다른 팀 or 흰공
                }
            }
        }
        IEnumerator Wait(float value)
        {
            yield return new WaitForSeconds(value);
            GameManager.currentGameState = GameManager.GameState.Rolling;
        }
        [PunRPC]
        void WaitCamera()
        {
            _cameraScript.SetCameraWhole();
            GameManager.NothingBool = true;
            press_time = 0;
            GameManager.line.enabled = false;
        }
        [PunRPC]
        void BallForce(Vector3 ballDirection,float ballPower)
        {
            rb.AddForce(ballDirection*ballPower);
        }
        [PunRPC]
        void SetBarWidth(float pressTime)
        {
            PowerBarScript.bar_width = (float)478*(pressTime);
        }    
    }
}