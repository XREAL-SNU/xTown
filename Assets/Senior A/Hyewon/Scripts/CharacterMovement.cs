using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public float moveSpeed = 1.0f;
	public float runSpeed = 3.0f;
	public float jumpPower = 5.0f;

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
		float hAxis = Input.GetAxisRaw("Horizontal");
		float vAxis = Input.GetAxisRaw("Vertical");

		bool isRun = Input.GetKey(KeyCode.LeftShift);

		Jump();
		Punch();

		// 이동하고자 하는 방향 계산
		Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

		// 이동하는 방향으로 회전
		transform.LookAt(transform.position + inputDir);


		if (!_isPunch)
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
	}

	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !_isJump && !_isPunch)
		{
			_isJump = true;
			_animator.SetTrigger("Jump");
			_charRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

			Invoke("ResetTrigger", 1f);
		}
	}

	void Punch()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl) && !_isJump && !_isPunch)
		{
			_isPunch = true;
			_animator.SetTrigger("Punch");

			Invoke("ResetTrigger", 0.5f);
		}
	}

	void ResetTrigger()
	{
		_isJump = false;
		_isPunch = false;
	}
}
