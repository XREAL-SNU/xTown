using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class BallMovement : MonoBehaviour
    {
        Rigidbody rb;
        
        public Vector3 ballVelocity;
        public Vector3 ballDirection;
        public Vector3 powerPosition;
        public static float TimePress;
        public static bool CueBool;
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
            BallNum=int.Parse(gameObject.name.Substring(5));
            power=0;
            CueBool = true;
        }

        // Update is called once per frame
        void Update()
        {
            ballVelocity=rb.velocity;
            if(Mathf.Abs(ballVelocity.x) <= 0.001 && Mathf.Abs(ballVelocity.z) <= 0.001) // 공이 멈췄을 때
            {
                //Debug.Log("hi"+BallNum.ToString());
                GameManager.isBallStop[BallNum]=1;
            }
            else
            {
                GameManager.isBallStop[BallNum]=0;
            }
            
            if(BallNum==0)
            {
                GameManager.whitePosition = transform.position;
            }
            /*if(GameManager.isBallStop.Sum()==16)
            {
                CueBool=true;
            }*/
            TimePress=press_time;
        }

        void OnMouseDown()
        {
            power=0;
            if(BallNum==0) //흰공일때
            {
                start_time = Time.time;
            }
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
            
            //Debug.Log(GameManager.isBallStop[0]); // 공이 없어지면서 뭔가 문제가 생긴듯
            if(GameManager.isBallStop.Sum()==16) // 모든 공이 완전히 멈췄을 때
            {
                //Debug.Log(GameManager.isBallStop.Sum());
                if(BallNum==0)
                {
                    //지우기
                    CueBool = false;

                    //흰공 방향 결정하자.
                    ballDirection=transform.position-PlayerScript.playerPosition;
                    ballDirection.y=0;                
                    
                    //시간 받아오기
                    end_time = Time.time;
                    
                    //힘 크기 결정 (500*누른 시간)
                    power=500*(end_time - start_time);

                    //최대 힘 500
                    if(power>500)
                    {
                        power=500;
                    }

                    //힘 가함 -> 누를 수록 늘어나도록 하자.
                    rb.AddForce(ballDirection*power);
                    //이거 isBallStop 버그 수정할 때 사용

                    /*foreach (var human in GameManager.isBallStop)
                    {
                        Debug.Log(human);
                    }*/

                    //큐대 reset
                    CueScript.PressTime = 0;
                    

                    StartCoroutine(Wait(1f));
                    
                    GameManager.line.enabled = false;
                    //아무것도 들어가지 않았을 때 거르기 위함 or 다른 팀 or 흰공
                    GameManager.NothingBool = true;

                    //CamBool
                    GameManager.CamBool = false;
                }
            }
        }

        IEnumerator Wait(float value)
        {
            yield return new WaitForSeconds(value);
            GameManager.currentGameState = GameManager.GameState.Rolling;
        }
    }


    
}