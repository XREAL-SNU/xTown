using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace JK
{
    public class DrawTrajectory : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        public Vector3 ballDirection;
        public GameObject WhiteBall;
        public GameObject WhiteCam;
        PhotonView _view;
        
        // Use this for initialization
        void Start ()
        {
            //라인렌더러 설정
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetColors(Color.red, Color.yellow);
            lineRenderer.SetWidth(0.03f, 0.03f);
            //라인렌더러 처음위치 나중위치
            lineRenderer.SetPosition(0, transform.position);
            _view = GetComponent<PhotonView>();
        }
        
        // Update is called once per frame
        void FixedUpdate ()
        {
            if(PocketDyeNetworkManager.Instance.networked)
            {
                _view.RPC("RenderTrajectory",RpcTarget.All);
            }
            else
            {
                RenderTrajectory();
            }
 
        }
        [PunRPC]
        void RenderTrajectory()
        {
            ballDirection = WhiteBall.transform.position - WhiteCam.transform.position;
            ballDirection.y = 0; 
            lineRenderer.SetPosition(0, WhiteBall.transform.position);
            lineRenderer.SetPosition(1, WhiteBall.transform.position+ ballDirection*2); 
        }
    }
}
