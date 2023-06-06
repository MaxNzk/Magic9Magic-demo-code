using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private float _waitNextSceneTime = 3.0f;
    [SerializeField] private float _waitSettingsSceneTime = 1.0f;
    private float _waitTeloportingTime;
    public string CurrentSceneName { get; private set; }

    public void Initialize()
    {
        CurrentSceneName = SceneManager.GetActiveScene().name;
        Tools.Log("--------------- " + CurrentSceneName + " -----------------");

        FindAndTestComponents();

        _loadingScreen.gameObject.SetActive(true);
        _loadingScreen.StartingScene();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<Scenes>(gameObject);

        Tools.CheckNull<LoadingScreen>(_loadingScreen, nameof(_loadingScreen), gameObject);
    }

    public void Load(string sceneName, float waitTeloportingTime)
    {
        _waitTeloportingTime = waitTeloportingTime;
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        float waitTime = 0;

        if (sceneName == "SettingsScene")
        {
            _loadingScreen.GoingToSettings();
            waitTime = _waitSettingsSceneTime;
        }

        if (sceneName == "CampScene")
        {
           _loadingScreen.FinishingScene();
           waitTime = _waitNextSceneTime;
        }

        if(sceneName == "World1Scene" || sceneName == "World1v2Scene" || sceneName == "World2Scene")
        {
            yield return new WaitForSeconds(_waitTeloportingTime);
            _loadingScreen.FinishingScene();
            waitTime = _waitNextSceneTime;
        }

        yield return new WaitForSeconds(waitTime);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
		yield return async;
    }
    
}
