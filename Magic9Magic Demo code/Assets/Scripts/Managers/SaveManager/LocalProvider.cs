using UnityEngine;
using System.IO;

public class LocalProvider : ISaveProvider
{
    private string _json;
    private string _localDir;
    private string _filePath;

    public LocalProvider()
    {
        _localDir = Application.persistentDataPath;
    }

    public void Load(ScriptableObject currentSO)
    {
        _json = "";
        _filePath = _localDir + "/" + currentSO.name + ".json";
        using (var reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                _json += line;
            }
        }

        if (string.IsNullOrEmpty(_json))
            return; // ScriptableObject will set default values

        JsonUtility.FromJsonOverwrite(_json, currentSO);
    }

    public void Save(ScriptableObject currentSO)
    {
        _filePath = _localDir + "/" + currentSO.name + ".json";
        _json = JsonUtility.ToJson(currentSO, false);
        using (var writer = new StreamWriter(_filePath))
        {
            writer.WriteLine(_json);
        }
    }

}
