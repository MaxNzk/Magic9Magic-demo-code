using UnityEngine;
public interface ISaveProvider
{
    void Save(ScriptableObject currentSO);
    void Load(ScriptableObject currentSO);    
}
