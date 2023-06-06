using UnityEngine;

public class MenuWorldManager : MenuManager
{
    private IFinishMission _IFinishMission;

    public void Initialize(IFinishMission iFinishMission)
    {
        _IFinishMission = iFinishMission;
    }

    protected override void ShowMenu()
    {
        Time.timeScale = 0;
        base.ShowMenu();
    }

    protected override void HideMenu()
    {
        Time.timeScale = 1;
        base.HideMenu();
    }

    // Canvas > UILayerPopup > Menu > Button_ResumeGame
    public void ResumeGame()
    {
        Toggle();
    }

    // Canvas > UILayerPopup > Menu > Button_BackToCamp
    public void MenuBackToCamp()
    {
        HideMenu();
        _IFinishMission.FinishMission(Missions.Status.BackToCamp);
    }
}
