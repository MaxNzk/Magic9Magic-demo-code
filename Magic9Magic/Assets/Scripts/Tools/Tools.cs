using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Tools
{
    public static bool CheckSingleInstance<T>(GameObject sender) where T: MonoBehaviour
    {
        var objs = Object.FindObjectsOfType<T>();

        if (objs.Length > 1)
            LogError(sender.name + $": More than one {typeof(T)} on scene! Count = " + objs.Length.ToString());
        
        return objs.Length == 1;
    }

    public static void CheckNull<T>(T obj, string nameOfVar, GameObject sender) where T : class
    {      
        if (obj == null)
            LogError(sender.name + " : " + typeof(T).ToString() + " " + nameOfVar + " == NULL");
    }

    public static T GetComponentWithAssertion<T>(this GameObject from) where T : class
    {
        var component = from.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError($"There is no {typeof(T)} on {from.name}");
            SaveToFile($"There is no {typeof(T)} on {from.name}");
        }
        return component;
    }

    public static T GetComponentInChildrenWithAssertion<T>(this GameObject from) where T : class
    {
        var component = from.GetComponentInChildren<T>();
        if (component == null)
        {
            Debug.LogError($"There is no {typeof(T)} in {from.name}'s children");
            SaveToFile($"There is no {typeof(T)} in {from.name}'s children");
        }
        return component;
    }

    public static void LogError(string message)
    {
        Debug.LogError(message);
        SaveToFile(message);
    }

    public static void Log(string message)
    {
        Debug.Log(message);
        SaveToFile(message);
    }

    public static void LogArray<T>(string title, T[] arr)
    {
        Debug.LogError("----------------- " + title + " -----------------");
        for (int i = 0; i < arr.Length; i++)
            Debug.Log(i.ToString() + " = " + arr[i].ToString());
        Debug.LogError("--------------------- E N D ---------------------");
    }

    public static void LogDictionary<TKey, TValue>(string title, Dictionary<TKey, TValue> dictionary)
    {
        Debug.LogError("----------------- " + title + " -----------------");
        foreach (var kvp in dictionary)
            Tools.Log("Key = " + kvp.Key + " | Value = " + kvp.Value);
        Debug.LogError("--------------------- E N D ---------------------");
    }

    private static void SaveToFile(string message)
    {
        string _filePath = Application.persistentDataPath + "/ToolsDebug.txt";
        File.AppendAllText(_filePath, message + System.Environment.NewLine);
    }

    //---------------------------------------------------------------------------
    //---------------------------------------------------------------------------

    public static Vector3 GetRandomOnUnitCircle(float radius)
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(x, 0, z);
        pos = pos.normalized * radius;
        return pos;
    }

}
