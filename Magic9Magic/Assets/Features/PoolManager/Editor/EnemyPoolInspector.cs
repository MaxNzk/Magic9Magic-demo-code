using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyPoolManager))]
public class EnemyPoolInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemyPoolManager targetScript = (EnemyPoolManager) target;
        if (GUILayout.Button("SetDefaultParents"))
            targetScript.SetDefaultParents();
    }
}
