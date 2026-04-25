using UnityEngine;

public class AirAttack : MonoBehaviour, IAttack
{
    public string AttackName => "Air Attack";

    public AttackElement Element => AttackElement.Air;

    public int BaseDamage => 11;

    public string AnimationTrigger => "AirAttack";

    [Header("Air Effect")]
    [Range(0f, 1f)]
    public float speedChance = 0.5f;

    public int speedDuration = 1;

    public void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    )
    {
        int damage = CalculateDamage(concentrationResult);

        defender.TakeDamage(damage);

        Debug.Log($"Air attack dealt {damage} damage.");

        TryApplySpeed(attacker);
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