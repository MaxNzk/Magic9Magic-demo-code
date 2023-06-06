using UnityEngine;

public class MenuCampManager : MenuManager
{
    private Scenes _scenes;

    public void Initialize(Scenes scenes)
    {
        _scenes = scenes;
    }

    // Canvas > UILayerPopup > Menu > Button_ResumeGame
    public void ResumeGame() => Toggle();

    // Canvas > UILayerHUD > SettingsButton
    // Canvas > UILayerPopup > MenuCamp > Button_Settings
    public void LoadSettingsScene()
    {
        _scenes.Load("SettingsScene", waitTeloportingTime: 0);
    }

    // Canvas > UILayerPopup > MenuCamp > Button_Exit
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
