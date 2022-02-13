using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
namespace JK
{
    public class FreeBallScript : MonoBehaviour
    {
        public static bool FreeBallBool;
        public static bool ColliderBool;
        public GameObject WhiteBall;
        public GameManager _gameManager;
        Rigidbody rb;
        int i=0;
        CameraScript _cameraScript;
        void Start()
        {
            FreeBallBool = false;
            _cameraScript = GetComponent<CameraScript>();
            rb = WhiteBall.GetComponent<Rigidbody>();
            _gameManager = GetComponent<GameManager>();
            ColliderBool = false;
        }
        void FixedUpdate()
        {
            float moveX = 0;
            float moveZ = 0;
            if(FreeBallBool && (!_gameManager._view.IsMine||!PocketDyeNetworkManager.Instance.networked))
            {
                if(!PocketDyeNetworkManager.Instance.networked)
                appearWhiteBall();
                else
                _gameManager._view.RPC("appearWhiteBall",RpcTarget.All);
                if(Input.GetKey(KeyCode.A))
                {
                    moveZ += 0.015f;
                }
                if(Input.GetKey(KeyCode.W))
                {
                    moveX += 0.015f;
                }
                if(Input.GetKey(KeyCode.D))
                {
                    moveZ -= 0.015f;
                }
                if(Input.GetKey(KeyCode.S))
                {
                    moveX -= 0.015f;
                }
                if(!PocketDyeNetworkManager.Instance.networked)
                    MoveWhiteBall(moveX,moveZ);
                else
                    _gameManager._view.RPC("MoveWhiteBall",RpcTarget.All,moveX,moveZ);
                
                //Debug.Log(GameManager.currentGameState);
                if(!ColliderBool)
                {
                    if(Input.GetMouseButtonDown(1))
                    {
                        if(!PocketDyeNetworkManager.Instance.networked)
                        ResumeGame();
                        else
                        _gameManager._view.RPC("ResumeGame",RpcTarget.All);
                    }
                }
                
            }
        }
        
        [PunRPC]
        void appearWhiteBall()
        {
            if(i==0)
            {
                WhiteBall.SetActive(true);
                WhiteBall.GetComponent<SphereCollider>().isTrigger = true;            
                rb.constraints = RigidbodyConstraints.FreezeAll;
                WhiteBall.GetComponent<Transform>().rotation = new Quaternion(0,0,0,0);
                WhiteBall.transform.position = new Vector3(0, 0.357f, 0.96f);
                i=1;
            }
        }
        [PunRPC]
        void ResumeGame()
        {
            WhiteBall.GetComponent<SphereCollider>().isTrigger = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            GameManager.isBallStop[0] = 1;
            FreeBallBool = false;
            _cameraScript.SetCameraCue();
            //GameManager.CamBool = true;
            GameManager.currentGameState = GameManager.GameState.Rolling;
            i=0;            
        }
        [PunRPC]
        void MoveWhiteBall(float x, float z)
        {
            WhiteBall.transform.Translate(new Vector3(x, 0f, z));
        }
    }
}