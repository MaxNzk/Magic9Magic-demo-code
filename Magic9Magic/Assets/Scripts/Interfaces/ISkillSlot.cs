using UnityEngine;

public interface ISkillSlot
{
    Sprite Icon { get; }
    int Mana { get; }
    float CooldownTime { get; }
    int AttackAnimationType { get; }

    void Initialize(MagicPoolManager magicPoolManager, ISoundManager soundManager);
    void Use(CharacterStats characterStats, Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition);
}
