# PoolManager

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

PoolManager based on ScriptableObjects. Use for Enemies, Magic and PopupTexts.

![PoolManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/PoolManager/poolManager01.jpg?raw=true)

MagicPoolManager: poolObjectSO names have to start with "Magic " + magicPrefab name.

## PopupTextPool

![PopupTextPool](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/PoolManager/popupTextPool01.jpg?raw=true)

EnemyTakeDamage.cs: 
	_popupTextPool.GetObject("PopupTextPool", newPosition, damage, _enemyStatsSO.MaxHealth, isCriticalDamage);
	
PopupTextPool.cs: 
	public GameObject GetObject(string poolName, Vector3 position, int damage, int maxHealth, bool isCriticalDamage)
{ ... objectFromPool.GetComponent<PopupText>().SetParams(position, damage, maxHealth, isCriticalDamage); ... }
	
PopupText.cs: 
	public void SetParams(Vector3 position, int damage, int maxHealth, bool isCriticalDamage)

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
