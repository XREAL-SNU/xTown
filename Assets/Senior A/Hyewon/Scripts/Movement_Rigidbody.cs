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
        // �Է°� �޾ƿ���
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // �̵��ϰ��� �ϴ� ���� ���
        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        _charRigidbody.MovePosition(_charRigidbody.position + inputDir * moveSpeed * Time.deltaTime);

        // �̵��ϴ� �������� ȸ��
        transform.LookAt(transform.position + inputDir);
    }
}