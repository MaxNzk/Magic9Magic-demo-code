# Enemies

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

**SpawnManager:** get enemies from the PoolManager with a given probability.

**PoolManager:** based on ScriptableObjects. Use for Enemies, Magic and PopupTexts.

**EnemyFactory:** for the enemies present on the scene when it starts. Without spawning and pooling.

You can use any option or a combination of them:

1. SpawnManager + PoolManager - for enemyTargetAI

![EnemyPool and EnemySpawner](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Enemies/enemyPoolSpawner01.jpg?raw=true)

![EnemyPool and EnemySpawner](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Enemies/enemyAddressables01.jpg?raw=true)

2. EnemyFactory - without Pooling
	
		EnemyPoint for enemyTargetAI
	
		WayPoint for enemyFSM AI
		
Instantiate all enemies on initialization.

GameManager: _enemyFactory.Initialize(enemySetIndex: _gameSettingsSO.GameDifficulty, _player.transform, _lootManager);

![EnemyFactory](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Enemies/enemyFactory01.jpg?raw=true)

-------------------------------------------------------------------------------------------

**Initialization Path:**

	Initialization Path for 1: GameManager >> (PoolManager >> Enemies) and SpawnManager

	Initialization Path for 2: GameManager >> EnemyFactory >> Enemies

**Project folders:**

	Scripts > Managers > EnemyFactory
	
	Scripts > Managers > PoolManager
	
	Scripts > Managers > SpawnManager
	
	Prefabs > Enemies
	
	Prefabs > Managers > Enemies


	SO for enemy settings in Prefabs > Enemies > EnemyName > EnemyName SO
	
	SO for enemy PoolManager in Scripts > Managers > PoolManager > Enemies WorldX >

**AI:**

	1. Target AI (Simple NavMesh AI): just sets the player as a target and moves towards the player.
	
	2. FSM AI: Pluggable Finite State Machine AI With Scriptable Objects.

-------------------------------------------------------------------------------------------

For Enemies W2 using "Nanolod | Automatic LODs" made LODs.

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
