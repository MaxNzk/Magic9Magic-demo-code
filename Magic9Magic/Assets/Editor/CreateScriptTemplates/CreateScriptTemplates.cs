using UnityEditor;

public static class CreateScriptTemplates
{
    [MenuItem("Assets/Create/MonoBehaviour Script", priority = 40)]
    public static void CreateMonoBehaviourTemplates()
    {
        string templatePath = "Assets/Editor/CreateScriptTemplates/Templates/BehaviourTemplates.cs.txt";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScript.cs");
    }

    [MenuItem("Assets/Create/ShortMonoBehaviour Script", priority = 41)]
    public static void CreateShortMonoBehaviourTemplates()
    {
        string templatePath = "Assets/Editor/CreateScriptTemplates/Templates/ShortBehaviourTemplates.cs.txt";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScript.cs");
    }

    [MenuItem("Assets/Create/ScriptableObject Script", priority = 42)]
    public static void CreateScriptableObjectTemplates()
    {
        string templatePath = "Assets/Editor/CreateScriptTemplates/Templates/ScriptableObjectTemplates.cs.txt";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
    }
}
