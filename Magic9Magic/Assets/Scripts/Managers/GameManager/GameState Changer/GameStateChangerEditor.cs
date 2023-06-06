#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameStateChanger))]
public class GameStateChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameStateChanger gameStateChanger = (GameStateChanger) target;

        GUILayout.Space(10);
        if (GUILayout.Button("First Cutscene"))
        {
            gameStateChanger.FirstCutscene();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("First Death"))
        {
            gameStateChanger.FirstDeath();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("First Item"))
        {
            gameStateChanger.FirstItem();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Open All Portals"))
        {
            gameStateChanger.OpenAllPortals();
        }
        GUILayout.Space(10);
    }
}
#endif