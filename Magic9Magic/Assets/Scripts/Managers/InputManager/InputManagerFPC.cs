using UnityEngine;

public class InputManagerFPC : MonoBehaviour
{
    public bool CanMove { get; private set; }
    public bool CanActivateActiveSlot { get; private set; }
    public Vector3 MousePosition { get; private set; }
    private bool[] IsSetActiveSlot = new bool[10];
    private UIManagerPlayer _uiManager;

    public void Initialize(UIManagerPlayer uiManager)
    {
        _uiManager = uiManager;
    }

    private void Update()
    {
        CanMove = Input.GetMouseButton(0);
        CanActivateActiveSlot = Input.GetMouseButtonDown(1);
        MousePosition = Input.mousePosition;

        IsSetActiveSlot[0] = Input.GetKeyDown(KeyCode.Alpha1);
        IsSetActiveSlot[1] = Input.GetKeyDown(KeyCode.Alpha2);
        IsSetActiveSlot[2] = Input.GetKeyDown(KeyCode.Alpha3);
        IsSetActiveSlot[3] = Input.GetKeyDown(KeyCode.Alpha4);
        IsSetActiveSlot[4] = Input.GetKeyDown(KeyCode.Alpha5);
        IsSetActiveSlot[5] = Input.GetKeyDown(KeyCode.Alpha6);
        IsSetActiveSlot[6] = Input.GetKeyDown(KeyCode.Alpha7);
        IsSetActiveSlot[7] = Input.GetKeyDown(KeyCode.Alpha8);
        IsSetActiveSlot[8] = Input.GetKeyDown(KeyCode.Alpha9);
        IsSetActiveSlot[9] = Input.GetKeyDown(KeyCode.Alpha0);

        // if (Input.GetButtonDown("MagicBook"))
            // _uiManager.ShowMagicBook();
        if (Input.GetButtonDown("Inventory"))
        {
            _uiManager.ShowInventory();
        }
        // if (Input.GetButtonDown("Journal"))
            // _uiManager.ShowJournal();
    }

    public bool GetIsActiveSlot(int index)
    {
        return IsSetActiveSlot[index];
    }

}
