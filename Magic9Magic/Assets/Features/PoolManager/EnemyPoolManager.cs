using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private EnemyLevelListSO[] _enemyLevelListSO;
    private int _difficultyLevel;
    private Transform _player;
    private ILootManager _lootManager;
    private ISoundManager _soundManager;
    private IAddressableManager _addressableManager;

    private Dictionary<string, int> _poolName = new Dictionary<string, int>(); // <_enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].EnemyNames, index of .EnemyLevelList>
    [SerializeField] private Transform _parentForObjectsInPool;
    [SerializeField] private Transform _parentForActiveObjects;

    public void Initialize(int difficultyLevel, Transform player, ILootManager lootManager, ISoundManager soundManager, IAddressableManager addressableManager)
    {
        // Debug.Log("difficultyLevel = " + difficultyLevel);
        _difficultyLevel = difficultyLevel;
        _player = player;
        _lootManager = lootManager;
        _soundManager = soundManager;
        _addressableManager = addressableManager;

        for (int i = 0; i < _enemyLevelListSO[_difficultyLevel].EnemyLevelList.Length; i++)
		{
            _poolName.Add(_enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].NameSO.ReferenceName, i);
            // Debug.Log("_poolName = " + _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].NameSO.ReferenceName);

            _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].PooledObjectQueue = new Queue<GameObject>();
            _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].CurrentObjectAmount = _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].ObjectAmount;

            GameObject newEnemyPrefab = _addressableManager.Get(_enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].NameSO.ReferenceName);
            
			for (int j = 0; j < _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].ObjectAmount; j++)
			{
				GameObject newObject = Instantiate(newEnemyPrefab);
                newObject.name = _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].NameSO.ReferenceName;
                newObject.transform.SetParent(_parentForObjectsInPool);
                _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].PooledObjectQueue.Enqueue(newObject);
                PlayerPoints playerPointsScript = _player.gameObject.GetComponent<PlayerPoints>();
                newObject.GetComponent<EnemyTakeDamage>().Initialize(isReturnToPool: true, this, _lootManager, _soundManager, playerPointsScript);
                
                EnemyMoveToTarget moveToTargetScript = newObject.GetComponent<EnemyMoveToTarget>();
                if (moveToTargetScript != null)
                    moveToTargetScript.Initialize(_player);

                newObject.GetComponent<EnemyMovement>().Initialize(_player);
                newObject.SetActive(false);
			}
		}
    }

    public void SetDefaultParents()
    {
        _parentForObjectsInPool = gameObject.transform;
        _parentForActiveObjects = gameObject.transform;
    }

    public GameObject GetObject(string poolName, Vector3 position)
    {
        if (_enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].PooledObjectQueue.Count > 0)
        {
            GameObject objectFromPool = _enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].PooledObjectQueue.Dequeue();
            objectFromPool.transform.SetParent(_parentForActiveObjects);
            objectFromPool.transform.position = position;
            objectFromPool.SetActive(true);
            return objectFromPool;
        }
        else
        {
            if (_enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].CurrentObjectAmount < _enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].MaxObjectAmount)
            {
                _enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].CurrentObjectAmount++;
                GameObject newEnemyPrefab = _addressableManager.Get(_enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].NameSO.ReferenceName);
                GameObject newObject = Instantiate(newEnemyPrefab, position, Quaternion.identity);
                newObject.name = _enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].NameSO.ReferenceName;
                newObject.transform.SetParent(_parentForActiveObjects);
                newObject.SetActive(false);
                PlayerPoints playerPointsScript = _player.gameObject.GetComponent<PlayerPoints>();
                newObject.GetComponent<EnemyTakeDamage>().Initialize(isReturnToPool: true, this, _lootManager, _soundManager, playerPointsScript);
                
                EnemyMoveToTarget moveToTargetScript = newObject.GetComponent<EnemyMoveToTarget>();
                if (moveToTargetScript != null)
                    moveToTargetScript.Initialize(_player);

                newObject.GetComponent<EnemyMovement>().Initialize(_player);
                newObject.SetActive(true);
                return newObject;
            }
            else
            {
                Tools.LogError(poolName + "Pool has run out of objects!!!");
                return null;
            }
        }
    }

    public void ReturnObject(string poolName, GameObject returnedObject)
    {
        _enemyLevelListSO[_difficultyLevel].EnemyLevelList[_poolName[poolName]].PooledObjectQueue.Enqueue(returnedObject);
        returnedObject.transform.SetParent(_parentForObjectsInPool);
        returnedObject.SetActive(false);
    }
}
