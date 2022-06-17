using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableObjectPoolManager))]
public class PoolCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ScriptableObjectPoolManager targetScript = (ScriptableObjectPoolManager) target;
        if (GUILayout.Button("SetDefaultParents"))
            targetScript.SetDefaultParents();
    }
}
