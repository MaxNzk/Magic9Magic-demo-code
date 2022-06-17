// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
// Удалить как только изменения параметров от магии заработает.
// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

using UnityEngine;

public class PlayerModifiers : MonoBehaviour, IModifiable
{
    private PlayerMovement _playerMovementScript;
    private PlayerPoints _playerPointsScript;
    private PlayerTakeDamage _playerTakeDamageScript;
    private PlayerWeapons _playerWeaponsScript;
    private ManaMaker _manaMakerScript;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _playerMovementScript = GetComponent<PlayerMovement>();
        _playerPointsScript =  GetComponent<PlayerPoints>();
        _playerTakeDamageScript = GetComponent<PlayerTakeDamage>();
        _playerWeaponsScript = GetComponent<PlayerWeapons>();
        _manaMakerScript = GetComponent<ManaMaker>();
        if (_playerMovementScript == null)
            Tools.LogError("PlayerMovement _playerMovementScript = NULL");
        if (_playerPointsScript == null)
            Tools.LogError("PlayerPoints _playerPointsScript = NULL");
        if (_playerTakeDamageScript == null)
            Tools.LogError("PlayerTakeDamage _playerTakeDamageScript = NULL");
        if (_playerWeaponsScript == null)
            Tools.LogError("PlayerWeapons _playerWeaponsScript = NULL");
        if (_manaMakerScript == null)
            Tools.LogError("ManaMaker _manaMakerScript = NULL");
    }

    public void SetModifiableParams()
    {
        // name
    }

}
