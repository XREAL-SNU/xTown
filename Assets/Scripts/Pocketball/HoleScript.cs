using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JK
{
    public class HoleScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject ColorPanel;
        public GameObject A_Line;
        public GameObject A_Color;
        public GameObject Color_Balls;
        public GameObject Line_Balls;
        
        void OnTriggerEnter(Collider ball)
        {
            

            //구멍에 들어가면 해당 번호 GameManager에 기록
            int BallNum= int.Parse(ball.gameObject.name.Substring(5));

            //첫번째 공이 들어간 경우
            if(BallNum != 0)
            {
                if(GameManager.ballChoice==0)
                {
                    if(GameManager.AorB)
                    {
                        if(BallNum>=9 && BallNum<=15)
                        {
                            GameManager.CorL=false;
                            StartCoroutine(SetActiveObjInSecond(ColorPanel, 2f));
                            StartCoroutine(SetActiveObjInSecond(A_Line, 2f));
                            Color_Balls.SetActive(true);
                            Color_Balls.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -80, 0);
                            Line_Balls.SetActive(true);
                            Line_Balls.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 80, 0);

                        }
                        else if(BallNum>=1 && BallNum<=7)
                        {
                            StartCoroutine(SetActiveObjInSecond(ColorPanel, 2f));
                            StartCoroutine(SetActiveObjInSecond(A_Color, 2f));
                            Color_Balls.SetActive(true);
                            Line_Balls.SetActive(true);
                        }
                    }
                    else
                    {
                        if(BallNum>=1 && BallNum<=7)
                        {
                            GameManager.CorL=false;
                            StartCoroutine(SetActiveObjInSecond(ColorPanel, 2f));
                            StartCoroutine(SetActiveObjInSecond(A_Line, 2f));
                            Color_Balls.SetActive(true);
                            Color_Balls.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -80, 0);
                            Line_Balls.SetActive(true);
                            Line_Balls.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 80, 0);
                        }
                        else if(BallNum>=9 && BallNum<=15)
                        {
                            StartCoroutine(SetActiveObjInSecond(ColorPanel, 2f));
                            StartCoroutine(SetActiveObjInSecond(A_Color, 2f));
                            Color_Balls.SetActive(true);
                            Line_Balls.SetActive(true);
                        }
                    }
                    //Debug.Log(GameManager.CorL.ToString());
                    GameManager.ballChoice=1;
                }
            }




            for(int i=0; i<16; i++)
            {
                if(i==BallNum)
                {
                    GameManager.isBall[i]=1;
                    GameManager.isBallStop[i]=1;
                    if(i>=1 && i<=7)
                    {
                        Color_Balls.transform.Find("Ball_Image_"+i.ToString()).GetComponent<Image>().color = new Color(0.3f,0.3f,0.3f);
                    }
                    else if(i>=9 && i<=15)
                    {
                        Line_Balls.transform.Find("Ball_Image_"+i.ToString()).GetComponent<Image>().color = new Color(0.3f,0.3f,0.3f);
                    }
                    Debug.Log(i.ToString());
                }
            }

            //구멍에 들어가면 공 없어짐
            ball.gameObject.SetActive(false);
            //isBallStop에 영향 안가도록 1로 설정
            if(BallNum != 0)
            {
                GameManager.isBallStop[BallNum]=1;
            }
            else
            {
                GameManager.isBallStop[BallNum]=0;
                FreeBallScript.FreeBallBool = true;
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
                        GameManager.NothingBool = false;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.countB=GameManager.countB-1;
                        if(GameManager.NothingBool)
                        GameManager.OtherBool = true;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.OtherBool = true;
                    }
                }
                else
                {
                    //알맞게 넣었을 때
                    if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.countA=GameManager.countA-1;
                        GameManager.NothingBool = false;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.countB=GameManager.countB-1;
                        if(GameManager.NothingBool)
                        GameManager.OtherBool = true;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.OtherBool = true;
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
                        GameManager.NothingBool = false;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.countA=GameManager.countA-1;
                        if(GameManager.NothingBool)
                        GameManager.OtherBool = true;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.OtherBool = true;
                    }
                }
                else
                {
                    //알맞게 넣었을 때
                    if(BallNum>=1 && BallNum<=7)
                    {
                        GameManager.countB=GameManager.countB-1;
                        GameManager.NothingBool = false;
                    }
                    //다른 팀 것 넣었을 때
                    else if(BallNum>=9 && BallNum<=15)
                    {
                        GameManager.countA=GameManager.countA-1;
                        if(GameManager.NothingBool)
                        GameManager.OtherBool = true;
                    }
                    //흰 공 넣었을 때
                    else if(BallNum==0)
                    {
                        GameManager.OtherBool = true;
                    }
                }
            }

            //OnTriggerEnter 끝난 후



            
        }

        IEnumerator SetActiveObjInSecond(GameObject obj, float second)
        {
            obj.SetActive(true);

            yield return new WaitForSeconds(second);
            obj.SetActive(false);
        }




    }
}