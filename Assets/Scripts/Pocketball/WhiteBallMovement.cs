using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace JK
{
    public class WhiteBallMovement : MonoBehaviour
    {
        Rigidbody rb;
        public Vector3 whiteBallVelocity;
        public Vector3 whiteBallDirection;
        public Vector3 powerPosition;
        public CameraScript _cameraScript;
        public static float TimePress;
        public static bool CueBool;
        public PhotonView _view;
        public PhotonTransformView _transformView;
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
            _transformView = GetComponent<PhotonTransformView>();
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
            PowerBarScript.bar_width = (float)478*(press_time);
        }


        void OnMouseUp() //마우스를 눌렀다가 뗐을 때
        {
            if(!FreeBallScript.FreeBallBool)
            {
                //Debug.Log(GameManager.isBallStop[0]); // 공이 없어지면서 뭔가 문제가 생긴듯
                if(GameManager.isBallStop.Sum()==16) // 모든 공이 완전히 멈췄을 때
                {
                    CueBool = false;
                    whiteBallDirection=transform.position-PlayerScript.playerPosition;
                    whiteBallDirection.y=0;                
                    end_time = Time.time;
                    power=500*(end_time - start_time);
                    if(power>500)
                    {
                        power=500;
                    }
                    moveBall(whiteBallDirection,power);
                    CueScript.PressTime = 0;
                    StartCoroutine(Wait(1f));      
                    GameManager.line.enabled = false;
                    //아무것도 들어가지 않았을 때 거르기 위함 or 다른 팀 or 흰공
                    GameManager.NothingBool = true;
                    _cameraScript.SetCameraWhole();
                }
            }
        }

        IEnumerator Wait(float value)
        {
            yield return new WaitForSeconds(value);
            GameManager.currentGameState = GameManager.GameState.Rolling;
        }

        [PunRPC]
        void moveBall(Vector3 ballDirection,float power)
        {
            rb.AddForce(ballDirection*power);
        }
    }


    
}