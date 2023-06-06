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
		[Header("My Settings")]
		[SerializeField] private bool _canJump = true;
		[SerializeField] private bool _canSprint = false;
		public bool CanActivateActiveSlot { get; private set; }
		public bool CanActivateAltSlot { get; private set; }
		private bool[] IsSetActiveSlot = new bool[10];
		private int _activeSlotIndex = 0;

		[Space(10)]
		[SerializeField] private int _filledSlotsNumber = 3;
		private IUIManagerInput _IUIManagerInput;
		private bool _isActive;
		//-----------------------------------------------

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if (_isActive)
				MoveInput(value.Get<Vector2>());
			else
				MoveInput(Vector2.zero);
		}

		public void OnLook(InputValue value)
		{
			if(_isActive && cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (_isActive && _canJump)
				JumpInput(value.isPressed);
			else
				JumpInput(false);
		}

		public void OnSprint(InputValue value)
		{
			if (_isActive && _canSprint)
				SprintInput(value.isPressed);
			else
				SprintInput(false);
		}
#endif

		//---------------------------------------------------------------------

		private void OnEnable()
		{
			MenuManager.OnShow += ShowMenu;
			MenuManager.OnHide += HideMenu;
		}

		private void OnDisable()
		{
			MenuManager.OnShow -= ShowMenu;
			MenuManager.OnHide -= HideMenu;
		}

		private void ShowMenu()
		{
			Deactivate();
			SetAfterMenuChange();
		}
		private void HideMenu()
		{
			Activate();
			SetAfterMenuChange();
		}

		public void Initialize(IUIManagerInput iUIManagerInput)
		{
			_IUIManagerInput = iUIManagerInput;
			SetCursorState(cursorLocked);
			_filledSlotsNumber--;
		}

		public void Activate() => _isActive = true;
    	public void Deactivate() => _isActive = false;

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

		public void OnMenu(InputValue value)
		{
			MenuManager.Toggle();
		}

		public void SetAfterMenuChange()
		{
			cursorLocked = !cursorLocked;
			SetCursorState(cursorLocked);
			cursorInputForLook = !cursorInputForLook;
			LookInput(Vector2.zero);
		}

		public void OnInventory(InputValue value)
		{
			if (_isActive == false)
				return;

			LookInput(Vector2.zero);
			_IUIManagerInput.ShowInventory();
			cursorLocked = false;
			SetCursorState(cursorLocked);
			cursorInputForLook = false;
		}

		public void OnInventoryClose()
		{
			cursorLocked = !cursorLocked;
			SetCursorState(cursorLocked);
			cursorInputForLook = !cursorInputForLook;
			LookInput(Vector2.zero);

		}

		public void OnFire(InputValue value)
		{
			if (_isActive == false)
				return;

			CanActivateActiveSlot = value.isPressed;
		}

		public void OnAltFire(InputValue value)
		{
			if (_isActive == false)
				return;

			CanActivateAltSlot = value.isPressed;
		}

		private void UnsetActiveSlots()
		{
			for (int i = 0; i < 10; i++)
				IsSetActiveSlot[i] = false;
		}

		public void OnSetActiveSlot0(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 0)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[0] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 1;
		}
		public void OnSetActiveSlot1(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 1)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[1] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 2;
		}
		public void OnSetActiveSlot2(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 2)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[2] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 3;
		}
		public void OnSetActiveSlot3(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 3)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[3] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 4;
		}
		public void OnSetActiveSlot4(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 4)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[4] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 5;
		}
		public void OnSetActiveSlot5(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 5)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[5] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 6;
		}
		public void OnSetActiveSlot6(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 6)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[6] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 7;
		}
		public void OnSetActiveSlot7(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 7)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[7] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 8;
		}
		public void OnSetActiveSlot8(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 8)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[8] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 9;
		}
		public void OnSetActiveSlot9(InputValue value)
		{
			if (_isActive == false || _filledSlotsNumber < 9)
				return;

			UnsetActiveSlots();
			IsSetActiveSlot[9] = value.isPressed;
			if (value.isPressed) _activeSlotIndex = 0;
		}

		public void OnSetNextActiveSlot(InputValue value)
		{
			if (_isActive == false)
				return;

			if (value.isPressed)
			{
				IsSetActiveSlot[_activeSlotIndex] = false;
				_activeSlotIndex = _activeSlotIndex == _filledSlotsNumber ? 0 : _activeSlotIndex + 1;
				IsSetActiveSlot[_activeSlotIndex] = true;
			}
		}
		public void OnSetPreviousActiveSlot(InputValue value)
		{
			if (_isActive == false)
				return;
				
			if (value.isPressed)
			{
				IsSetActiveSlot[_activeSlotIndex] = false;
				_activeSlotIndex = _activeSlotIndex == 0 ? _filledSlotsNumber : _activeSlotIndex - 1;
				IsSetActiveSlot[_activeSlotIndex] = true;
			}
		}
		
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