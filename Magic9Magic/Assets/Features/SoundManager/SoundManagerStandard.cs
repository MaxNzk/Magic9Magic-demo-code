using System.Collections.Generic;
using UnityEngine;

public class SoundManagerStandard : SoundManager, ISoundManager
{
    [SerializeField] private Transform _parentForCharacterSounds;
    [SerializeField] private Transform _parentForMagicSounds;
    private Dictionary<string, GameObject> _soundDictionary = new Dictionary<string, GameObject>();

    [Space(10)]
    [SerializeField] private SoundSnapshotListSO _snapshotListSO;
    private Dictionary<string, int> _snapshotDictionary = new Dictionary<string, int>();

    public void Initialize()
    {
        FindAndTestComponents();
        FillOutSoundDictionary();
        FillOutSnapshotDictionary();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<SoundManagerStandard>(gameObject);

        Tools.CheckNull<Transform>(_parentForCharacterSounds, nameof(_parentForCharacterSounds), gameObject);
        Tools.CheckNull<Transform>(_parentForMagicSounds, nameof(_parentForMagicSounds), gameObject);
    }

    private void FillOutSoundDictionary()
    {
        Sound[] tmpSounds = GetComponentsInChildren<Sound>();
        foreach(var n in tmpSounds)
        {
            if (_soundDictionary.ContainsKey(n.gameObject.name))
            {
                Tools.LogError(n.gameObject.name + " sound already exists in _soundDictionary");
            }
            else
            {
                _soundDictionary.Add(n.gameObject.name, n.gameObject);
                CreateParallelSound(n.gameObject, n.gameObject.transform.parent);
            }
        }
        if (_soundDictionary.Count == 0)
            Tools.LogError("_soundDictionary is Empty!");
    }

    private void FillOutSnapshotDictionary()
    {
        _snapshotDictionary.Add(SnapshotName.Gameplay.ToString(), 0);
        _snapshotDictionary.Add(SnapshotName.Menu.ToString(), 1);
        _snapshotDictionary.Add(SnapshotName.Cutscene.ToString(), 2);
        _snapshotDictionary.Add(SnapshotName.Dialogue.ToString(), 3);
        _snapshotDictionary.Add(SnapshotName.Popup.ToString(), 4);
    }

    public void AddSound(GameObject sound, string parentType)
    {
        if (_soundDictionary.ContainsKey(sound.name))
            return;

        Transform parentTransform = transform;
        if (parentType == ParentType.Character.ToString()) parentTransform = _parentForCharacterSounds;
        if (parentType == ParentType.Magic.ToString()) parentTransform = _parentForMagicSounds;

        GameObject go = Instantiate(sound, parentTransform);
        go.name = sound.name;
        _soundDictionary.Add(sound.name, go);
        CreateParallelSound(sound, parentTransform);
    }

    private void CreateParallelSound(GameObject sound, Transform parentTransform)
    {
        Sound soundScript = sound.GetComponent<Sound>();
        if (soundScript.IsParallelPlayback == false)
            return;

        int count = soundScript.ParallelSoundAmount;
        for (int i = 1; i < count; i++)
        {
            GameObject go = Instantiate(sound, parentTransform);
            go.name = sound.name + " (" + i.ToString() + ")";
            _soundDictionary.Add(go.name, go);
        }
    }

    // "Click": Canvas > ... > Button_Circle01_Gray (Settings Button) and other buttons
    public void Play(string soundName)
    {
        Play(soundName, Vector3.zero);
    }

    public void Play(string soundName, Vector3 position)
    {
        // Debug.Log("soundName = " + soundName);
        if (System.String.IsNullOrEmpty(soundName) == true)
            return;
        
        if (soundName == "None")
            return;

        Sound currentSound = _soundDictionary[soundName].GetComponent<Sound>();
        if (currentSound.IsPlaying())
        {      
            if (currentSound.IsParallelPlayback)
            {
                string nextSoundName = GetNextSoundName(soundName);
                if (_soundDictionary.ContainsKey(nextSoundName))
                {
                    Play(nextSoundName, position);
                }
            }
        }
        else
        {
            if (position == Vector3.zero)
            {
                currentSound.Play2dSound();
            }
            else
            {
                currentSound.Play3dSound(position);
            }
        }
    }

    public void Pause(string soundName)
    {
        if (_soundDictionary.ContainsKey(soundName))
        {
            _soundDictionary[soundName].GetComponent<Sound>().Pause();
        }
    }

    public void Stop(string soundName)
    {
        if (_soundDictionary.ContainsKey(soundName))
        {
            _soundDictionary[soundName].GetComponent<Sound>().Stop();
        }
    }

    private string GetNextSoundName(string soundName)
    {
        if (soundName.EndsWith(")"))
        {
            int indexStart = soundName.IndexOf("(") + 1;
            int indexEnd = soundName.LastIndexOf(")");
            string s = soundName.Substring(indexStart, indexEnd - indexStart);
            int nextIndex = System.Int32.Parse(s);
            nextIndex++;
            string soundNameWithoutIndex = soundName.Substring(0, indexStart - 2);
            
            return soundNameWithoutIndex + " (" + nextIndex.ToString() + ")";
        }
        else
        {
            return soundName + " (1)";
        }
    }

    public void TransitionTo(string snapshotName)
    {
        int index = _snapshotDictionary[snapshotName];
        float transitionTime = _snapshotListSO.SnapshotList[index]._transitionTime;
        _snapshotListSO.SnapshotList[index]._gamePlaySnapshot.TransitionTo(transitionTime);
    }
}