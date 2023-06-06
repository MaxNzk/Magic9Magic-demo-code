using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MagicPoolManager))]
public class MagicPoolInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MagicPoolManager targetScript = (MagicPoolManager) target;
        if (GUILayout.Button("SetDefaultParents"))
            targetScript.SetDefaultParents();
    }
}
