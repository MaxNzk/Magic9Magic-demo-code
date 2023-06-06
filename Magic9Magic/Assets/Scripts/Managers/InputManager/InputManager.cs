using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool CanMove { get; private set; }
    public bool CanActivateActiveSlot { get; private set; }
    public Vector3 MousePosition { get; private set; }
    private bool[] IsSetActiveSlot = new bool[10];
    private int _activeSlotIndex = 0;

	[SerializeField] private int _filledSlotsNumber = 3;

    private IUIManagerInput _IUIManagerInput;
    private IUIManagerMenu _IUIManagerMenu;
    private bool _isActive;

    public void Initialize(IUIManagerInput iUIManagerInput, IUIManagerMenu iUIManagerMenu)
    {
        _IUIManagerInput = iUIManagerInput;
        _IUIManagerMenu = iUIManagerMenu;
        _filledSlotsNumber--;
    }

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

    private void ShowMenu() => Deactivate();
    private void HideMenu() => Activate();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.Toggle();
        }
        
        if (_isActive == false)
        {
            CanMove = false;
            return;
        }

        CanMove = Input.GetMouseButton(0);
        CanActivateActiveSlot = Input.GetMouseButtonDown(1);
        MousePosition = Input.mousePosition;

        for (int i = 0; i < 10; i++)
        {
            IsSetActiveSlot[i] = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && _filledSlotsNumber >= 0)
        {
            IsSetActiveSlot[0] = true;
            _activeSlotIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && _filledSlotsNumber >= 1)
        {
            IsSetActiveSlot[1] = true;
            _activeSlotIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && _filledSlotsNumber >= 2)
        {
            IsSetActiveSlot[2] = true;
            _activeSlotIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && _filledSlotsNumber >= 3)
        {
            IsSetActiveSlot[3] = true;
            _activeSlotIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && _filledSlotsNumber >= 4)
        {
            IsSetActiveSlot[4] = true;
            _activeSlotIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && _filledSlotsNumber >= 5)
        {
            IsSetActiveSlot[5] = true;
            _activeSlotIndex = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && _filledSlotsNumber >= 6)
        {
            IsSetActiveSlot[6] = true;
            _activeSlotIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && _filledSlotsNumber >= 7)
        {
            IsSetActiveSlot[7] = true;
            _activeSlotIndex = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && _filledSlotsNumber >= 8)
        {
            IsSetActiveSlot[8] = true;
            _activeSlotIndex = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) && _filledSlotsNumber >= 9)
        {
            IsSetActiveSlot[9] = true;
            _activeSlotIndex = 9;
        }

        if (Input.GetButtonDown("PreviousActiveSlot"))
        {
            IsSetActiveSlot[_activeSlotIndex] = false;
			_activeSlotIndex = _activeSlotIndex == 0 ? _filledSlotsNumber : _activeSlotIndex - 1;
			IsSetActiveSlot[_activeSlotIndex] = true;
        }
        if (Input.GetButtonDown("NextActiveSlot"))
        {
            IsSetActiveSlot[_activeSlotIndex] = false;
			_activeSlotIndex = _activeSlotIndex == _filledSlotsNumber ? 0 : _activeSlotIndex + 1;
			IsSetActiveSlot[_activeSlotIndex] = true;
        }

        // if (Input.GetButtonDown("MagicBook"))
            // _IUIManagerInput.ShowMagicBook();
        if (Input.GetButtonDown("Inventory"))
        {
            _IUIManagerInput.ShowInventory();
        }
        // if (Input.GetButtonDown("Journal"))
            // _IUIManagerInput.ShowJournal();
    }

    public bool GetIsActiveSlot(int index)
    {
        return IsSetActiveSlot[index];
    }

    public void Activate() => _isActive = true;
    public void Deactivate() => _isActive = false;

}
