using UnityEngine;

public interface ISpawnManager
{
    void Initialize(EnemyPoolManager poolManager, Transform playerTransform, int difficultyLevel);
    void StartSpawner();
    void StopSpawner();
}
