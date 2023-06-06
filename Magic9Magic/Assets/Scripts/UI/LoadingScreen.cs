using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _gameName;
    [SerializeField] private SceneFade _sceneFade;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<LoadingScreen>(gameObject);
        
        Tools.CheckNull<GameObject>(_gameName, nameof(_gameName), gameObject);
        Tools.CheckNull<SceneFade>(_sceneFade, nameof(_sceneFade), gameObject);
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
