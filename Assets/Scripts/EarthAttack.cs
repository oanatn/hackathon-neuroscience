using UnityEngine;

public class EarthAttack : MonoBehaviour, IAttack
{
    public string AttackName => "Earth Attack";

    public AttackElement Element => AttackElement.Earth;

    public int BaseDamage => 15;

    public string AnimationTrigger => "EarthAttack";

    [Header("Earth Effect")]
    [Range(0f, 1f)]
    public float stunChance = 0.35f;

    public void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    )
    {
        int damage = CalculateDamage(concentrationResult);

        defender.TakeDamage(damage);

        Debug.Log($"Earth attack dealt {damage} damage.");

        TryApplyStun(defender);
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

    private void TryApplyStun(CharacterStats defender)
    {
        float roll = Random.value;

        if (roll <= stunChance)
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