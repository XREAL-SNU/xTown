using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Rigidbody : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    private Rigidbody _charRigidbody;

    void Start()
    {
        Debug.Log("Start");
        _charRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 입력값 받아오기
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // 이동하고자 하는 방향 계산
        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        _charRigidbody.MovePosition(_charRigidbody.position + inputDir * moveSpeed * Time.deltaTime);

        // 이동하는 방향으로 회전
        transform.LookAt(transform.position + inputDir);
    }
}