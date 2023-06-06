# LootManager

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

**Add LootManager in a new scene:**

1. Add LootManager prefab from Assets > Features > LootManager > Prefabs

2. Add spawnLootPosition prefabs in LootManager from Assets > Features > LootManager > Prefabs

3. Drag&Drop spawnLootPosition from Hierarchy to LootManager - SpawnLootPositionList

![LootManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/LootManager/lootManager01.jpg?raw=true)

![LootManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/LootManager/lootManager02.jpg?raw=true)

![LootManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/LootManager/lootManager03.jpg?raw=true)

public void Get(bool isSenderPosition, Vector3 senderPosition, string spawnLootPositionName, bool isSenderItemListsSOIndex, int itemListsSOIndex, int minItemAmount, int maxItemAmount, float minDelay, float maxDelay)


1. isSenderPosition: true & isSenderItemListsSOIndex: true (enemyStatsSO.Level) - for enemy loot

2. isSenderPosition: false & isSenderItemListsSOIndex: false (playerSettingsSO.Level) - for static position lootBox

3. isSenderPosition: true & isSenderItemListsSOIndex: false (playerSettingsSO.Level) - for dynamic position lootBox

_lootItemPosition = isSenderPosition ? senderPosition : _spawnLootPositionList[_indexSpawnPosition].transform.position;

_itemListsSOIndex = isSenderItemListsSOIndex ? itemListsSOIndex : _playerSettingsSOLevel - 1;

**DG)** GameDifficulty affects TotalDropProbability:

if (_gameSettingsSO.GameDifficulty == 1) currentTotalDropProbability -= _probabilityModifierOnMediumDifficulty;
    
if (_gameSettingsSO.GameDifficulty == 2) currentTotalDropProbability -= _probabilityModifierOnHighDifficulty;

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
