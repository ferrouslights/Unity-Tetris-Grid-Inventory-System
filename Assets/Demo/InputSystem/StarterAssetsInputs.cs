using ProjectFiles.TetrisGrid;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	
	public class StarterAssetsInputs : MonoBehaviour, IInteractInput, ICursorInput, IUseInput
	{
		public bool Interact
		{
			get => interact;
			set => InteractInput(value);
		}
		
		public bool Use
		{
			get => use;
			set => UseInput(value);
		}

		public Vector2 CursorPosition => cursorPosition;

		public bool CursorLocked
		{
			get => cursorLocked;
			set => SetCursorState(value);
		}
		
		public bool CursorInputForLook
		{
			get => cursorInputForLook;
			set => cursorInputForLook = value;
		}
		
		
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public Vector2 cursorPosition;
		
		public bool jump;
		public bool interact;
		public bool use;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM 
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}
		
		public void OnMousePosition(InputValue value)
		{
			MousePositionInput(value);
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
		
		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}
		
		public void OnUse(InputValue value)
		{
			UseInput(value.isPressed);
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
		
		private void MousePositionInput(InputValue value)
		{
			cursorPosition = value.Get<Vector2>();
		}
		
		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}
		
		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}
		
		public void UseInput(bool newUseState)
		{
			use = newUseState;
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