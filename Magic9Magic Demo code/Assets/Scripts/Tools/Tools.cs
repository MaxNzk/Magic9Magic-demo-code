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

    public static T GetComponentWithAssertion<T>(this GameObject from) where T : class
    {
        var component = from.GetComponent<T>();
        if (component.Equals(null))
        {
            #if UNITY_EDITOR
            Debug.LogError("There is no " + $"{typeof(T)} on {from.name}");
            #endif
            SaveToFile("There is no " + $"{typeof(T)} on {from.name}");
        }
        return component;
    }

    public static void LogError(string message)
    {
        #if UNITY_EDITOR
        Debug.LogError(message);
        #endif
        SaveToFile(message);
    }

    private static void SaveToFile(string message)
    {
        string _filePath = Application.persistentDataPath + "/ToolsDebugLog.txt";
        File.AppendAllText(_filePath, message + System.Environment.NewLine);
    }

    public static void Log(string message)
    {
        #if UNITY_EDITOR
        Debug.Log(message);
        #endif
        SaveToFile(message);
    }

    public static void LogArray<T>(string title, T[] arr)
    {
        #if UNITY_EDITOR
        Debug.LogError("----------------- " + title + " -----------------");
        for (int i = 0; i < arr.Length; i++)
            Debug.Log(i.ToString() + " = " + arr[i].ToString());
        Debug.LogError("--------------------- E N D ---------------------");
        #endif
    }

}
