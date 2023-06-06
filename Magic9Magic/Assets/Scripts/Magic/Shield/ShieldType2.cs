using UnityEngine;

public class ShieldType2 : MonoBehaviour, IShieldType
{
    [SerializeField] private int minDefenseValuesDivider = 4;
    private bool _isDamaged = false;
    
    public int[] CalcDamages(int[] damages, int[] defenseValues)
    {
        _isDamaged = false;
        int[] result = new int[damages.Length];
        for (int i = 0; i < damages.Length; i++)
        {
            bool lessThan = damages[i] < defenseValues[i] / minDefenseValuesDivider;
            bool moreThan = damages[i] > defenseValues[i];
            if (lessThan || moreThan)
            {
                result[i] = damages[i];
                _isDamaged = true;
            }
        }
        return result;
    }

    public bool IsDamaged()
    {
        return _isDamaged;
    }

}
