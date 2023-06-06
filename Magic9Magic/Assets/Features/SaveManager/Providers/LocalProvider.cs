using UnityEngine;
using System.IO;

public class LocalProvider : MonoBehaviour, ISaveProvider
{
    private string _jsonData;
    private string _localDir;
    private string _filePath;

    private void Start() {}

    public void Initialize()
    {
        _localDir = Application.persistentDataPath;
    }

    public void Load(ScriptableObject currentSO)
    {
        _jsonData = "";
        _filePath = _localDir + "/" + currentSO.name + ".json";

        using (var reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                _jsonData += line;
            }
        }

        if (string.IsNullOrEmpty(_jsonData))
            return; // ScriptableObject will set default values

        JsonUtility.FromJsonOverwrite(_jsonData, currentSO);
    }

    public void Save(ScriptableObject currentSO)
    {
        _filePath = _localDir + "/" + currentSO.name + ".json";
        _jsonData = JsonUtility.ToJson(currentSO, false);
        
        using (var writer = new StreamWriter(_filePath))
        {
            writer.WriteLine(_jsonData);
        }
    }
}
