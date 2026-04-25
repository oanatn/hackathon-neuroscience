using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

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
}