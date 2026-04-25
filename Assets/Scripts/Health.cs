using UnityEngine;
using UnityEngine.UI;

public class SimpleHealth : MonoBehaviour
{
    public Image fillImage;

    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateBar();
    }

    private void UpdateBar()
    {
        fillImage.fillAmount = (float)currentHealth / maxHealth;
    }
}