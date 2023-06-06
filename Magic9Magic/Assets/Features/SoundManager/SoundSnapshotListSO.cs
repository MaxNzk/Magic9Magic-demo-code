using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSnapshotList", menuName = "Scriptable Objects/SoundSnapshotList")]
public class SoundSnapshotListSO : ScriptableObject
{
    public List<SnapshotWithTransitionTime> SnapshotList = new List<SnapshotWithTransitionTime>();

    [System.Serializable]
    public class SnapshotWithTransitionTime
    {
        public AudioMixerSnapshot _gamePlaySnapshot;
        public float _transitionTime;
    }
}
    