using UnityEngine;

public class MarketTrigger : MonoBehaviour
{
    private IUIManagerMarket _uiManagerMarket;
    private IUIManagerInventory _IUIManagerInventory;

    public void Initialize(IUIManagerMarket uiManagerMarket, IUIManagerInventory iUIManagerInventory)
    {
        _uiManagerMarket = uiManagerMarket;
        _IUIManagerInventory = iUIManagerInventory;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManagerMarket.ShowMarket();
            _IUIManagerInventory.ShowInventory();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManagerMarket.HideMarket();
            _IUIManagerInventory.HideInventory();
        }
    }

}
