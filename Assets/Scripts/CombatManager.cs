using System.Collections;
using UnityEngine;

public enum CombatTurn
{
    PlayerChoosing,
    PlayerConcentrating,
    EnemyTurn,
    CombatEnded
}

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public ConcentrationMeter concentrationMeter;
    public CharacterStats player;
    public CharacterStats enemy;
    public BattleAnimatorController battleAnimator;
    public CombatUIController combatUI;

    [Header("Timing")]
    public float concentrationDuration = 10f;
    public float enemyTurnDelay = 1f;

    public CombatTurn CurrentTurn { get; private set; }
    public float ConcentrationTimeRemaining { get; private set; }

    private Coroutine activeRoutine;

    private void Start()
    {
        if (player == null)
            Debug.LogError("Player CharacterStats reference is missing.");

        if (enemy == null)
            Debug.LogError("Enemy CharacterStats reference is missing.");

        if (player != null && player.equippedAttack == null)
            Debug.LogError("Player has no equipped attack.");

        if (enemy != null && enemy.equippedAttack == null)
            Debug.LogError("Enemy has no equipped attack.");

        StartPlayerChoice();
    }

    private void StartPlayerChoice()
    {
        if (IsCombatOver())
            return;

        player.ProcessStartOfTurnConditions();

        if (player.IsDead)
        {
            EndCombat(false);
            return;
        }

        if (player.ShouldSkipTurn())
        {
            activeRoutine = StartCoroutine(EnemyTurnRoutine());
            return;
        }

        CurrentTurn = CombatTurn.PlayerChoosing;
        Debug.Log("Player turn: choose an action.");
    }

    public void ChooseAttack()
    {
        if (CurrentTurn != CombatTurn.PlayerChoosing)
        {
            Debug.Log("Cannot attack right now.");
            return;
        }

        activeRoutine = StartCoroutine(PlayerAttackRoutine());
    }

    public void ChooseRun()
    {
        if (CurrentTurn != CombatTurn.PlayerChoosing)
        {
            Debug.Log("Cannot run right now.");
            return;
        }

        Debug.Log("Player ran away.");
        EndCombat(false);
    }

    private IEnumerator PlayerAttackRoutine()
    {
        CurrentTurn = CombatTurn.PlayerConcentrating;
        ConcentrationTimeRemaining = concentrationDuration;

        Debug.Log("Concentrate now!");

        while (ConcentrationTimeRemaining > 0f)
        {
            ConcentrationTimeRemaining -= Time.deltaTime;
            ConcentrationTimeRemaining = Mathf.Max(0f, ConcentrationTimeRemaining);

            yield return null;
        }
        if (player.equippedAttack == null)
        {
            Debug.LogError("Player has no equipped attack.");
            yield break;
        }

        AttackResult result = concentrationMeter.GetAttackResult();

        if (battleAnimator != null)
            yield return StartCoroutine(battleAnimator.PlayPlayerAttack());

        int enemyHealthBefore = enemy.currentHealth;

        player.equippedAttack.Execute(player, enemy, result);

        int damageDealt = enemyHealthBefore - enemy.currentHealth;

        if (combatUI != null)
            combatUI.ShowAttackResult(result, damageDealt);

        Debug.Log($"Player used {player.equippedAttack.AttackName}. Result: {result}. Damage dealt: {damageDealt}.");

        if (enemy.IsDead)
        {
            EndCombat(true);
            yield break;
        }

        if (player.ConsumeSpeedBoost())
        {
            Debug.Log("Player gets an extra turn from SpeedBoost.");
            StartPlayerChoice();
            yield break;
        }

        activeRoutine = StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        CurrentTurn = CombatTurn.EnemyTurn;
        Debug.Log("Enemy turn.");

        yield return new WaitForSeconds(enemyTurnDelay);

        enemy.ProcessStartOfTurnConditions();

        if (enemy.IsDead)
        {
            EndCombat(true);
            yield break;
        }

        if (enemy.ShouldSkipTurn())
        {
            StartPlayerChoice();
            yield break;
        }

        if (enemy.equippedAttack == null)
        {
            Debug.LogError("Enemy has no equipped attack.");
            yield break;
        }

        if (battleAnimator != null)
            yield return StartCoroutine(battleAnimator.PlayEnemyAttack());

        int playerHealthBefore = player.currentHealth;

        enemy.equippedAttack.Execute(enemy, player, AttackResult.Weak);

        int damageDealt = playerHealthBefore - player.currentHealth;

        Debug.Log($"Enemy used {enemy.equippedAttack.AttackName}. Damage dealt: {damageDealt}.");

        if (player.IsDead)
        {
            EndCombat(false);
            yield break;
        }

        if (enemy.ConsumeSpeedBoost())
        {
            Debug.Log("Enemy gets an extra turn from SpeedBoost.");
            activeRoutine = StartCoroutine(EnemyTurnRoutine());
            yield break;
        }

        StartPlayerChoice();
    }

    private bool IsCombatOver()
    {
        return player == null ||
               enemy == null ||
               player.IsDead ||
               enemy.IsDead ||
               CurrentTurn == CombatTurn.CombatEnded;
    }

    private void EndCombat(bool playerWon)
    {
        CurrentTurn = CombatTurn.CombatEnded;

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (combatUI != null)
            combatUI.ShowCombatEnded(playerWon);

        Debug.Log(playerWon
            ? "Combat ended: player won."
            : "Combat ended: player lost or escaped.");
    }
}