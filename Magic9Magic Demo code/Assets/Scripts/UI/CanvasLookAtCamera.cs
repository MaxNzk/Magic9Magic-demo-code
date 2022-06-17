using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    
    private void Awake()
    {
        FindAndTestComponents();        
    }

    private void FindAndTestComponents()
    {
        _cameraTransform = Camera.main.GetComponent<Transform>();
        if (_cameraTransform == null)
            Tools.LogError("Transform _cameraTransform = NULL");
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
