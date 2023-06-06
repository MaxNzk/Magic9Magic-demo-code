using UnityEngine;

[CreateAssetMenu(fileName = "WorldNameSO", menuName = "Scriptable Objects/WorldDataSO")]
public class WorldDataSO : ScriptableObject
{
    [field: SerializeField] public string WorldSOName { get; private set; }

    [Space(10)]
    [SerializeField] private bool[] _isMagicTypesExist = new bool[9] { true, true, true, true, true, true, true, true, true };
    public bool[] IsMagicTypesExist => _isMagicTypesExist;
    
    [Header("// Equal to 0 means no restrictions")]
    [SerializeField] private int[] _maxLevelCraftingSpells = new int[9];
    public int[] MaxLevelCraftingSpells { get => _maxLevelCraftingSpells; }

    private void OnEnable()
    {
        WorldSOName = this.name;
    }
    
}
