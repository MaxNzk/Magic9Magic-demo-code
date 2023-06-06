using UnityEngine;

[CreateAssetMenu(fileName = "CursorSettings", menuName = "Scriptable Objects/CursorSettings")]
public class CursorSettingsSO : ScriptableObject
{
    [field: SerializeField] public CursorMode Mode { get; private set; }
    [field: SerializeField] public CursorsStruct[] Cursors { get; private set; }
}

[System.Serializable]
    public struct CursorsStruct
    {
        public Cursors.CursorNames Name;
        public Texture2D Texture;
        public Vector2 HotSpot;
    }
