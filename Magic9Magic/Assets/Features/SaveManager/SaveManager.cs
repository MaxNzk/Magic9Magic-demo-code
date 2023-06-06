using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class SaveManager : MonoBehaviour
{
    [SerializeField] private bool _saveBeforeNextScene;
    [SerializeField] private bool _saveBeforeAppQuit;
    [SerializeField] private List<ScriptableObject> _saveSOList = new List<ScriptableObject>();

    [Space(10)]
    [SerializeField] private List<ScriptableObject> _loadSOList = new List<ScriptableObject>();
    [Header("ONLY for Edit Mode. Set values in all SO. Check, Play and uncheck.")]
    [SerializeField] private bool _resaveLoadSOListToJson;

    [Space(10)]
    [Header("SOListSO. ONLY for Edit Mode.")]
    [SerializeField] private DefaultSOListSO _default;
    [SerializeField] private DefaultSOListSO _firstCutscene;
    [SerializeField] private DefaultSOListSO _firstDeath;
    [SerializeField] private DefaultSOListSO _firstItem;
    [SerializeField] private DefaultSOListSO _openAllPortals;

    private bool _isInitialized;
    private ISaveProvider[] _saveProviders;
    private bool _isSaved;

    public void Initialize()
    {
        if (_isInitialized)
            return;

        FindAndTestComponents();
        SetProviderTypes();       

        if (_resaveLoadSOListToJson || CheckFilesExist() == false)
            SaveLoadSOList();

        Load();

        _isInitialized = true;
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<SaveManager>(gameObject);
    }

    private void SetProviderTypes()
    {
        _saveProviders = GetComponents<ISaveProvider>();

        if (_saveProviders.Length == 0)
        {
            Tools.LogError("SaveManager : No providers");
        }

        for (int i = 0; i < _saveProviders.Length; i++)
        {
            _saveProviders[i].Initialize();
        }
    }

    private bool CheckFilesExist()
    {
        string filePath = Application.persistentDataPath + "/GameSettings.json";
        bool result = File.Exists(filePath);
        return result;
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
        {
            Save();
        }
    }

    #if UNITY_EDITOR
    public void ResetSOsByDefault(int index)
    {
        if (_default == null)
        {
            Debug.Log("_defaultSOListSO == null");
            return;
        }
        if (_default.CurrentAndDefaultSOList.Length == 0)
        {
            Debug.Log("_defaultSOListSO.CurrentAndDefaultSOList.Length == 0");
            return;
        }
        if (_default.CurrentAndDefaultSOList[index].CurrentSO == null)
        {
            Debug.Log("_defaultSOListSO.CurrentAndDefaultSOList[" + index + "].CurrentSO == null");
            return;
        }
        if (_default.CurrentAndDefaultSOList[index].DefaultSO == null)
        {
            Debug.Log("_defaultSOListSO.CurrentAndDefaultSOList[" + index + "].DefaultSO == null");
            return;
        }

        SetProviderTypes();
        ScriptableObject tmpSO = Instantiate(_default.CurrentAndDefaultSOList[index].DefaultSO) as ScriptableObject;
        tmpSO.name = _default.CurrentAndDefaultSOList[index].CurrentSO.name;

        for (int i = 0; i < _saveProviders.Length; i++)
        {
            _saveProviders[i].Save(tmpSO);
            _saveProviders[i].Load(_default.CurrentAndDefaultSOList[index].CurrentSO as ScriptableObject);
        }

        EditorUtility.SetDirty(tmpSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("ResetSOsByDefault()");
    }

    public void FirstCutscene() => ResetDefaultSOListSO(_firstCutscene);
    public void FirstDeath() => ResetDefaultSOListSO(_firstDeath);
    public void FirstItem() => ResetDefaultSOListSO(_firstItem);
    public void OpenAllPortals() => ResetDefaultSOListSO(_openAllPortals);

    private void ResetDefaultSOListSO(DefaultSOListSO list)
    {
        SetProviderTypes();

        for (int i = 0; i < list.CurrentAndDefaultSOList.Length; i++)
        {
            ScriptableObject tmpSO = Instantiate(list.CurrentAndDefaultSOList[i].DefaultSO) as ScriptableObject;
            tmpSO.name = list.CurrentAndDefaultSOList[i].CurrentSO.name;

            for (int k = 0; k < _saveProviders.Length; k++)
            {
                _saveProviders[k].Save(tmpSO);
                _saveProviders[k].Load(list.CurrentAndDefaultSOList[i].CurrentSO as ScriptableObject);
            }

            EditorUtility.SetDirty(tmpSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    #endif

    private void Load()
    {
        foreach (ScriptableObject so in _loadSOList)
        {
            for (int i = 0; i < _saveProviders.Length; i++)
            {
                _saveProviders[i].Load(so);
            }
        }
    }

    private void Save()
    {
        foreach (ScriptableObject so in _saveSOList)
        {
            for (int i = 0; i < _saveProviders.Length; i++)
            {
                _saveProviders[i].Save(so);
            }
        }
    }

    private void SaveLoadSOList()
    {
        foreach (ScriptableObject so in _loadSOList)
        {
            for (int i = 0; i < _saveProviders.Length; i++)
            {
                _saveProviders[i].Save(so);
            }
        }
            
        #if UNITY_EDITOR
        if (_resaveLoadSOListToJson)
            Debug.Break();
        #endif
    }

}
