using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DartScript : MonoBehaviour
{
    public Vector2 moveVal;
    public GameObject target;
    public GameObject Dart2;
    public GameObject Dart3;
    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        //초기에는 중력 X(방향 조절 해야하므로)
        rb.useGravity = false;
    }
    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void OnFire()
    {
        rb.AddForce(-this.transform.right*300f);
        //던지고 나서 중력 작용
        rb.useGravity = true;
        //던지고 난 후에는 더이상 방향 조절 or 발사 불가
        this.GetComponent<PlayerInput>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Balloon"))
        {
            col.gameObject.SetActive(false);

            //Dart_2, Dart_3 오브젝트를 disable 시켜놨다가, 하나씩 SetActive(true)하는 방식을 사용한다
            if(this.gameObject.name == "darts_3ds1")
            {
                Dart2.SetActive(true);
            }
            else if(this.gameObject.name == "darts_3ds2")
            {
                Dart3.SetActive(true);
            }

            //Dart disabled
            this.gameObject.SetActive(false);
        }
        else if(col.CompareTag("Obstacle"))
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX;

            if(this.gameObject.name == "darts_3ds1")
            {
                Dart2.SetActive(true);
            }
            else if(this.gameObject.name == "darts_3ds2")
            {
                Dart3.SetActive(true);
            }
        }

        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;

            if(this.gameObject.name == "darts_3ds1")
            {
                Dart2.SetActive(true);
            }
            else if(this.gameObject.name == "darts_3ds2")
            {
                Dart3.SetActive(true);
            }
        }
        
    }

    void Update()
    {
        //좌우 방향 움직임
        this.transform.RotateAround(target.transform.position, Vector3.up,(float)(moveVal.x*0.1));
        //상하 방향 움직임
        this.transform.RotateAround(target.transform.position, Vector3.left,(float)(moveVal.y*0.1));
    }
}
