using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour, ILootManager
{
    [SerializeField, Min(0.5f)] private float _dropItemRadius;
    [SerializeField, Range(5, 20), Tooltip("Probability modifier on medium difficulty")] private int _probabilityModifierOnMediumDifficulty;
    [SerializeField, Range(5, 20), Tooltip("Probability modifier on high difficulty")] private int _probabilityModifierOnHighDifficulty;

    [Space(10)]
    [SerializeField] private List<SpawnLootPosition> _spawnLootPositionList = new List<SpawnLootPosition>();

    private GameStateSettingsSO _gameStateSettingsSO;
    private int _playerSettingsSOLevel;
    private float _minDelay;
    private float _maxDelay;

    private int _indexSpawnPosition;
    private int _itemListsSOIndex;
    private Vector3 _lootItemPosition;
    private int _itemAmount;
    private List<LootItem> _possibleItemList;

    public void Initialize(GameStateSettingsSO gameStateSettingsSO, PlayerSettings playerSettingsSO)
    {
        _gameStateSettingsSO = gameStateSettingsSO;
        _playerSettingsSOLevel = playerSettingsSO.Level;
    }

    /// <summary>
    /// isSenderPosition: true,  isSenderItemListsSOIndex: true  (enemyStatsSO.Level)     - for enemy loot <br/>
    /// isSenderPosition: false, isSenderItemListsSOIndex: false (playerSettingsSO.Level) - for static position lootBox <br/>
    /// isSenderPosition: true,  isSenderItemListsSOIndex: false (playerSettingsSO.Level) - for dynamic position lootBox
    /// </summary>
    public void Get(bool isSenderPosition, Vector3 senderPosition, string spawnLootPositionName, bool isSenderItemListsSOIndex, int itemListsSOIndex, int minItemAmount, int maxItemAmount, float minDelay, float maxDelay)
    {
        // Debug.Log("LootManager: Get() *********************************");

        // Convert spawnLootPositionName to indexSpawnPosition (index of _spawnLootPositionList) ------------------------------
        for (_indexSpawnPosition = 0; _indexSpawnPosition < _spawnLootPositionList.Count; _indexSpawnPosition++)
        {
            if (_spawnLootPositionList[_indexSpawnPosition].name == spawnLootPositionName)
            {
                // Debug.Log("Find: _spawnLootPositionList[" + _indexSpawnPosition.ToString() + "].name = " + spawnLootPositionName);
                break;
            }
        }
        if (_indexSpawnPosition == _spawnLootPositionList.Count)
        {
            Tools.LogError("LootManager: there is no type of spawnPositionName like " + spawnLootPositionName);
            return;
        }
        // -------------------------------------------------------------------------------------------------------------------
        _itemListsSOIndex = isSenderItemListsSOIndex ? itemListsSOIndex : _playerSettingsSOLevel - 1;
        if (_itemListsSOIndex >= _spawnLootPositionList[_indexSpawnPosition].ItemListsSO.Count)
        {
            _itemListsSOIndex = _spawnLootPositionList[_indexSpawnPosition].ItemListsSO.Count - 1;
            Tools.LogError("_itemListsSOIndex >= _spawnLootPositionList[_indexSpawnPosition].ItemListsSO.Count");
        }

        _lootItemPosition = isSenderPosition ? senderPosition : _spawnLootPositionList[_indexSpawnPosition].transform.position;
        _lootItemPosition += Tools.GetRandomOnUnitCircle(_dropItemRadius);

        _minDelay = minDelay;
        _maxDelay = maxDelay;
        // -------------------------------------------------------------------------------------------------------------------
        int currentTotalDropProbability = Random.Range(0, 101);
        
        if (_gameStateSettingsSO.GameDifficulty == 1)
            currentTotalDropProbability -= _probabilityModifierOnMediumDifficulty;
        if (_gameStateSettingsSO.GameDifficulty == 2)
            currentTotalDropProbability -= _probabilityModifierOnHighDifficulty;

        if (currentTotalDropProbability > _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].TotalDropProbability) return;
        // -------------------------------------------------------------------------------------------------------------------
        int itemListCount = _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList.Count;
        if (maxItemAmount > itemListCount)
        {
            maxItemAmount = itemListCount - 1;
            Tools.LogError("LootManager: maxItemAmount > itemListCount in " + _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].name);
        }

        _itemAmount = Random.Range(minItemAmount, maxItemAmount + 1);
        int randomNumber = Random.Range(1, 100);
        // Debug.Log("itemAmount = " + _itemAmount.ToString());
        // Debug.Log("randomNumber = " + randomNumber.ToString());
        // -------------------------------------------------------------------------------------------------------------------
        _possibleItemList = new List<LootItem>();
        int currentProbability = -1;
        bool isCurrentProbabilitySet = false;
        // Debug.Log("possibleItemList: ----------------------------------");
        for (int i = 0; i < itemListCount; i++)
        {
            if (randomNumber <= _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].Probability && 
                _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].Probability != 100)
            {
                if (isCurrentProbabilitySet == false)
                {
                    isCurrentProbabilitySet = true;
                    currentProbability = _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].Probability;
                }
                else
                {
                    if (currentProbability != _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].Probability && _possibleItemList.Count >= _itemAmount)
                        break;
                }
                _possibleItemList.Add(_spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i]);
                // Debug.Log("_spawnLootPositionList[" + _indexSpawnPosition.ToString() + "].ItemListsSO[" + _itemListsSOIndex.ToString() + "].ItemList[" + i.ToString() + "].ItemSO.name = " + _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].ItemSO.name);
            }
        }
        // Debug.Log("----------------------------------------------------");
        // -------------------------------------------------------------------------------------------------------------------
        _possibleItemList.Shuffle();
        InstantiateRequiredNumberOfItems();
        InstantiateItemsWithProbability100();
        // Debug.Log("****************************************************");
    }

    private void InstantiateRequiredNumberOfItems()
    {
        for (int i = 0; i < _possibleItemList.Count; i++)
        {
            if (i < _itemAmount)
            {
                // Debug.Log("Drop: " + _possibleItemList[i].ItemSO.name + " | " + _possibleItemList[i].Probability.ToString());
                GameObject lootItemPrefab = _possibleItemList[i].ItemSO.ItemPrefab;
                StartCoroutine(InstantiateWithDelay(lootItemPrefab, _lootItemPosition));
            }
            else
            {
                break;
            }
        }
    }

    private void InstantiateItemsWithProbability100()
    {
        int itemListCount = _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList.Count;
        for (int i = 0; i < itemListCount; i++)
        {
            if (_spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].Probability == 100)
            {
                // Debug.Log("Drop (100%): " + _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].ItemSO.name + " " + _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].Probability.ToString());
                GameObject lootItemPrefab = _spawnLootPositionList[_indexSpawnPosition].ItemListsSO[_itemListsSOIndex].ItemList[i].ItemSO.ItemPrefab;
                StartCoroutine(InstantiateWithDelay(lootItemPrefab, _lootItemPosition));
            }
        }
    }

    private IEnumerator InstantiateWithDelay(GameObject lootItemPrefab, Vector3 pos)
    {
        float delaySeconds = Random.Range(_minDelay, _maxDelay);
        yield return new WaitForSeconds(delaySeconds);
        Instantiate(lootItemPrefab, pos, Quaternion.identity);
    }
}
