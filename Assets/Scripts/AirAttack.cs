using UnityEngine;

public class AirAttack : AttackBase
{
    public override string AttackName => "Air Attack";

    public override AttackElement Element => AttackElement.Air;

    public override int BaseDamage => 11;

    public override string AnimationTrigger => "AirAttack";

    [Header("Air Effect")]
    [Range(0f, 1f)]
    public float speedChance = 0.5f;

    public int speedDuration = 1;

    public override void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    )
    {
        int damage = CalculateDamage(concentrationResult);

        // Apply weaken modifier if attacker is affected
        damage = attacker.ModifyOutgoingDamage(damage);

        defender.TakeDamage(damage);

        Debug.Log($"Air attack dealt {damage} damage.");

        TryApplySpeed(attacker);
    }

    private void TryApplySpeed(CharacterStats attacker)
    {
        float roll = Random.value;

        if (roll <= speedChance)
        {
            attacker.ApplyCondition(ConditionType.SpeedBoost, speedDuration);
            Debug.Log($"Speed boost applied for {speedDuration} turn.");
        }
        else
        {
            Debug.Log("Speed boost failed.");
        }
    }
}