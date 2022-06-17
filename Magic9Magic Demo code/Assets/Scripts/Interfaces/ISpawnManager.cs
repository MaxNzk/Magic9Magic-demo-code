public interface ISpawnManager
{
    void Initialize(ScriptableObjectPoolManager poolManager);
    void StartSpawner();
    void StopSpawner();
}
