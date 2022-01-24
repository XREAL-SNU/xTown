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
            //구멍에 들어가면 해당 번호 GameManager에 기록
            int BallNum= int.Parse(ball.gameObject.name.Substring(5));
            for(int i=0; i<16; i++)
            {
                if(i==BallNum)
                {
                    GameManager.isBall[i]=1;
                    Debug.Log(i.ToString());
                }
            }

            GameManager.isBallStop[BallNum]=1;
            //구멍에 들어가면 공 없어짐
            ball.gameObject.SetActive(false);

            //첫번째 공이 들어간 경우
            if(GameManager.ballChoice==0)
            {
                if(GameManager.AorB)
                {
                    if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.CorL=false;
                    }
                }
                else
                {
                    if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.CorL=false;
                    }
                }
                //Debug.Log(GameManager.CorL.ToString());
                GameManager.ballChoice=1;
            }

            //각자 팀에 맞는 공을 넣었는지 확인

            //A팀인 경우
            if(GameManager.AorB)
            {
                if(GameManager.CorL)
                {
                    //알맞게 넣었을 때
                    if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.countA=GameManager.countA-1;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=9 && BallNum<=15)
                    {

                        GameManager.AorB=false;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {

                        GameManager.AorB=false;
                    }
                }
                else
                {
                    //알맞게 넣었을 때
                    if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.countA=GameManager.countA-1;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=1 && BallNum<=7)
                    {

                        GameManager.AorB=false;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {

                        GameManager.AorB=false;
                    }
                }
            }

            //B팀인 경우
            else
            {
                if(GameManager.CorL)
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
                else
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