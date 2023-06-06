using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private InventoryItemSO _itemSO;

    private Inventory _inventory;
    private SpriteRenderer _icon;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _icon = GetComponentInChildren<SpriteRenderer>();
        if (_icon == null)
            Tools.Log(gameObject.name + " InventoryItem.cs _icon == null");
        _icon.sprite = _itemSO.Icon;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = 0;
    }

    private void OnEnable()
    {
        _rigidbody.AddForce(Vector3.up);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inventory = other.gameObject.GetComponent<CharacterStats>().InventoryScript;
            if (_inventory == null)
            {
                Tools.Log(gameObject.name + " InventoryItem.cs _inventory == null");
            }

            _inventory.AddItem(_itemSO);
            _inventory.PlayPickupItemSound();
            
            Destroy(gameObject);
        }
    }

    public void SetSprite()
    {
        _icon = GetComponentInChildren<SpriteRenderer>();
        _icon.sprite = _itemSO.Icon;
    }

}
