using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace JK
{
    public class BallMovement : MonoBehaviour
    {
        Rigidbody rb;
        public Vector3 ballVelocity;
        public PhotonView _view;
        int BallNum;
        //PhotonView _view;
        //PhotonTransformView _transformView;
        //private bool trigger = true;

        // Start is called before the first frame update
        void Start()
        {
            rb=GetComponent<Rigidbody>();
            //_view = GetComponent<PhotonView>();
            //_transformView = GetComponent<PhotonTransformView>();
            BallNum=int.Parse(gameObject.name.Substring(5));
            _view = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ballVelocity=rb.velocity;
            if(Mathf.Abs(ballVelocity.x) <= 0.0005 && Mathf.Abs(ballVelocity.z) <= 0.0005) // 공이 멈췄을 때
            {
                //Debug.Log("hi"+BallNum.ToString());
                GameManager.isBallStop[BallNum]=1;
            }
            else
            {
                GameManager.isBallStop[BallNum]=0;
            }
        }
    }    
}