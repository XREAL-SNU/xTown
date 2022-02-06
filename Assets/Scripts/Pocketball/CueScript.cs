using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class CueScript : MonoBehaviour
    {
        public GameObject Cue;
        public GameObject Whiteball;

        public static float PressTime;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            PressTime = BallMovement.TimePress;
            Cue.transform.LookAt(Whiteball.transform);
            Cue.transform.position = new Vector3(GameManager.whitePosition.x*(float)(1.2-0.4*PressTime) + PlayerScript.playerPosition.x*(float)(0.8+0.4*PressTime)/2,GameManager.whitePosition.y+0.4f, (float)(GameManager.whitePosition.z*(float)(1.2-0.4*PressTime) + PlayerScript.playerPosition.z*(0.8+0.4*PressTime))/2);
            
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
