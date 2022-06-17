using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [field: SerializeField] public int[] AttackValues { get; private set; }
    [field: SerializeField] public int[] DefenseValues { get; private set; }
    [Header("0 = Fire, 1 = Water, 2 = Earth")]
    [Header("3 = Wind, 4 = Metal, 5 = Lightning")]
    [Header("6 = Life, 7 = Death, 8 = Space")]
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxMana;
    public int CurrentMana;
    public int XpPoints;

}
