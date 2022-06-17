using UnityEngine;

public interface ISkillSlot
{
    Sprite Icon { get; }
    int Mana { get; }
    float CooldownTime { get; }
    int AttackAnimationType { get; }

    void Use(Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition, SoundManager soundManager);
}
