using UnityEngine;

public interface ISoundManager
{
    void Initialize();
    void AddSound(GameObject sound, string parentType);
    void Play(string soundName);
    void Play(string soundName, Vector3 position);
    void Pause(string soundName);
    void Stop(string soundName);
    void TransitionTo(string snapshotName);    
}