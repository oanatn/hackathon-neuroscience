using UnityEngine;

public class FireAttack : AttackBase
{
    public override string AttackName => "Fire Attack";

    public override AttackElement Element => AttackElement.Fire;

    public override int BaseDamage => 12;

    public override string AnimationTrigger => "FireAttack";

    [Header("Fire Effect")]
    [Range(0f, 1f)]
    public float burnChance = 0.35f;

    public int burnDuration = 3;
    public int burnDamagePerTurn = 5;

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

        Debug.Log($"Fire attack dealt {damage} damage.");

        TryApplyBurn(defender);
    }

    private void TryApplyBurn(CharacterStats defender)
    {
        float roll = Random.value;

        if (roll <= burnChance)
        {
            defender.ApplyCondition(
                ConditionType.Burning,
                burnDuration
            );

            Debug.Log($"Burn applied for {burnDuration} turns.");
        }
        else
        {
            Debug.Log("Burn failed.");
        }
    }
}