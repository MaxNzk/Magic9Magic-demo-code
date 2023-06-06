using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _highlightedColor;
    private TextMeshProUGUI _text;

    private void Awake() => _text = GetComponentInChildren<TextMeshProUGUI>();
    public void OnPointerExit(PointerEventData eventData) => _text.color = _normalColor;
    public void OnPointerEnter(PointerEventData eventData) => _text.color = _highlightedColor;

}
