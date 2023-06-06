# SoundManager

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

Location of scripts:
![Location of scripts](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound00.jpg?raw=true)

SoundEditor.cs ( : Editor) - add "Play Sound" button to Inspector for each Sound.cs

Public Methods of SoundManager:

	public void Play(string soundName)
	
	public void Play(string soundName, Vector3 position)
	
	public void Pause(string soundName)
	
	public void Stop(string soundName)
	
Audio > MainAudioMixer.mixer

	Snapshots { GamePlay, Cutscene, Dialogue, Popup }
	
	Groups > Master { Music, Ambient, UI, Character, Dialogues, Effects, Footsteps }
	
	Exposed Parameters: MasterVolume, MusicVolume, AmbientVolume, UIVolume, CharacterVolume, DialoguesVolume, EffectsVolume, FootstepsVolume.

-------------------------------------------------------------------------------------------

**Add a new sound:**
1. Add a new audio file to the desired category in the Project window.

![Add a new audio file](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound01.jpg?raw=true)

2. Make a duplicate of a similar prefab and change it.

![Make a duplicate of a similar prefab](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound02.jpg?raw=true)

For enemy sounds: Assets > Prefabs > Enemies > Sounds

For magic sounds: Assets > Prefabs > Magic > {spell name} > (Prefabs >) Sounds 

3. Add the prefab to the desired category in the Hierarchy (SoundNamesUI, SoundNamesCharacter, SoundNamesMagic, SoundNamesFootsteps, SoundNamesDialogues).

![Add the prefab to the desired category](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound03.jpg?raw=true)

4. Add the prefab name to appropriate enum (at the end of it) in SoundManager script.

Attention! Add only at the end! In the opposite case, there will be a global failure of sound names everywhere.

![Add the prefab name to appropriate enum](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound04.jpg?raw=true)

All 3D sounds with a trigger place in 3dSoundsWithTrigger.
![3D sounds with a trigger](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound05.jpg?raw=true)

-------------------------------------------------------------------------------------------

**Footstep System:** Get active textures at a Raycast Hit on Terrain or Renderer and plays the appropriate sound.
![Footstep System](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound06.jpg?raw=true)

For RPG Player - the script is hung on a gameObject with Animator.

For FPC Player - the script is hung on a gameObject with the First Person Controller script.

**"ParallelPlayback"** functionality allows us to play a sound file named "name", and if it is already playing, then search in the SoundManager hierarchy for a "free" file (gameObject) with the name "name (1)" or "name (2), and so on.

-------------------------------------------------------------------------------------------

**Add sounds in runtime**

SoundManager:

	public enum ParentType { Character, Magic }
	
	public void AddSound(GameObject sound, ParentType parentType)

EnemyTakeDamage.cs: _soundManager.AddSound(_soundDeadPrefab.gameObject, SoundManager.ParentType.Character);

WeaponOnShoot.cs, WeaponOnClick.cs, Shield.cs: _soundManager.AddSound(_soundName.gameObject, SoundManager.ParentType.Magic);

**Magic Sounds:**

GameManager***.Awake() > Initialize() > MagicPoolManager.Initialize(...) { ... newObject.GetComponent<ISkillSlot>().Initialize(this, _soundManager); ... }

![Magic sound](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/sound07.jpg?raw=true)

-------------------------------------------------------------------------------------------

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
