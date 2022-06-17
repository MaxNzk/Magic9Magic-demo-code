using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
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
		public bool cursorInputForLook = true;
		//-----------------------------------------------
		// [Header("Our Settings")]
		public bool CanActivateActiveSlot { get; private set; }
		public bool CanActivateAltSlot { get; private set; }
		private bool[] IsSetActiveSlot = new bool[10];
		private UIManagerPlayer _uiManager;
		//-----------------------------------------------

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
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
			// SprintInput(value.isPressed);
		}
#endif

		//---------------------------------------------------------------------
		public void Initialize(UIManagerPlayer uiManager)
		{
			_uiManager = uiManager;
			SetCursorState(cursorLocked);
		}

		public bool GetIsActiveSlot(int index)
		{
			return IsSetActiveSlot[index];
		}

		// Canvas > UILayerPopup > Inventory > Button_Close
		public void SetCursorToFpsMode()
		{
			cursorLocked = true;
			SetCursorState(cursorLocked);
			cursorInputForLook = true;
		}

		public void OnInventory(InputValue value)
		{
			_uiManager.ShowInventory();
			cursorLocked = false;
			SetCursorState(cursorLocked);
			cursorInputForLook = false;
		}

		public void OnFire(InputValue value) => CanActivateActiveSlot = value.isPressed;
		public void OnAltFire(InputValue value) => CanActivateAltSlot = value.isPressed;

		public void OnSetActiveSlot0(InputValue value) => IsSetActiveSlot[0] = value.isPressed;
		public void OnSetActiveSlot1(InputValue value) => IsSetActiveSlot[1] = value.isPressed;
		public void OnSetActiveSlot2(InputValue value) => IsSetActiveSlot[2] = value.isPressed;
		public void OnSetActiveSlot3(InputValue value) => IsSetActiveSlot[3] = value.isPressed;
		public void OnSetActiveSlot4(InputValue value) => IsSetActiveSlot[4] = value.isPressed;
		public void OnSetActiveSlot5(InputValue value) => IsSetActiveSlot[5] = value.isPressed;
		public void OnSetActiveSlot6(InputValue value) => IsSetActiveSlot[6] = value.isPressed;
		public void OnSetActiveSlot7(InputValue value) => IsSetActiveSlot[7] = value.isPressed;
		public void OnSetActiveSlot8(InputValue value) => IsSetActiveSlot[8] = value.isPressed;
		public void OnSetActiveSlot9(InputValue value) => IsSetActiveSlot[9] = value.isPressed;
		
		
		//---------------------------------------------------------------------

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