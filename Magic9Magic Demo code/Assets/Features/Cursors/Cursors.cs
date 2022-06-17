using System.Collections.Generic;
using UnityEngine;

public class Cursors : MonoBehaviour
{
    [SerializeField] private CursorSettingsSO _cursorSettingsSO;
    [SerializeField] private bool _isCursorLocked;
    private Dictionary<CursorNames, CursorsStruct> _cursors = new Dictionary<CursorNames, CursorsStruct>();

    public enum CursorNames
    {
        Default,
        DragDrop,
        Props
    }

    private void Awake()
    {
        FindAndTestComponents();
        LoadCursors();
        SetCursorState();
        SetDefaultCursor();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<Cursors>(gameObject);
        if (_cursorSettingsSO == null)
            Tools.LogError("CursorSettings _cursorSettingsSO = NULL");
    }

    private void LoadCursors()
    {
        foreach (var c in _cursorSettingsSO.Cursors)
            if (_cursors.ContainsKey(c.Name))
            {
                Tools.LogError(c.Name.ToString() + " cursor already exists in _cursors");
            }
            else
            {
                _cursors.Add(c.Name, c);
            }
    }

    private void SetDefaultCursor()
    {
        SetCursor(CursorNames.Default);
    }

    public void SetCursor(CursorNames name)
    {
        Cursor.SetCursor(_cursors[name].Texture, _cursors[name].HotSpot, _cursorSettingsSO.Mode);
    }

    private void SetCursorState()
	{
		Cursor.lockState = _isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
	}

}
