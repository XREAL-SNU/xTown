using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace JK
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent BallStop;
        public UnityEvent GameOver;

        public static int[] isBall= new int[16]; //구멍에 들어간 공 목록 (들어가면 1)
        public static int[] isBallStop = new int[16]; // 공의 정지 여부

        public static int ballChoice = 0; //목적공 결정 여부
        
        public static bool[] Arraytrigger = new bool[16]; // 

        public static bool AorB= true; //현재 턴 (A 또는 B) -- A의 턴이면 true
        public static bool CorL= true; //목적공 (색공 또는 띠공) -- A가 색공이면 true

        public static int countA=8; //남은 공 개수
        public static int countB=8;

        int i=0; //GameOver 이벤트를 한 번만 부르기 위해 사용하는 변수

        // Start is called before the first frame update
        void Start()
        {
            isBallStop = new int[16];
            for(int j=0; j<16; j++)
            {
                Arraytrigger[j]=true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Ball 속도 모두 0일때
            if(isBallStop.Sum() == 16)
            {
                BallStop.Invoke();
                //Debug.Log(isBallStop.Sum());
            }
            //


            /*if(AorB)
             * //Update함수에다가 하면 안될듯. 그러면 각 공이 들어가서 바꾸자마자 다른게 실행되어서 유도하는 UI가 아님
            {
                //A팀 코딩
            }
            else
            {
                //B팀 코딩
            }*/



            //8번 공이 구멍에 들어간 경우 게임 종료
            if(i==0)
            {
                if(isBall[8]==1)
                {
                    GameOver.Invoke();
                    i=1;
                }
            }
        }
    }
}