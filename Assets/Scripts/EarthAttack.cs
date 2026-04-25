using UnityEngine;

public class EarthAttack : AttackBase
{
    public override string AttackName => "Earth Attack";
    public override AttackElement Element => AttackElement.Earth;
    public override int BaseDamage => 15;
    public override string AnimationTrigger => "EarthAttack";

    [Header("Earth Effect")]
    [Range(0f, 1f)]
    public float stunChance = 0.35f;

    public override void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    )
    {
        int damage = CalculateDamage(concentrationResult);

        damage = attacker.ModifyOutgoingDamage(damage);

        defender.TakeDamage(damage);

        Debug.Log($"Earth attack dealt {damage} damage.");

        TryApplyStun(defender);
    }

    private void TryApplyStun(CharacterStats defender)
    {
        if (Random.value <= stunChance)
        {
            defender.ApplyCondition(ConditionType.Stunned, 1);
            Debug.Log("Stun applied!");
        }
        else
        {
            Debug.Log("Stun failed.");
        }
    }
}