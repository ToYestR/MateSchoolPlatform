using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = false;

		public void Update()
		{

			//yzl临时添加
			if (Input.GetKey(KeyCode.Mouse0))
			{
				cursorInputForLook = true;
			}
			else if (!Input.GetKey(KeyCode.Mouse0))
			{
				cursorInputForLook = false;
			}

		}
#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				//对输入值做处理
				if(Mathf.Abs(value.Get<Vector2>().x)>2)
                {
					cursorInputForLook = false;
					//return;
				}
				//Vector2 v2 = new Vector2(Mathf.Clamp(value.Get<Vector2>().x, -0.5f, 0.5f), value.Get<Vector2>().y);
				//Debug.Log(v2);
				//LookInput(v2)
				LookInput(value.Get<Vector2>());
				//cursorInputForLook = false;
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
#endif


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

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}