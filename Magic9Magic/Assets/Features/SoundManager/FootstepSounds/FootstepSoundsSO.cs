using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FootstepSounds")]
public class FootstepSoundsSO : ScriptableObject
{
    public Texture Albedo;
    public Color Color;
    public SoundManager.SoundNamesFootsteps WalkSoundName;
    public SoundManager.SoundNamesFootsteps RunSoundName;
    public SoundManager.SoundNamesFootsteps JumpSoundName;
}
