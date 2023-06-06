using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] protected SoundManager.SoundNamesUI _soundNameMenuOpening;
    [SerializeField] protected SoundManager.SoundNamesUI _soundNameMenuClosing;
    protected IUIManagerMenu _IUIManagerMenu;
    protected ISoundManager _soundManager;

    public static event Action OnShow;
    public static event Action OnHide;
    public static bool IsShown;

    public static void Toggle()
    {
        IsShown = !IsShown;
        if (IsShown)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private static void Show() => OnShow?.Invoke();
    private static void Hide() => OnHide?.Invoke();

    protected virtual void OnEnable()
    {
        OnShow += ShowMenu;
        OnHide += HideMenu;
    }

    protected virtual void OnDisable()
    {
        OnShow -= ShowMenu;
        OnHide -= HideMenu;
    }

    public void Initialize(IUIManagerMenu iUIManagerMenu, ISoundManager soundManager)
    {
        _IUIManagerMenu = iUIManagerMenu;
        _soundManager = soundManager;
    }
    
    protected virtual void ShowMenu()
    {
        _IUIManagerMenu.ShowMenu();
        _soundManager.Play(_soundNameMenuOpening.ToString());
        _soundManager.TransitionTo(SoundManager.SnapshotName.Menu.ToString());
    }

    protected virtual void HideMenu()
    {
        _IUIManagerMenu.HideMenu();
        _soundManager.Play(_soundNameMenuClosing.ToString());
        _soundManager.TransitionTo(SoundManager.SnapshotName.Gameplay.ToString());
    }

}
