using UnityEngine;

[System.Serializable]
public class EnemyTargetAIList
{
    public Transform EnemyPoint;
    public GameObject[] EnemyPrefabList;
}

[System.Serializable]
public class EnemyFSMaiList
{
    public StateSO StartFSMState;
    public Transform[] WayPointList;
    public GameObject[] EnemyPrefabList;
}

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private EnemyTargetAIList[] _enemyTargetAIList;
    [SerializeField] private EnemyFSMaiList[] _enemyFSMaiList;
    private ILootManager _lootManager;
    private ISoundManager _soundManager;

    public void Initialize(int enemySetIndex, Transform player, ILootManager lootManager, ISoundManager soundManager)
    {
        _lootManager = lootManager;
        _soundManager = soundManager;

        for (int i = 0; i < _enemyTargetAIList.Length; i++)
        {
            Transform enemyTransform = _enemyTargetAIList[i].EnemyPoint;
            GameObject newObject = (GameObject) Instantiate(_enemyTargetAIList[i].EnemyPrefabList[enemySetIndex], enemyTransform.position, Quaternion.identity);
            newObject.transform.SetParent(enemyTransform.parent);
            newObject.SetActive(false);
            PlayerPoints playerPointsScript = player.gameObject.GetComponent<PlayerPoints>();
            newObject.GetComponent<EnemyTakeDamage>().Initialize(isReturnToPool: false, null, _lootManager, _soundManager, playerPointsScript);

            EnemyMoveToTarget moveToTargetScript = newObject.GetComponent<EnemyMoveToTarget>();
            if (moveToTargetScript != null)
                moveToTargetScript.Initialize(player);
            
            newObject.GetComponent<EnemyMovement>().Initialize(player);
            newObject.SetActive(true);
        }
        for (int i = 0; i < _enemyFSMaiList.Length; i++)
        {
            Transform enemyTransform = _enemyFSMaiList[i].WayPointList[0];
            GameObject enemy = Instantiate(_enemyFSMaiList[i].EnemyPrefabList[enemySetIndex], enemyTransform.position, Quaternion.identity);
            enemy.transform.SetParent(enemyTransform.parent);
            enemy.GetComponent<EnemyFSMController>().CurrentState = _enemyFSMaiList[i].StartFSMState;
            enemy.GetComponent<EnemyFSMController>().WayPointList.AddRange(_enemyFSMaiList[i].WayPointList);
            enemy.SetActive(false);
            PlayerPoints playerPointsScript = player.gameObject.GetComponent<PlayerPoints>();
            enemy.GetComponent<EnemyTakeDamage>().Initialize(isReturnToPool: false, null, _lootManager, _soundManager, playerPointsScript);
            enemy.GetComponent<EnemyMovement>().Initialize(player);
            enemy.SetActive(true);
        }
    }
    
}
