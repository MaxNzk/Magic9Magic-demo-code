using UnityEngine;

public class UIManagerWorld1 : MonoBehaviour, ITimer
{
    [SerializeField] private UITimer _uiTimer;
    [SerializeField] private UIMessages _uiMessages;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_uiTimer == null)
            Tools.LogError("UITimer _uiTimer = NULL");
        if (_uiMessages == null)
            Tools.LogError("UIMessages _uiMessages = NULL");
    }

    public void ShowTimer() => _uiTimer.Show();
    public void HideTimer() => _uiTimer.Hide();
    public void SetTimerText(int minutes, int seconds) => _uiTimer.SetText(minutes, seconds);

    public void ShowSurviveXminutes() => _uiMessages.ShowSurviveXminutes();
    public void ShowMissionCompleted() => _uiMessages.ShowMissionCompleted();
    public void ShowMissionFailed() => _uiMessages.ShowMissionFailed();

}
