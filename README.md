# Magic9Magic

_Fork from my private repository. This document contains only brief information, full information in a private document. Keywords: game, Unity, C#_

[Magic9Magic Demo builds](https://drive.google.com/drive/folders/15Oc2qxO1DV7XEaaKJv2pWv8EiUG3HH7I/)

## Table of Сontents

1. [Game description](/README.md#game-description)
	
2. [Todo List](/README.md#todo-list)
	
3. [Brief description of the technical side](/README.md#brief-description-of-the-technical-side)

## Game description:
**Game name:** Magic9Magic

**Game genre:** multi-genre

**Game setting:** Fantasy


## Todo List:
#### Milestones:
1. Build architecture and two worlds
2. Build Turn-based strategy type
3. Multiplayer mode
4. 2D Platformer type
5. Build world with crowds of enemies (DOTS)

#### Milestone 1:
* Unity Services Accounts
* Enemies (architecture and new types)
* Magic System (+ crafting)
* Journal (Quests, BattlePass, Achievements)
* Procedural level generation
* Refactoring
* Android build


## Brief description of the technical side:
**Unity version:** 2022.3.0f1 (URP)

**Git Branching** workflow is GitHub Flow.

-------------------------------------------------------------------------------------------

**Scenes:**

Scenes in Build (index): StartScene (0), CampScene (1), SettingsScene (2), World1Scene (3), World1v2Scene (4), World2Scene (5).

StartScene - auto jump to next scene (SettingsScene or CampScene).

	First game launch: StartScene > SettingsScene > CampScene.

	Next game launches: StartScene > CampScene.

In Unity Edit Mode you can run any scene. That is, you do not need to run from the StartScene.

"Entry point" is GameManager (Awake() > Initialize()) in any scene.
GameManager for different worlds do not inherit from a common base class (at least not yet).

**The Version can only be a float type due to the update check feature.**

-------------------------------------------------------------------------------------------

[GameManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/GameManager/GameManager.md)

[Game Difficulty](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/GameDifficulty/GameDifficulty.md)

[Portal](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Portal/Portal.md)

[Enemies](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Enemies/Enemies.md)

[PoolManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/PoolManager/PoolManager.md)

[UIManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/UIManager/UIManager.md)

[Cutscenes](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Cutscenes/Cutscenes.md)

[DialogueSystem](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/DialogueSystem/DialogueSystem.md)

[Translation](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Translation/Translation.md)

[SoundManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/SoundManager/SoundManager.md)

[Inventory/Market and Stats](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Inventory/Inventory.md)

[LootManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/LootManager/LootManager.md)

[Analytics](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Analytics/Analytics.md)

[Editor Tools](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/EditorTools/EditorTools.md)
		
[Third-party Resources](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/ThirdPartyResources/ThirdPartyResources.md)

[C# Coding Conventions](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/CodingConventions/CodingConventions.md)

[Architectural remarks](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/ArchitecturalRemarks/ArchitecturalRemarks.md)

**ScriptableObects** are used for local data storage. Helps to avoid Singletons for passing data between scenes. And to implement the Flyweight pattern.

SOs use for: analytics, gameSettings, playerSettings, enemy stats, objectPoolSettings, magicSettings, WorldSettings - data for procedural world generation, AI FSM, FinishMissionSO for missionTypes, "Stats/Inventory/Market".

**SaveManager:** save/load ScriptableObjects in/from json. Save provider Type: Local and UnityCloud (empty yet).

**InputManager:** use new Input System. Old Input System for Camp and World1 (transition to a new Imput System in the near future).

**Accessibility:** Implemented "Color Blind (CVD)" mode.

**Camera** uses Cinemachine and post-processing with integrated CVD mode.

**Light baking:** CampScene, World1Scene, World1v2Scene, World2Scene

	Assets > Scenes > LightingSettings > CampSceneSettings: CampScene, World1Scene and World1v2Scene.
	Assets > Scenes > LightingSettings > World2SceneSettings: World2Scene.

	CampScene: Directional Light Intensity for baking 1.3 and 1.0 after baking.
	World1Scene: Directional Light Intensity for baking 1.1 and 1.0 after baking.
	World1v2Scene: Directional Light Intensity for baking 1.1 and 1.0 after baking.
	World2Scene: Directional Light Intensity for baking 0.25 and 0.25 after baking.

**Light Probes:** CampScene and World2Scene.

**Occlusion Culling:** World1Scene, World1v2Scene and World2Scene.

**Remote Settings:** checking for the game update in CampScene (GameManagerCamp, FSM).

**Unity Cloud Diagnostics** is On.
