using System.Collections;
using UnityEngine;

public class SpawnManagerType1 : MonoBehaviour, ISpawnManager
{
    [SerializeField] private PoolObjectSO[] _enemiesSO;
    private string[] _enemySONames;
    [Header("The total should be 1.0")]
    [SerializeField] private float[] _enemyPrefabsProbability;
    [SerializeField] private float _spawnTime;
    private Transform[] _spawnPositions;
    private SpawnPos[] _spawnPosScripts;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private bool _isGizmos = true;
    [SerializeField] private bool _isGizmosDistance = true;
    [SerializeField] private Color _activeGizmosColor = new Color(0, 1, 0, 0.5f);
    [SerializeField] private Color _inactiveGizmosColor = new Color(1, 0, 0, 0.5f);
    private ScriptableObjectPoolManager _poolManager;

    public void Initialize(ScriptableObjectPoolManager poolManager)
    {
        _poolManager = poolManager;
        FindAndTestComponents();
        InitEnemyNamesForPool();
        InitSpawnPositions();
    }

    public void StartSpawner() => StartCoroutine("Spawn");
    public void StopSpawner() => StopCoroutine("Spawn");

    private void FindAndTestComponents()
    {
        _spawnPosScripts = GetComponentsInChildren<SpawnPos>();
        _spawnPositions = new Transform[_spawnPosScripts.Length];
        for (int i = 0; i < _spawnPosScripts.Length; i++)
            _spawnPositions[i] = _spawnPosScripts[i].GetComponent<Transform>();
        if (_spawnPositions == null)
            Tools.LogError("Transform[] _spawnPositions = NULL");
        if (_spawnPosScripts == null)
            Tools.LogError("SpawnPos[] _spawnPosScripts = NULL");
        if (_spawnPosScripts.Length != _spawnPositions.Length)
            Tools.LogError("_spawnPosScripts != _spawnPositions");
        
        if (_poolManager == null)
            Tools.LogError("ScriptableObjectPoolManager _poolManager = NULL");
    }

    private void InitEnemyNamesForPool()
    {
        _enemySONames = new string[_enemiesSO.Length];
        for (int i = 0; i < _enemiesSO.Length; i++)
            _enemySONames[i] = _enemiesSO[i].name;
    }

    private void InitSpawnPositions()
    {
        for (int i = 0; i < _spawnPosScripts.Length; i++)
            _spawnPosScripts[i].Initialize(_playerTransform, _isGizmos, _isGizmosDistance, _activeGizmosColor, _inactiveGizmosColor);
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds _waitForFewSeconds = new WaitForSeconds(_spawnTime);
        while (true)
        {
            yield return _waitForFewSeconds;
            int randomEnemyPrefab = GetRandomEnemyIndex(_enemyPrefabsProbability);
            bool isSpawned = false;
            while (isSpawned == false)
            {
                int randomSpawnPos = Random.Range(0, _spawnPositions.Length);
                if (_spawnPosScripts[randomSpawnPos].Scan())
                {
                    _poolManager.GetObject(_enemySONames[randomEnemyPrefab], _spawnPositions[randomSpawnPos].position);
                    isSpawned = true;
                }
            }
        }
    }

    private int GetRandomEnemyIndex(float[] enemyPrefabsProbability)
    {
		float total = 0;
		foreach (float elem in enemyPrefabsProbability)
			total += elem;
		
		float randomPoint = Random.value * total;
		for (int i= 0; i < enemyPrefabsProbability.Length; i++)
			if (randomPoint < enemyPrefabsProbability[i])
				return i;
			else
				randomPoint -= enemyPrefabsProbability[i];
		return enemyPrefabsProbability.Length - 1;
	}

}
