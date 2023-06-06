using UnityEngine;

public class UIManagerWorld1 : UIManager, IUIManagerTimer, IUIManagerMenu, IUIManagerTutorialInput, IUIManagerMission
{
    [SerializeField] private UIShowHide _menu;
    [SerializeField] private UIShowHide _tutorialInput;
    [SerializeField] private UITimer _uiTimer;
    [SerializeField] private UIMissionMessages _uiMissionMessages;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<UIShowHide>(_menu, nameof(_menu), gameObject);
        Tools.CheckNull<UITimer>(_uiTimer, nameof(_uiTimer), gameObject);
        Tools.CheckNull<UIMissionMessages>(_uiMissionMessages, nameof(_uiMissionMessages), gameObject);
    }

    public void ShowMenu() => _menu.Show();
    public void HideMenu() => _menu.Hide();

    public void ShowTutorialInput() => _tutorialInput.Show();
    public void HideTutorialInput() => _tutorialInput.Hide();

    public void ShowTimer() => _uiTimer.Show();
    public void HideTimer() => _uiTimer.Hide();
    public void SetTimerText(int minutes, int seconds) => _uiTimer.SetText(minutes, seconds);

    public void ShowMissionAssignment(string msg) => _uiMissionMessages.ShowMissionAssignment(msg);
    public void HideMissionAssignment() => _uiMissionMessages.HideMissionAssignment();
    public void ShowMissionCompleted() => _uiMissionMessages.ShowMissionCompleted();
    public void ShowMissionFailed() => _uiMissionMessages.ShowMissionFailed();

}
