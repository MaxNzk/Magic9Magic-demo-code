using UnityEngine;

public interface ILootManager
{
    void Initialize(GameStateSettingsSO gameStateSettingsSO, PlayerSettings playerSettingsSO);
    public void Get(bool isSenderPosition, Vector3 senderPosition, string spawnLootPositionName, bool isSenderItemListsSOIndex, int itemListsSOIndex, int minItemAmount, int maxItemAmount, float minDelay, float maxDelay);
    
}