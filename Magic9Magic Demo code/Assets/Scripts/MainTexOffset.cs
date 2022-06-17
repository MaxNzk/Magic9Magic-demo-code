using UnityEngine;

public class MainTexOffset : MonoBehaviour
{
    [SerializeField] private float scrollSpeedX = 2.0f;
    private Renderer _renderer;
    private float _offsetX;
    
    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
            Tools.LogError("Renderer _renderer = NULL");
    }

    private void Update()
    {
        _offsetX += Time.deltaTime * scrollSpeedX;
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(_offsetX, 0));
        if (_offsetX > 1) _offsetX = 0;    
    }

}
