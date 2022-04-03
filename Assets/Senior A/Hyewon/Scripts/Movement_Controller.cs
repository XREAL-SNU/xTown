using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
	public float moveSpeed = 1.0f;

	private CharacterController _characterController;
	private Vector3 currentMovement;

	void Start()
	{
		_characterController = GetComponent<CharacterController>();
	}

	void Update()
	{
		float hAxis = Input.GetAxisRaw("Horizontal");
		float vAxis = Input.GetAxisRaw("Vertical");

		// 이동할 벡터 : 이동하고자 하는 방향 * 이동 속도 * Time.deltaTime
		currentMovement = new Vector3(hAxis, 0, vAxis) * moveSpeed * Time.deltaTime;

		_characterController.Move(currentMovement);
		transform.LookAt(transform.position + currentMovement);
	}
}
