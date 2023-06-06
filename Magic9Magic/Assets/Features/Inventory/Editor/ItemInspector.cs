#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryItem))]
public class ItemInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InventoryItem targetScript = (InventoryItem) target;
        if (GUILayout.Button("Set sprite"))
            targetScript.SetSprite();
    }
}
#endif