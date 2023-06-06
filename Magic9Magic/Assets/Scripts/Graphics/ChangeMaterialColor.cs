using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    [SerializeField] private bool _isColor;
    [SerializeField] private Color _color;

    [SerializeField] private bool _isEmissionColor;
    [SerializeField] private Color _emissionColor;

    private void Awake()
    {
        Renderer renderer = Tools.GetComponentWithAssertion<Renderer>(gameObject);
        if (_isColor)
        {
            renderer.material.SetColor("_Color", _color);
        }
        if (_isEmissionColor)
        {
            renderer.material.SetColor("_EmissionColor", _emissionColor);
        }
    }

}
