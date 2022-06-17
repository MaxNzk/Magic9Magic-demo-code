using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private enum SaveProviderType { Local, Cloud }
    [SerializeField] private SaveProviderType _saveProviderType = SaveProviderType.Local;
    [SerializeField] private bool _loadOnInitialize;
    [SerializeField] private bool _saveBeforeNextScene;
    [SerializeField] private bool _saveBeforeAppQuit;
    [SerializeField] private List<ScriptableObject> _saveSOList = new List<ScriptableObject>();
    [SerializeField] private List<ScriptableObject> _loadSOList = new List<ScriptableObject>();
    private ISaveProvider _saveProvider;
    private bool _isSaved;
    private bool _isInitialized;

    public void Initialize(bool isFirstStartEver)
    {
        if (_isInitialized) return;

        FindAndTestComponents();

        switch (_saveProviderType)
        {
            case SaveProviderType.Local:
                _saveProvider = new LocalProvider();
                break;
            case SaveProviderType.Cloud:
                _saveProvider = new CloudProvider();
                break;
            default:
                Tools.LogError("_saveProviderType is not valid value.");
                break;
        }

        if (isFirstStartEver || CheckFilesExist() == false)
            SaveAllOnFirstGameLoad();
        if (_loadOnInitialize)
            Load();
        _isInitialized = true;
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<SaveManager>(gameObject);
    }

    private bool CheckFilesExist()
    {
        string filePath = Application.persistentDataPath + "/AnalyticsStorage.json";
        return File.Exists(filePath);   
    }

    private void OnApplicationQuit()
    {
        if (_saveBeforeAppQuit)
        {
            _isSaved = true;
            Save();
        }
    }

    private void OnDisable()
    {
        if (_saveBeforeNextScene && _isSaved == false)
            Save();
    }

    private void Load()
    {
        foreach (ScriptableObject so in _loadSOList)
            _saveProvider.Load(so);
    }

    private void Save()
    {
        foreach (ScriptableObject so in _saveSOList)
            _saveProvider.Save(so);
    }

    private void SaveAllOnFirstGameLoad()
    {
        foreach (ScriptableObject so in _loadSOList)
            _saveProvider.Save(so);  
    }

}
