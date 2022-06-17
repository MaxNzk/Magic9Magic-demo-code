using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _gameName;
    private SceneFade _sceneFade;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    { 
        _sceneFade = FindObjectOfType<SceneFade>();
        if (FindObjectsOfType<LoadingScreen>().Length > 1)
            Tools.LogError(gameObject.name + ": More than one LoadingScreen on scene!");
        if (_gameName == null)
            Tools.LogError("GameObject _gameName = NULL");
        if (_sceneFade == null)
            Tools.LogError("SceneFade _sceneFade = NULL");
    }

    public void StartingScene()
    {
        _gameName.SetActive(false);
        _sceneFade.FadeOut();
    }

    public void FinishingScene()
    {
        _gameName.SetActive(true);
        _sceneFade.FadeIn();
    }

    public void GoingToSettings()
    {
        _gameName.SetActive(false);
        _sceneFade.FastFadeIn();
    }

}
