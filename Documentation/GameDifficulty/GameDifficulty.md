# Game Difficulty

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

**Place of difficulty setting:**

    _gameSettingsSO.GameDifficulty set in CampScene: Portal.cs

    public void ChooseDistance(int variant) { _gameSettingsSO.GameDifficulty = variant;

**Where does it count:**

	Enemy difficulty (GameManager.CalcWorldLevel())

	Mission time (+-)

	Chance of dropping more/less rare loot


[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
