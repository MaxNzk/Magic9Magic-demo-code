using UnityEngine;

public interface ISaveProvider
{
    void Initialize();
    void Save(ScriptableObject currentSO);
    void Load(ScriptableObject currentSO);    
}
