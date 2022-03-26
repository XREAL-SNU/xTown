using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer_CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float runSpeed = 3.0f;
    public float jumpPower = 5.0f;
    public float rollSpeed = 3.0f;

    private bool _isRoll;
    private bool _isJump;
    private bool _isPunch;
    private Rigidbody _charRigidbody;
    private Animator _animator;

    void Start()
    {
        _charRigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

        // �Է°� �޾ƿ���
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        bool isRun;

        Roll();
        Jump();
        Punch();

        // �̵��ϰ��� �ϴ� ���� ���
        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        // �̵��ϴ� �������� ȸ��
        transform.LookAt(transform.position + inputDir);
        
        // �ٱ� Ȯ��
        isRun = Input.GetKey(KeyCode.LeftShift);


        if (!_isRoll)
        {
            if (!isRun)
            {
                _charRigidbody.MovePosition(_charRigidbody.position + inputDir * moveSpeed * Time.deltaTime);
            }
            else
            {
                _charRigidbody.MovePosition(_charRigidbody.position + inputDir * runSpeed * Time.deltaTime);
            }

            _animator.SetBool("isWalking", inputDir != Vector3.zero);
            _animator.SetBool("isRunning", isRun);
        }
        else
        {
            _charRigidbody.MovePosition(_charRigidbody.position + inputDir * rollSpeed * Time.deltaTime);
        }
    }

    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_isJump && !_isRoll && !_isPunch)
        {
            _isRoll = true;
            _animator.SetTrigger("Roll");

            Invoke("ResetTrigger", 1f);
        }
    }

    void Punch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isJump && !_isRoll && !_isPunch)
        {
            _isRoll = true;
            _animator.SetTrigger("Punch");

            Invoke("ResetTrigger", 0.3f);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJump && !_isRoll && !_isPunch)
        {
            _isJump = true;
            _animator.SetTrigger("Jump");
            _charRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            Invoke("ResetTrigger", 1f);
        }
    }

    void ResetTrigger()
    {
        _isJump = false;
        _isRoll = false;
        _isPunch = false;
    }
}