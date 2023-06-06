using UnityEngine;

public class ShieldType3 : MonoBehaviour, IShieldType
{
    private bool _isDamaged = false;

    public int[] CalcDamages(int[] damages, int[] defenseValues)
    {
        _isDamaged = false;
        int[] result = new int[damages.Length];
        for (int i = 0; i < damages.Length; i++)
        {
            if (defenseValues[i] - damages[i] < 0)
            {
                result[i] = -1 * (defenseValues[i] - damages[i]);
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
