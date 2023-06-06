using System.Collections.Generic;
using UnityEngine;

public abstract class WorldCreator : MonoBehaviour
{
    [SerializeField] protected List<WorldDataSO> worldDataSOList = new List<WorldDataSO>();
    protected Dictionary<string, WorldDataSO> worldDataSODictionary = new Dictionary<string, WorldDataSO>();
    protected WorldDataSO _worldDataSO;
    protected int _gameDifficulty;

    public void CreateWorld(string worldSOName, int gameDifficulty)
    {
        ConvertWorldDataSOListToDic();
        if (worldDataSODictionary.ContainsKey(worldSOName))
        {
            Tools.Log("gameSettingsSO.WorldSOName = " + worldSOName);
            _worldDataSO = worldDataSODictionary[worldSOName];
        }
        else
        {
            Tools.Log("gameSettingsSO.WorldSOName is not in the worldDataSOList, so the default data is loaded = " + worldDataSOList[0]);
            _worldDataSO = worldDataSOList[0];
        }
        _gameDifficulty = gameDifficulty;
        Create();
    }

    protected abstract void Create();

    private void ConvertWorldDataSOListToDic()
    {
        for(int i = 0; i < worldDataSOList.Count; i++)
        {
            worldDataSODictionary.Add(worldDataSOList[i].name, worldDataSOList[i]);
        }
    }

}
