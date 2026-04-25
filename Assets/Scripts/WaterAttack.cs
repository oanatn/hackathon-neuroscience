using UnityEngine;

public class WaterAttack : MonoBehaviour, IAttack
{
    public string AttackName => "Water Attack";

    public AttackElement Element => AttackElement.Water;

    public int BaseDamage => 10;

    public string AnimationTrigger => "WaterAttack";

    [Header("Water Effect")]
    [Range(0f, 1f)]
    public float weakenChance = 0.45f;

    public int weakenDuration = 1;

    public void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    )
    {
        int damage = CalculateDamage(concentrationResult);

        defender.TakeDamage(damage);

        Debug.Log($"Water attack dealt {damage} damage.");

        TryApplyWeaken(defender);
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