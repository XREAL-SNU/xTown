using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace JK
{
    public class CueScript : MonoBehaviour
    {
        public GameObject Cue;
        public GameObject Whiteball;
        public GameObject Player;
        public PhotonView _view;

        public static float PressTime;
        void Start()
        {
           _view = GetComponent<PhotonView>();
        }
        void FixedUpdate()
        {
            PressTime = WhiteBallMovement.press_time;
            //Debug.Log(PressTime);
            if(PocketDyeNetworkManager.Instance.networked)
            {
                _view.RPC("CueSetting",RpcTarget.All);
            }
            else
            {
                CueSetting();
            }

        }
        [PunRPC]
        void CueSetting()
        {
            Cue.transform.LookAt(Whiteball.transform);
            Cue.transform.position = new Vector3((Whiteball.transform.position.x*(float)(1.2-0.4*PressTime) + Player.transform.position.x*(float)(0.8+0.4*PressTime))/2,Whiteball.transform.position.y+0.2f, (float)(Whiteball.transform.position.z*(float)(1.2-0.4*PressTime) + Player.transform.position.z*(0.8+0.4*PressTime))/2);
            
            //Debug.Log(BallMovement.CueBool);
            if(WhiteBallMovement.CueBool)
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
