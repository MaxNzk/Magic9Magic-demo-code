using System.Collections.Generic;
using UnityEngine;

public class Cursors : MonoBehaviour
{
    [SerializeField] private CursorSettingsSO _cursorSettingsSO;
    [SerializeField] private bool _isCursorLocked;

    [Space(10)]
    [SerializeField] private bool _isGlobalCursorMode;
    [SerializeField] private CursorMode _globalCursorMode = CursorMode.ForceSoftware;

    private Dictionary<CursorNames, CursorsStruct> _cursors = new Dictionary<CursorNames, CursorsStruct>();

    public enum CursorNames
    {
        Default = 0,
        DragDrop = 5,
        Props = 10
    }

    private void Awake()
    {
        FindAndTestComponents();
        LoadCursors();
        SetCursorState();
        SetDefaultCursor();
    }

    private void Start() {}

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
        if (_isGlobalCursorMode)
        {
            Cursor.SetCursor(_cursors[name].Texture, _cursors[name].HotSpot, _globalCursorMode);
        }
        else
        {
            Cursor.SetCursor(_cursors[name].Texture, _cursors[name].HotSpot, _cursorSettingsSO.Mode);
        }
    }

    public void SetCursorMode(CursorMode globalCursorMode, bool isGlobalCursorMode)
    {
        _globalCursorMode = globalCursorMode;
        _isGlobalCursorMode = isGlobalCursorMode;
    }

    private void SetCursorState()
	{
		Cursor.lockState = _isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
	}

}
