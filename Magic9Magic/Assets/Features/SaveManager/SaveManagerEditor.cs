#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SaveManager saveManager = (SaveManager) target;

        if (GUILayout.Button("Reset All SO By Default"))
        {
            saveManager.ResetSOsByDefault(0);
            saveManager.ResetSOsByDefault(1);
            saveManager.ResetSOsByDefault(2);
            saveManager.ResetSOsByDefault(3);
            saveManager.ResetSOsByDefault(4);
            saveManager.ResetSOsByDefault(5);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Reset AnalyticsStorage"))
            saveManager.ResetSOsByDefault(0);
        if (GUILayout.Button("Reset GameSettings"))
            saveManager.ResetSOsByDefault(1);
        if (GUILayout.Button("Reset PlayerSettings"))
            saveManager.ResetSOsByDefault(2);
        if (GUILayout.Button("Reset PlayerStatsListSO"))
            saveManager.ResetSOsByDefault(3);
        if (GUILayout.Button("Reset PlayerItemListSO"))
            saveManager.ResetSOsByDefault(4);
        if (GUILayout.Button("Reset MarketItemListSO"))
            saveManager.ResetSOsByDefault(5);
    }
}
#endif
