public interface IShieldType
{
    int[] CalcDamages(int[] damages, int[] defenseValues);
    bool IsDamaged();
}
