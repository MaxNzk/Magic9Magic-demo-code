using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PopupTextPool))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PopupTextPool targetScript = (PopupTextPool) target;
        if (GUILayout.Button("SetDefaultParents"))
            targetScript.SetDefaultParents();
    }
}
