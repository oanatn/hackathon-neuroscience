using UnityEngine;

public class FireAttack : MonoBehaviour, IAttack
{
    public string AttackName => "Fire Attack";

    public AttackElement Element => AttackElement.Fire;

    public int BaseDamage => 12;

    public string AnimationTrigger => "FireAttack";

    [Header("Fire Effect")]
    [Range(0f, 1f)]
    public float burnChance = 0.35f;

    public int burnDuration = 3;
    public int burnDamagePerTurn = 5;

    public void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    )
    {
        int damage = CalculateDamage(concentrationResult);

        defender.TakeDamage(damage);

        Debug.Log($"Fire attack dealt {damage} damage.");

        TryApplyBurn(defender);
    }

    private int CalculateDamage(AttackResult result)
    {
        switch (result)
        {
            case AttackResult.Fail:
                return 0;

            case AttackResult.Weak:
                return BaseDamage;

            case AttackResult.Strong:
                return Mathf.RoundToInt(BaseDamage * 1.5f);

            case AttackResult.Special:
                return BaseDamage * 2;

            default:
                return BaseDamage;
        }
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