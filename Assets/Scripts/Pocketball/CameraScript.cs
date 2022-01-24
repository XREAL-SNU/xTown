using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace JK
{
    public class CameraScript : MonoBehaviour
    {
        public CinemachineVirtualCamera virCam = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {
            //Debug.Log(GameManager.Arraytrigger[0]);
            if(GameManager.isBallStop.Sum()==16)
            {
                virCam.Priority = 9;
            }
            else
            {
                virCam.Priority = 11;
            }
        
        }
    }
}