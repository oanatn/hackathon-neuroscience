using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    Burning,
    Stunned,
    Weakened,
    SpeedBoost
}

public class CharacterStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("Condition Settings")]
    public int burnDamagePerTurn = 5;
    public float weakenedDamageMultiplier = 0.5f;

    private readonly Dictionary<ConditionType, int> activeConditions =
        new Dictionary<ConditionType, int>();

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        amount = Mathf.Max(0, amount);

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} HP: {currentHealth}/{maxHealth}");
    }

    public void ApplyCondition(ConditionType condition, int duration)
    {
        if (duration <= 0)
            return;

        if (activeConditions.ContainsKey(condition))
        {
            activeConditions[condition] = Mathf.Max(
                activeConditions[condition],
                duration
            );
        }
        else
        {
            activeConditions.Add(condition, duration);
        }

        Debug.Log($"{gameObject.name} gained {condition} for {duration} turn(s).");
    }

    public bool HasCondition(ConditionType condition)
    {
        return activeConditions.ContainsKey(condition);
    }

    public int GetConditionDuration(ConditionType condition)
    {
        if (!activeConditions.ContainsKey(condition))
            return 0;

        return activeConditions[condition];
    }

    public void RemoveCondition(ConditionType condition)
    {
        if (activeConditions.ContainsKey(condition))
        {
            activeConditions.Remove(condition);
            Debug.Log($"{gameObject.name} is no longer {condition}.");
        }
    }

    public void ProcessStartOfTurnConditions()
    {
        if (IsDead)
            return;

        List<ConditionType> conditionsToRemove = new List<ConditionType>();
        List<ConditionType> conditions = new List<ConditionType>(activeConditions.Keys);

        foreach (ConditionType condition in conditions)
        {
            switch (condition)
            {
                case ConditionType.Burning:
                    TakeDamage(burnDamagePerTurn);
                    Debug.Log($"{gameObject.name} took {burnDamagePerTurn} burn damage.");
                    break;
            }

            activeConditions[condition]--;

            if (activeConditions[condition] <= 0)
                conditionsToRemove.Add(condition);
        }

        foreach (ConditionType condition in conditionsToRemove)
        {
            RemoveCondition(condition);
        }
    }

    public bool ShouldSkipTurn()
    {
        if (!HasCondition(ConditionType.Stunned))
            return false;

        RemoveCondition(ConditionType.Stunned);
        Debug.Log($"{gameObject.name} is stunned and skips the turn.");
        return true;
    }

    public int ModifyOutgoingDamage(int baseDamage)
    {
        int finalDamage = baseDamage;

        if (HasCondition(ConditionType.Weakened))
        {
            finalDamage = Mathf.RoundToInt(finalDamage * weakenedDamageMultiplier);
            RemoveCondition(ConditionType.Weakened);

            Debug.Log($"{gameObject.name}'s attack was weakened.");
        }

        return Mathf.Max(0, finalDamage);
    }

    public bool ConsumeSpeedBoost()
    {
        if (!HasCondition(ConditionType.SpeedBoost))
            return false;

        RemoveCondition(ConditionType.SpeedBoost);
        Debug.Log($"{gameObject.name} consumed SpeedBoost.");

        return true;
    }
}