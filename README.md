# Magic9Magic Demo code (295MB)

_Fork from my private repository. This document contains only brief information, full information in a private document. Keywords: game, Unity, C#_

## Game description:
**Game name:** Magic9Magic

**Description:** The most...

**Platforms:** focus on maximum cross-platform ( + one account in multiple devices)

**References:** Magicka, Divinity Original Sin, Magica.io (Mobile)

**Accessibility:** Colorblind Mode.

## Todo List:
#### Milestones:
1. build architecture and two worlds (RPG and FPS)
2. build TBS world
3. build world with crowds of enemies (DOTS)
4. multiplayer mode

#### Milestones 1:
* Tutorial (FPS world)
* NavMeshAgent (surrounding, curved paths)
* Inventory System
* Market
* Magic System (+ crafting)
* Journal (Quests, BattlePass, Achievements)
* Teleport: difficulty levels
* Procedural level generation
* Addressable Asset System (local)
* Dialogue System


## Brief description of the technical side:
**Unity version:** 2021.3.1f1 LTS

Scenes in Build (index): StartScene (0), CampScene (1), SettingsScene (2), World1Scene (3), World2Scene (4)

**"Entry point"** is in GameManager ( Awake() > Initialize() )

**ScriptableObects** are used for local data storage. SOs use for: analytics, gameSettings, playerSettings, enemy settings (Flyweight pattern), objectPoolSettings, magicSettings, WorldSettings - data for procedural world generation (Prototype pattern)

**Analytics System:** Unity Analytics
Analytics data is stored in ScriptableObject - AnalyticsStorage.asset (_analyticsStorageSO)

**SaveManager:** save and load ScriptableObjects in/from json.

**UIManager:** implemented via Mediator pattern.

**SpawnManager:** get enemies from the PoolManager with a given probability.

**PoolManager:** based on ScriptableObjects. Use for Enemies, PopupTexts (and particles in the near future).

**Translation** (UI) via free "Lean Localization" plugin. Dialogue via buil-in translation system in "Dialogue System for Unity" plugin.

**InputManager:** use new Input System. Old Input System for Camp and World1 (transition to a new Imput System in the near future). 

**Footstep System:** Get active textures at a Raycast Hit on Terrain or Renderer.

**Unity Remote Settings:** checking for the game update.

**Camera** uses Cinemachine.

**Cutscene** (Timeline, Cinemachine)

**AI:**

	1. NavMesh AI: just go to Player
	
	2. Finite State Machine (FSM): Pluggable AI With Scriptable Objects
	
**Optimization** has not been done yet.

## Side resources used:				
	TOOLS:			
		Lean Localization
		https://assetstore.unity.com/packages/tools/localization/lean-localization-28504
		Dialogue System for Unity
		https://assetstore.unity.com/packages/tools/ai/dialogue-system-for-unity-11672
		CVD Filter
		https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/cvd-filter-post-processing-v2-127303
		Starter Assets - First Person Character Controller (free): as a basis for development
		https://assetstore.unity.com/packages/essentials/starter-assets-first-person-character-controller-196525
	3D:		
		3D Animations:	
			Mixamo (free)
			https://www.mixamo.com/
		3D Characters:	
			Mixamo (free): Aib
			https://www.mixamo.com/
			Battle Wizard Poly Art (free): seller in Camp
			https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/battle-wizard-poly-art-128097
			LowPoly Fantasy Monsters Pack Ver1.0_DEMO (free): enemies
			https://assetstore.unity.com/packages/3d/characters/creatures/lowpoly-fantasy-monsters-pack-ver1-0-demo-98393
		3D Environments:	
			LowPoly Environment Pack (free): CampScene, World1Scene, World2Scene
			https://assetstore.unity.com/packages/3d/environments/landscapes/lowpoly-environment-pack-99479
			The Free Medieval and War Props (free): bonfire
			https://assetstore.unity.com/packages/3d/props/the-free-medieval-and-war-props-174433
		3D Props:	
			Toon Crystals Pack (free)
			https://assetstore.unity.com/packages/3d/props/toon-crystals-pack-66182
			Gold Coins (free)
			https://assetstore.unity.com/packages/3d/props/gold-coins-1810
	2D:
		2D Fonts:
			Noto Sans Simplified Chinese
			https://fonts.google.com/noto/specimen/Noto+Sans+SC?subset=chinese-simplified
		2D GUI:
			GUI PRO Kit - Fantasy RPG
			https://assetstore.unity.com/packages/2d/gui/gui-pro-kit-fantasy-rpg-170168
				
	AUDIO:
		Fantasy Sounds Bundle
		https://assetstore.unity.com/packages/audio/sound-fx/fantasy-sounds-bundle-193760
		Magic Spells Sound Effects
		https://assetstore.unity.com/packages/audio/sound-fx/magic-spells-sound-effects-114628
	VFX:	
		Fire Ice Projectile - Explosion (free)
		https://assetstore.unity.com/packages/vfx/particles/fire-ice-projectile-explosion-217688
  
  
## C# Coding Conventions
https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions

To shorten the code, it is recommended to use	

	[field: SerializeField] public int[] AttackValues { get; private set; }
	
instead of	

	[SerializeField] private int[] _attackValues;
	
	public int[] AttackValues { get => _attackValues; }

...
