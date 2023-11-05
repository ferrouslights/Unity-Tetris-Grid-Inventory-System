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

		[Header("Character Input Values")]
		public Vector2 cursorPosition;
		
		public bool interact;
		public bool use;

#if ENABLE_INPUT_SYSTEM 
		public void OnMousePosition(InputValue value)
		{
			MousePositionInput(value);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}
		
		public void OnUse(InputValue value)
		{
			UseInput(value.isPressed);
		}
#endif

		private void MousePositionInput(InputValue value)
		{
			cursorPosition = value.Get<Vector2>();
		}
		
		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}
		
		public void UseInput(bool newUseState)
		{
			use = newUseState;
		}
	}
	
}