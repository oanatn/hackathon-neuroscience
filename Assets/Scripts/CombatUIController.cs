using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUIController : MonoBehaviour
{
    [Header("References")]
    public CombatManager combatManager;
    public ConcentrationMeter concentrationMeter;
    public CharacterStats player;
    public CharacterStats enemy;

    [Header("Health UI")]
    public Image playerHealthBar;
    public Image enemyHealthBar;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;

    [Header("Combat UI")]
    public TMP_Text turnText;
    public TMP_Text resultText;
    public TMP_Text concentrationText;

    [Header("Buttons")]
    public Button attackButton;
    public Button runButton;

    private void Start()
    {
        if (attackButton != null)
            attackButton.onClick.AddListener(OnAttackClicked);

        if (runButton != null)
            runButton.onClick.AddListener(OnRunClicked);

        RefreshUI();
    }

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        UpdateHealthUI();
        UpdateConcentrationUI();
        UpdateTurnUI();
        UpdateButtons();
    }

    private void UpdateHealthUI()
    {
        if (player != null)
        {
            if (playerHealthBar != null)
                playerHealthBar.fillAmount = (float)player.currentHealth / player.maxHealth;

            if (playerHealthText != null)
                playerHealthText.text = $"{player.currentHealth}/{player.maxHealth}";
        }

        if (enemy != null)
        {
            if (enemyHealthBar != null)
                enemyHealthBar.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;

            if (enemyHealthText != null)
                enemyHealthText.text = $"{enemy.currentHealth}/{enemy.maxHealth}";
        }
    }

    private void UpdateConcentrationUI()
    {
        if (concentrationMeter == null || concentrationText == null)
            return;

        int percentage = Mathf.RoundToInt(concentrationMeter.Concentration01 * 100f);
        concentrationText.text = $"Concentration: {percentage}%";
    }

    private void UpdateTurnUI()
    {
        if (combatManager == null || turnText == null)
            return;

        switch (combatManager.CurrentTurn)
        {
            case CombatTurn.PlayerChoosing:
                turnText.text = "Choose an action";
                break;

            case CombatTurn.PlayerConcentrating:
                turnText.text = "Concentrate!";
                break;

            case CombatTurn.EnemyTurn:
                turnText.text = "Enemy turn";
                break;

            case CombatTurn.CombatEnded:
                turnText.text = "Combat ended";
                break;
        }
    }

    private void UpdateButtons()
    {
        if (combatManager == null)
            return;

        bool canChoose = combatManager.CurrentTurn == CombatTurn.PlayerChoosing;

        if (attackButton != null)
            attackButton.interactable = canChoose;

        if (runButton != null)
            runButton.interactable = canChoose;
    }

    private void OnAttackClicked()
    {
        if (combatManager != null)
            combatManager.ChooseAttack();

        if (resultText != null)
            resultText.text = "";
    }

    private void OnRunClicked()
    {
        if (combatManager != null)
            combatManager.ChooseRun();

        if (resultText != null)
            resultText.text = "You ran away.";
    }

    public void ShowAttackResult(AttackResult result, int damage)
    {
        if (resultText == null)
            return;

        resultText.text = $"{result}! {damage} damage.";
    }

    public void ShowCombatEnded(bool playerWon)
    {
        if (resultText == null)
            return;

        resultText.text = playerWon ? "You win!" : "You lost.";
    }
}
