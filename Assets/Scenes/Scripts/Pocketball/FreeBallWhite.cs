using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class FreeBallWhite : MonoBehaviour
    {
        void OnTriggerEnter(Collider ball)
        {
            if(ball.gameObject.name.Substring(0,4) == "Ball")
            {
                FreeBallScript.ColliderBool  = true;
            }
        }

        void OnTriggerExit (Collider ball)
        {
            if(ball.gameObject.name.Substring(0,4) == "Ball")
            {
                FreeBallScript.ColliderBool  = false;
            }
        }
    }   
}

