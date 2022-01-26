using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class CueScript : MonoBehaviour
    {
        Vector3 cueDirection;
        Vector3 offsetPos = new Vector3(0.2f, -0.7f, -0.2f); //큐대 초기 위치 조정값
        public static Vector3 cuePosition = Vector3.zero;

        public GameObject whiteBall;
        public static Rigidbody rb;
        public static bool isCueMoving = false; //true 중에는 큐대 위치가 초기화되지 않도록 하는 변수

        // Start is called before the first frame update
        void Start()
        {
            whiteBall = GameObject.Find("Ball_0");
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            cuePosition = transform.position;

            if (GameManager.isBallStop.Sum() == 16 && isCueMoving == false) // 모든 공이 완전히 멈춘 경우 & 큐대 움직이지 않을 때
            {
                //큐대 위치 초기화
                transform.position = Camera.main.transform.position + offsetPos; //화면에 적절히 보이도록 배치
                //transform.rotation = Camera.main.transform.localRotation;
                
                transform.LookAt(whiteBall.transform); //흰 공을 바라보게 한다
                transform.rotation = Quaternion.LookRotation(transform.position - whiteBall.transform.position ); //큐대 방향이 반대여서 조정
                //transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            if (CueScript.isCueMoving == true) //큐대 이동
            {
                //transform.Translate(-BallMovement.ballDirection * Time.deltaTime * BallMovement.power);
                transform.Translate(-BallMovement.ballDirection * Time.deltaTime * BallMovement.power * 0.03f);
            }

        }
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject == whiteBall)
            {
                isCueMoving = false;
            }
        }


    }
}
