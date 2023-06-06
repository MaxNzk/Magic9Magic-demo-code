#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sound))]
public class SoundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Sound sound = (Sound) target;
        if (GUILayout.Button("Play Sound"))
            sound.PlayInEditor();
    }
}
#endif