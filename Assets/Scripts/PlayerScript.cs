using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class PlayerScript : MonoBehaviour
    {
        public GameObject whiteBall;
        public static Vector3 playerPosition = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            whiteBall = GameObject.Find("Ball_0");
        }

        // Update is called once per frame
        void Update()
        {
            playerPosition = transform.position;

            //카메라 이동
            //A(왼쪽) 눌렀을 때
            if (Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(whiteBall.transform.position, Vector3.up, 1f);
            }
            //D(오른쪽) 눌렀을 때
            if(Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(whiteBall.transform.position, Vector3.up, -1f);
            }
            
        }
    }
}