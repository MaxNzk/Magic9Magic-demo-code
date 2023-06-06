using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    [SerializeField] private SaveManager _saveManager;

    private void Awake()
    {
        Tools.CheckSingleInstance<GameStateChanger>(gameObject);
        Tools.CheckNull<SaveManager>(_saveManager, nameof(_saveManager), gameObject);
    }
    
    #if UNITY_EDITOR
        public void FirstCutscene()
        {
            _saveManager.FirstCutscene();
            Debug.Log("GameStateChanger: FirstCutscene()");
        }

        public void FirstDeath()
        {
            _saveManager.FirstDeath();
            Debug.Log("GameStateChanger: FirstDeath()");
        }

        public void FirstItem()
        {
            _saveManager.FirstItem();
            Debug.Log("GameStateChanger: FirstItem()");
        }

        public void OpenAllPortals()
        {
            _saveManager.OpenAllPortals();
            Debug.Log("GameStateChanger: OpenAllPortals()");
        }
    #endif

}
