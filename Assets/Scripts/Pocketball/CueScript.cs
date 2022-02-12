using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace JK
{
    public class CueScript : MonoBehaviour
    {
        public GameObject Cue;
        public GameObject Whiteball;
        public GameObject Player;
        public PhotonView _view;

        void Start()
        {
           _view = GetComponent<PhotonView>();
        }
        void FixedUpdate()
        {
            //Debug.Log(PressTime);
            if(PocketDyeNetworkManager.Instance.networked && _view.IsMine)
            {
                _view.RPC("CueSetting",RpcTarget.All,WhiteBallMovement.press_time,WhiteBallMovement.CueBool);
            }
            else if(!PocketDyeNetworkManager.Instance.networked)
            {
                CueSetting(WhiteBallMovement.press_time,WhiteBallMovement.CueBool);
            }

        }
        [PunRPC]
        void CueSetting(float pressTime,bool cueBool)
        {
            Cue.transform.LookAt(Whiteball.transform);
            Cue.transform.position = new Vector3((Whiteball.transform.position.x*(float)(1.2-0.4*pressTime) + Player.transform.position.x*(float)(0.8+0.4*pressTime))/2,Whiteball.transform.position.y+0.2f, (float)(Whiteball.transform.position.z*(float)(1.2-0.4*pressTime) + Player.transform.position.z*(0.8+0.4*pressTime))/2);
            
            //Debug.Log(BallMovement.CueBool);
            if(cueBool)
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
