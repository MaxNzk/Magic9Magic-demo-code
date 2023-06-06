using System.Collections.Generic;
using UnityEngine;

public class MagicPoolManager : MonoBehaviour
{
    [SerializeField] private PoolObjectSO[] _pooledObjects;
    private Dictionary<string, int> _poolName = new Dictionary<string, int>(); // <PoolObjectSO.name, index of _pooledObjects>
    [SerializeField] private Transform _parentForObjectsInPool;
    [SerializeField] private Transform _parentForActiveObjects;
    private ISoundManager _soundManager;

    public void Initialize(ISoundManager soundManager)
    {
        FindAndTestComponents();

        _soundManager = soundManager;

        for (int i = 0; i < _pooledObjects.Length; i++) 
		{
            _poolName.Add(_pooledObjects[i].name, i);
            _pooledObjects[i].PooledObjectQueue = new Queue<GameObject>();
            _pooledObjects[i].CurrentObjectAmount = _pooledObjects[i].ObjectAmount;
            
			for (int j = 0; j < _pooledObjects[i].ObjectAmount; j++)
			{
				GameObject newObject = Instantiate(_pooledObjects[i].ObjectPrefab);
                newObject.name = _pooledObjects[i].name;
                newObject.transform.SetParent(_parentForObjectsInPool);
                _pooledObjects[i].PooledObjectQueue.Enqueue(newObject);
                newObject.GetComponent<ISkillSlot>().Initialize(this, _soundManager);
                newObject.SetActive(false);
			}
		}
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<MagicPoolManager>(gameObject);
        if (_pooledObjects.Length == 0)
            Tools.LogError("PoolManager > MagicPoolManagers: _pooledObjects is Empty!");
    }

    public void SetDefaultParents()
    {
        _parentForObjectsInPool = gameObject.transform;
        _parentForActiveObjects = gameObject.transform;
    }

    public GameObject GetObject(string poolName, Vector3 position)
    {
        if (_pooledObjects[_poolName[poolName]].PooledObjectQueue.Count > 0)
        {
            GameObject objectFromPool = _pooledObjects[_poolName[poolName]].PooledObjectQueue.Dequeue();
            objectFromPool.transform.SetParent(_parentForActiveObjects);
            objectFromPool.transform.position = position;
            objectFromPool.SetActive(true);
            return objectFromPool;
        }
        else
        {
            if (_pooledObjects[_poolName[poolName]].CurrentObjectAmount < _pooledObjects[_poolName[poolName]].MaxObjectAmount)
            {
                _pooledObjects[_poolName[poolName]].CurrentObjectAmount++;
                GameObject newObject = Instantiate(_pooledObjects[_poolName[poolName]].ObjectPrefab, position, _pooledObjects[_poolName[poolName]].ObjectPrefab.transform.rotation);
                newObject.name = _pooledObjects[_poolName[poolName]].name;
                newObject.transform.SetParent(_parentForActiveObjects);
                newObject.GetComponent<ISkillSlot>().Initialize(this, _soundManager);
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
        _pooledObjects[_poolName[poolName]].PooledObjectQueue.Enqueue(returnedObject);
        returnedObject.transform.SetParent(_parentForObjectsInPool);
        returnedObject.SetActive(false);
    }
}
