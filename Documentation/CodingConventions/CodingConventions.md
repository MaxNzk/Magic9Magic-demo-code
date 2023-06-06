# C# Coding Conventions

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions

**Git**

	Git Branching workflow is GitHub Flow.

	Subject line format: "Module name: subject"

	Subject line examples:

		Build: update Timeline package

		Editor: add ReadOnlyAttribute

		ProjectSettings: update TMP Settings

		Doc: update README.md

		Dialogue System: add voices for first dialog

**1.** To shorten the code, it is recommended to use	

	[field: SerializeField] public int[] AttackValues { get; private set; }
	
instead of	

	[SerializeField] private int[] _attackValues;
	
	public int[] AttackValues { get => _attackValues; }


**2.** For method's bool argument without variable use this format:	Method(IsMakeSomething: true);
	
	instead of this: Method(true);

or if something unclear: _poolManager.Initialize(enemySetIndex: _gameSettingsSO.GameDifficulty);

	instead of this: _poolManager.Initialize(_gameSettingsSO.GameDifficulty);


**3.** If a UI element invokes a public method through an event in the Inspector, then you should comment about it in the code. Example:

    // Canvas > UILayerHUD > SettingsButton
    
    // Canvas > UILayerPopup > MenuCamp > Button_Settings
    
    public void LoadSettingsScene()
    
**4.** Use the full form:

	if (MarketScript != null) MarketScript.Initialize(InventoryScript, _wallet, _UIManagerCamp, _playerSettingsSO);

	instead of this: MarketScript?.Initialize(InventoryScript, _wallet, _UIManagerCamp, _playerSettingsSO);
	
**5.** Don't use "!" instead of false in conditional statements.

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
