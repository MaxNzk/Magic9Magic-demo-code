using System.Collections;
using UnityEngine;

public class SpawnManagerType1 : MonoBehaviour, ISpawnManager
{
    [SerializeField] private bool _isDisabledDueToTestMode;

    [Space(10)]
    [SerializeField] private EnemyLevelListSO[] _enemyLevelListSO;
    private int _difficultyLevel;
    private string[] _enemySONames;
    private float[] _probabilities;

    [Space(10)]
    [SerializeField] private SpawnTimeSO _spawnTimeSO;
    private int _spawnTimeArrIndex;
    private Transform[] _spawnPositions;
    private SpawnPos[] _spawnPosScripts;
    [SerializeField] private bool _isGizmos = true;
    [SerializeField] private bool _isGizmosDistance = true;
    [SerializeField] private Color _activeGizmosColor = new Color(0, 1, 0, 0.5f);
    [SerializeField] private Color _inactiveGizmosColor = new Color(1, 0, 0, 0.5f);
    private EnemyPoolManager _poolManager;
    private Transform _playerTransform;

    public void Initialize(EnemyPoolManager poolManager, Transform playerTransform, int difficultyLevel)
    {
        _poolManager = poolManager;
        _playerTransform = playerTransform;
        _difficultyLevel = difficultyLevel;

        FindAndTestComponents();
        InitEnemyNamesAndProbabilities();
        InitSpawnPositions();
    }

    public void StartSpawner()
    {
        if (_isDisabledDueToTestMode == false)
        {
            StartCoroutine("Spawn");
        }
    }

    public void StopSpawner()
    {
        StopCoroutine("Spawn");
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<Transform>(_playerTransform, nameof(_playerTransform), gameObject);
        Tools.CheckNull<EnemyPoolManager>(_poolManager, nameof(_poolManager), gameObject);
        Tools.CheckNull<SpawnTimeSO>(_spawnTimeSO, nameof(_spawnTimeSO), gameObject);
        
        _spawnPosScripts = GetComponentsInChildren<SpawnPos>();
        _spawnPositions = new Transform[_spawnPosScripts.Length];

        for (int i = 0; i < _spawnPosScripts.Length; i++)
        {
            _spawnPositions[i] = _spawnPosScripts[i].GetComponent<Transform>();
        }

        if (_spawnPositions == null)
            Tools.LogError("Transform[] _spawnPositions = NULL");
        if (_spawnPosScripts == null)
            Tools.LogError("SpawnPos[] _spawnPosScripts = NULL");
        if (_spawnPosScripts.Length != _spawnPositions.Length)
            Tools.LogError("_spawnPosScripts != _spawnPositions");
        if (_spawnTimeSO.SpawnTimeArr.Length == 0)
            Tools.LogError("_spawnTimeSO.SpawnTimeArr.Length == 0");
    }

    private void InitEnemyNamesAndProbabilities()
    {
        int length = _enemyLevelListSO[_difficultyLevel].EnemyLevelList.Length;
        _enemySONames = new string[length];
        _probabilities = new float[length];
        for (int i = 0; i < length; i++)
        {
            _enemySONames[i] = _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].NameSO.ReferenceName;
            _probabilities[i] = _enemyLevelListSO[_difficultyLevel].EnemyLevelList[i].Probability;
        }
    }

    private void InitSpawnPositions()
    {
        for (int i = 0; i < _spawnPosScripts.Length; i++)
        {
            _spawnPosScripts[i].Initialize(_playerTransform, _isGizmos, _isGizmosDistance, _activeGizmosColor, _inactiveGizmosColor);
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            float waitSeconds = _spawnTimeArrIndex < _spawnTimeSO.SpawnTimeArr.Length ? _spawnTimeSO.SpawnTimeArr[_spawnTimeArrIndex++] : _spawnTimeSO.SpawnTimeArr[_spawnTimeSO.SpawnTimeArr.Length - 1];
            yield return new WaitForSeconds(waitSeconds);

            int randomEnemyPrefab = GetRandomEnemyIndex(_probabilities);
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
        {
			total += elem;
        }
		
		float randomPoint = Random.value * total;
		for (int i= 0; i < enemyPrefabsProbability.Length; i++)
        {
			if (randomPoint < enemyPrefabsProbability[i])
            {
				return i;
            }
			else
            {
				randomPoint -= enemyPrefabsProbability[i];
            }
        }

		return enemyPrefabsProbability.Length - 1;
	}

}
