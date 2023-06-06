// You don't need PhysicsRaycaster on MainCamera (because use RectTransform)

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItems : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private bool _isSmoothDamp = true;
    [SerializeField] private float _dampingSpeed = 0.05f;
    private Vector3 _velocity;

    private Inventory _inventory;
    private InventorySlot _inventorySlot;
    private RectTransform _draggingObjectRectTransform;
    private Image _image;
    private CanvasGroup _canvasGroup;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
                
        _inventorySlot = GetComponent<InventorySlot>();
        _draggingObjectRectTransform = _inventory.DraggingIcon;
        _image = _draggingObjectRectTransform.gameObject.GetComponent<Image>();
        _canvasGroup = _draggingObjectRectTransform.gameObject.GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_inventorySlot.IsDragDrop == false)
            return;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObjectRectTransform,
            eventData.position, eventData.pressEventCamera, out var globalMousePosition))
        {
            if (_isSmoothDamp)
            {
                _draggingObjectRectTransform.position = Vector3.SmoothDamp(_draggingObjectRectTransform.position, globalMousePosition, ref _velocity, _dampingSpeed);
            }
            else
            {
                _draggingObjectRectTransform.position = globalMousePosition;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_inventorySlot.IsDragDrop == false)
            return;
        
        _inventorySlot.OnPointerClick(null);
        
        _canvasGroup.blocksRaycasts = false;
        _image.sprite = _inventorySlot.Icon.sprite;
        _inventorySlot.Icon.gameObject.SetActive(false);
        _draggingObjectRectTransform.position = transform.position;
        _draggingObjectRectTransform.gameObject.SetActive(true);

        _inventory.PlayBeginDragItemSound();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_inventorySlot.IsDragDrop == false)
            return;

        _canvasGroup.blocksRaycasts = true;
        _draggingObjectRectTransform.gameObject.SetActive(false);
        _inventorySlot.Icon.gameObject.SetActive(true);

        _inventory.PlayEndDragItemSound();
    }
}
