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
        public static Vector3 ballPosition;
        public static Vector3 ballDirection;
        public Vector3 powerPosition;

        public static float power; //공 이동 힘 크기
        private float start_time; //마우스 클릭 시작한 시간
        private float bar_time; //마우스 놓은 시간
        private float press_time; //마우스 누르고 있던 시간
        private float end_time;

        int BallNum;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            BallNum = int.Parse(gameObject.name.Substring(5));
            power = 0; //파워 초기화

        }

        // Update is called once per frame
        void Update()
        {
            ballPosition = transform.position;
            ballVelocity = rb.velocity;
            if (ballVelocity == Vector3.zero && GameManager.Arraytrigger[BallNum]) //공이 완전히 멈췄을 때
            {
                //Debug.Log("Hi"+BallNum.ToString());
                GameManager.isBallStop[BallNum] = 1; //공이 멈췄음을 표시
                GameManager.Arraytrigger[BallNum] = false;
            }

        }

        private void OnMouseDown() //마우스 눌렀을 때
        {
            power = 0; //파워 리셋
            if (BallNum == 0) //흰 공일때
            {
                start_time = Time.time;
            }
        }

        private void OnMouseDrag() //마우스 누르고 있을 때
        {
            bar_time = Time.time;
            press_time = bar_time - start_time;
            if (press_time > 2) //최댓값 제한
            {
                press_time = 2;
            }
            //PowerBarScript.bar_width = (float)143.5 * (press_time);
        }

        private void OnMouseUp() //마우스를 땠을 때
        {
            Debug.Log(GameManager.isBallStop.Sum()); // 공이 없어지면서 뭔가 문제가 생긴듯
            if (GameManager.isBallStop.Sum() == 16) //모든 공이 완전히 멈춘 경우
            {
                if (BallNum == 0) //흰 공인 경우
                {
                    end_time = Time.time;

                    //공 이동 방향 설정
                    ballDirection = transform.position - CueScript.cuePosition;
                    ballDirection.y = 0; //땅 쪽으로 힘 받으면 튀니까 제어
                    //rb.AddForce(Camera.main.transform.forward * 1000);

                    //공 이동 힘 크기 설정
                    power = 500 * (end_time - start_time);
                    if (power > 1000) //최댓값 제한
                    {
                        power = 1000;
                    }

                    //공 이동
                    //rb.AddForce(ballDirection * power);
                    CueScript.isCueMoving = true; //큐대 이동 시작

                    //모든 공을 움직이는 상태로 표시 변경
                    for (int i = 0; i < 16; i++)
                    {
                        GameManager.isBallStop[i] = 0;
                    }
                    // isBallStop 디버깅용
                    foreach (var human in GameManager.isBallStop)
                    {
                        Debug.Log(human);
                    }

                    //Array Trigger 초기화
                    for (int j = 0; j < 16; j++)
                    {
                        GameManager.Arraytrigger[j] = true;
                    }
                }

            }
        }
    }
}