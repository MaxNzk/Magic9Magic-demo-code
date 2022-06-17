using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, Sound> _soundDictionary = new Dictionary<string, Sound>();

    // Order: 2d (UI), 3d (Effects), Footsteps
    public enum SoundNames
    {
        Click,
        AppearingPortal, SendingPortal,
        FireballStart, Fireball, FireMeteor01, Shield01,
        FootstepsSandWalk, FootstepsSandRun, FootstepsSandJump,
        FootstepsDirtyGroundWalk, FootstepsDirtyGroundRun, FootstepsDirtyGroundJump,
        FootstepsGrassWalk, FootstepsGrassRun, FootstepsGrassJump,
        FootstepsGravelWalk, FootstepsGravelRun, FootstepsGravelJump,
        FootstepsLeavesWalk, FootstepsLeavesRun, FootstepsLeavesJump,
        FootstepsMetalWalk, FootstepsMetalRun, FootstepsMetalJump,
        FootstepsMudWalk, FootstepsMudRun, FootstepsMudJump,
        FootstepsRockWalk, FootstepsRockRun, FootstepsRockJump,
        FootstepsSnowWalk, FootstepsSnowRun, FootstepsSnowJump,
        FootstepsTileWalk, FootstepsTileRun, FootstepsTileJump,
        FootstepsWaterWalk, FootstepsWaterRun, FootstepsWaterJump,
        FootstepsWoodWalk, FootstepsWoodRun, FootstepsWoodJump
    }

    private void Awake()
    {
        FindAndTestComponents();
        FillOutSoundDictionary();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<SoundManager>(gameObject);
    }

    private void FillOutSoundDictionary()
    {
        Sound[] tmpSounds = FindObjectsOfType<Sound>();
        foreach(var n in tmpSounds)
            if (_soundDictionary.ContainsKey(n.gameObject.name))
            {
                Tools.LogError(n.gameObject.name + " sound already exists in _soundDictionary");
            }
            else
            {
                _soundDictionary.Add(n.gameObject.name, n);
            }
        if (_soundDictionary.Count == 0)
            Tools.LogError("_soundDictionary is Empty!");
    }

    public void Play(string soundName)
    {
        if (System.String.IsNullOrEmpty(soundName) == false)
            if (_soundDictionary[soundName].IsPlaying())
            {
                if (_soundDictionary[soundName].IsParallelPlayback)
                {
                    string nextSoundName = GetNextSoundName(soundName);
                    if (_soundDictionary.ContainsKey(nextSoundName))
                        Play(nextSoundName);
                }
            }
            else
            {
                _soundDictionary[soundName].Play2dSound();
            }
    }
    // "Click": Canvas > Button_Circle01_Gray (Settings Button)

    public void Play(string soundName, Vector3 position)
    {
        if (System.String.IsNullOrEmpty(soundName) == false)
            if (_soundDictionary[soundName].IsPlaying())
            {
                if (_soundDictionary[soundName].IsParallelPlayback)
                {
                    string nextSoundName = GetNextSoundName(soundName);
                    if (_soundDictionary.ContainsKey(nextSoundName))
                        Play(nextSoundName, position);
                }
            }
            else
            {
                _soundDictionary[soundName].Play3dSound(position);
            }
    }

    public void Stop(string soundName)
    {
        if (_soundDictionary.ContainsKey(soundName))
            _soundDictionary[soundName].Stop();
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

}
