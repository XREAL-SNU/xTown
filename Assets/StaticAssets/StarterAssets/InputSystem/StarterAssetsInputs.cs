using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool sit;
		public bool random;
		public int randomIndex;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnSit(InputValue value)
		{
			SitInput(value.isPressed);
		}

		public void OnF1(InputValue value)
        {
			FunctionInput(1);
		}

		public void OnF2(InputValue value)
		{
			FunctionInput(2);
		}

		public void OnF3(InputValue value)
		{
			FunctionInput(3);
		}

		public void OnF4(InputValue value)
		{
			FunctionInput(4);
		}

		public void OnF5(InputValue value)
		{
			FunctionInput(5);
		}

		public void OnF6(InputValue value)
		{
			FunctionInput(6);
		}

		public void OnF7(InputValue value)
		{
			FunctionInput(7);
		}

		public void OnF8(InputValue value)
		{
			FunctionInput(8);
		}

		public void OnF9(InputValue value)
		{
			FunctionInput(9);
		}

		public void OnF10(InputValue value)
		{
			FunctionInput(10);
		}

		public void OnF11(InputValue value)
		{
			FunctionInput(11);
		}

		public void OnF12(InputValue value)
		{
			FunctionInput(12);
		}

		public void FunctionInput(int value)
        {
			random = true;
			randomIndex = value - 1;
        }

		public void SitInput(bool newSitState)
        {
			sit = newSitState;
			Debug.Log("Sit command : " +sit);
        }

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}