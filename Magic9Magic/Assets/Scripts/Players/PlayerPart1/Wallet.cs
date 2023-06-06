using UnityEngine;

public class Wallet : MonoBehaviour
{
    private PlayerSettings _playerSettingsSO;
    private IUIManagerWallet _IUIManagerWallet;

    public void Initialize(PlayerSettings playerSettingsSO, IUIManagerWallet iUIManagerWallet)
    {
        _playerSettingsSO = playerSettingsSO;
        _IUIManagerWallet = iUIManagerWallet;

        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_playerSettingsSO.Gold < 0)
            Tools.LogError("Wallet: playerSettingsSO.Gold < 0");
        if (_playerSettingsSO.SoulStone < 0)
            Tools.LogError("Wallet: playerSettingsSO.SoulStone < 0");
    }

    public bool HaveEnoughGold(int amount) => _playerSettingsSO.Gold >= amount;
    public bool HaveEnoughSoulStone(int amount) => _playerSettingsSO.SoulStone >= amount;

    public bool TryPayWithGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Wallet: TryPayWithGold(" + amount.ToString() + ")");
            return false;
        }

        if (_playerSettingsSO.Gold >= amount)
        {
            _playerSettingsSO.Gold -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryPayWithSoulStone(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Wallet: TryPayWithSoulStone(" + amount.ToString() + ")");
            return false;
        }
        
        if (_playerSettingsSO.SoulStone >= amount)
        {
            _playerSettingsSO.SoulStone -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GetMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Wallet: GetMoney(" + amount.ToString() + ")");
            return;
        }
        _playerSettingsSO.Gold += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _IUIManagerWallet.SetGold(_playerSettingsSO.Gold);
        _IUIManagerWallet.SetSoulStone(_playerSettingsSO.SoulStone);
    }

}
