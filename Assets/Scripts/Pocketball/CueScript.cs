using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class CueScript : MonoBehaviour
    {
        public GameObject Cue;
        public GameObject Whiteball;
        public GameObject Player;


        public static float PressTime;

        // Update is called once per frame
        void FixedUpdate()
        {
            PressTime = BallMovement.press_time;
            //Debug.Log(PressTime);
            Cue.transform.LookAt(Whiteball.transform);
            Cue.transform.position = new Vector3((Whiteball.transform.position.x*(float)(1.2-0.4*PressTime) + Player.transform.position.x*(float)(0.8+0.4*PressTime))/2,Whiteball.transform.position.y+0.2f, (float)(Whiteball.transform.position.z*(float)(1.2-0.4*PressTime) + Player.transform.position.z*(0.8+0.4*PressTime))/2);
            
            //Debug.Log(BallMovement.CueBool);
            if(BallMovement.CueBool)
            {
                Cue.SetActive(true);
            }
            else
            {
                Cue.SetActive(false);
            }
        }
    }
}
