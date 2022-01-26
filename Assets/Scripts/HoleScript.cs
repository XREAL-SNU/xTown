using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class HoleScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        void OnTriggerEnter(Collider ball)
        {
            /* --- 공이 구멍에 들어간 경우 --- */
            int BallNum = int.Parse(ball.gameObject.name.Substring(5)); //구멍에 들어간 공 번호
            for (int i=0; i<16; i++) //구멍에 들어간 공을 GameManager에 기록
            {
                if(i==BallNum)
                {
                    GameManager.isBall[i]=1;
                    Debug.Log(i.ToString());
                }
            }
            GameManager.isBallStop[BallNum]=1; // 구멍에 들어간 공은 멈춘 것으로 표시
            ball.gameObject.SetActive(false); //구멍에 들어간 공은 사라짐(비활성화)


            /* --- 브레이크: 구멍에 처음으로 공이 들어간 경우 --- */
            if (GameManager.ballChoice==0) //아직 목적공이 정해지지 않은 상태
            {
                if(GameManager.AorB) //A의 턴인 경우
                {
                    if(BallNum>=9 && BallNum<=15) //들어간 공이 띠공인 경우
                    {
                        GameManager.CorL=false; //A의 목적공은 띠공
                    }
                }
                else //B의 턴인 경우
                {
                    if(BallNum>=1 && BallNum<=7) //들어간 공이 색공인 경우
                    {
                        GameManager.CorL=false; //A의 목적공은 띠공
                    }
                }
                Debug.Log(GameManager.CorL.ToString());
                GameManager.ballChoice=1; //목적공 결정 완료
            }

            /* --- 구멍에 넣은 공이 알맞은 목적공인지 확인 --- */
            if(GameManager.AorB) //A의 턴인 경우
            {
                if(GameManager.CorL) //A의 목적공이 색공인 경우
                {
                    //알맞게 넣었을 때
                    if(BallNum>=1 && BallNum<=7) //색공인 경우
                    {
                        GameManager.countA=GameManager.countA-1; //남은 공 개수 1 차감
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=9 && BallNum<=15) //띠공인 경우
                    {
                        GameManager.AorB=false; //턴 전환
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.AorB=false; //턴 전환
                    }
                }
                else //A의 목적공이 띠공인 경우
                {
                    //알맞게 넣었을 때
                    if(BallNum>=9 && BallNum<=15) //띠공인 경우
                    {
                        GameManager.countA=GameManager.countA-1; //남은 공 개수 1 차감
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=1 && BallNum<=7) //색공인 경우
                    {
                        GameManager.AorB=false; //턴 전환
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.AorB=false; //턴 전환
                    }
                }
            }
            else //B의 턴인 경우
            {
                if(GameManager.CorL) //A의 목적공이 색공인 경우
                {
                    //알맞게 넣었을 때
                    if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.countB=GameManager.countB-1;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.AorB=true;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.AorB=true;
                    }
                }
                else //A의 목적공이 띠공인 경우
                {
                    //알맞게 넣었을 때
                    if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.countB=GameManager.countB-1;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.AorB=true;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.AorB=true;
                    }
                }
            }

            //OnTriggerEnter 끝난 후


            
        }


    }
}