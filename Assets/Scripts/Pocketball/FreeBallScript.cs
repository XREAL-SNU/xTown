using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace JK
{
    public class FreeBallScript : MonoBehaviour
    {
        public static bool FreeBallBool;
        public static bool ColliderBool;
        public GameObject WhiteBall;
        Rigidbody rb;
        int i=0;
        void Start()
        {
            FreeBallBool = false;
            rb = WhiteBall.GetComponent<Rigidbody>();
            ColliderBool = false;
        }

        // Update is called once per frame
        void Update()
        {
            float moveX = 0;
            float moveZ = 0;
            if(FreeBallBool)
            {
                WhiteBall.SetActive(true);
                WhiteBall.GetComponent<SphereCollider>().isTrigger = true;
                
                rb.constraints = RigidbodyConstraints.FreezeAll;
                if(i==0)
                {
                    WhiteBall.GetComponent<Transform>().rotation = new Quaternion(0,0,0,0);
                    WhiteBall.transform.position = new Vector3(0, 0.357f,  0.96f);
                    i=1;
                }

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
                WhiteBall.transform.Translate(new Vector3(moveX, 0f, moveZ) * 0.1f);
                Debug.Log(GameManager.currentGameState);
                if(!ColliderBool)
                {
                    if(Input.GetMouseButtonDown(1))
                    {
                        
                        WhiteBall.GetComponent<SphereCollider>().isTrigger = false;
                        rb.constraints = RigidbodyConstraints.None;
                        rb.constraints = RigidbodyConstraints.FreezePositionY;
                        GameManager.isBallStop[0] = 1;
                        FreeBallBool = false;
                        GameManager.CamBool = true;
                        GameManager.currentGameState = GameManager.GameState.Rolling;
                    }
                }
                
            }
        }
    }
}