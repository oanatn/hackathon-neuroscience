using UnityEngine;

public class WaterAttack : AttackBase
{
    public override string AttackName => "Water Attack";

    public override AttackElement Element => AttackElement.Water;

    public override int BaseDamage => 10;

    public override string AnimationTrigger => "WaterAttack";

    [Header("Water Effect")]
    [Range(0f, 1f)]
    public float weakenChance = 0.45f;

    public int weakenDuration = 1;

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

        Debug.Log($"Water attack dealt {damage} damage.");

        TryApplyWeaken(defender);
    }

    private void TryApplyWeaken(CharacterStats defender)
    {
        float roll = Random.value;

        if (roll <= weakenChance)
        {
            defender.ApplyCondition(ConditionType.Weakened, weakenDuration);
            Debug.Log($"Weaken applied for {weakenDuration} turn.");
        }
        else
        {
            Debug.Log("Weaken failed.");
        }
    }
}