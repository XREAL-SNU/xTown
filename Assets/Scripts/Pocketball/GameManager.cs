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

        public static Vector3 whitePosition;
        public static int[] isBall= new int[16];
        public static int[] isBallStop = new int[16];

        public static int ballChoice = 0; //첫 공 들어가서 공 색깔 정해졌는지 생각
        
        public static bool[] Arraytrigger = new bool[16];

        public static bool AorB= true; //현재 A의 턴인지 B의 턴인지 판단하는 bool 
        public static bool CorL= true; //띠 공인지 색 공인지 -- A가 색공이면 true(1회용)

        public static int countA=8;
        public static int countB=8;

        int i=0;
        // Start is called before the first frame update
        void Start()
        {
            isBallStop = new int[16];
            for(int j=0; j<16; j++)
            {
                Arraytrigger[j]=true;
            }
            Physics.bounceThreshold = 0.2f;
            Physics.sleepThreshold = 0.015f;
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


            /*if(AorB)        //Update함수에다가 하면 안될듯. 그러면 각 공이 들어가서 바꾸자마자 다른게 실행되어서 유도하는 UI가 아님
            {
                //A팀 코딩
            }
            else
            {
                //B팀 코딩
            }*/








            //8번이 충돌 bool=true;
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