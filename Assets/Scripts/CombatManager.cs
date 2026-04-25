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
    public float concentrationDuration = 2f;
    public float enemyTurnDelay = 1f;

    [Header("Player Damage")]
    public int weakDamage = 10;
    public int strongDamage = 20;
    public int specialDamage = 35;

    [Header("Enemy Damage")]
    public int enemyDamage = 12;

    public CombatTurn CurrentTurn { get; private set; }

    private Coroutine activeRoutine;

    private void Start()
    {
        StartPlayerChoice();
    }

    private void StartPlayerChoice()
    {
        if (IsCombatOver())
            return;

        CurrentTurn = CombatTurn.PlayerChoosing;
        Debug.Log("Player turn: choose an action.");
    }

    // Hook this to the Attack button.
    public void ChooseAttack()
    {
        if (CurrentTurn != CombatTurn.PlayerChoosing)
        {
            Debug.Log("Cannot attack right now.");
            return;
        }

        activeRoutine = StartCoroutine(PlayerAttackRoutine());
    }

    // Hook this to the Run button.
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
        Debug.Log("Concentrate now!");

        yield return new WaitForSeconds(concentrationDuration);

        AttackResult result = concentrationMeter.GetAttackResult();
        int damage = GetDamageFromAttackResult(result);

        if (battleAnimator != null)
            yield return StartCoroutine(battleAnimator.PlayPlayerAttack());

        enemy.TakeDamage(damage);

        if (combatUI != null)
            combatUI.ShowAttackResult(result, damage);

        Debug.Log($"Player attack result: {result}. Damage dealt: {damage}.");

        if (enemy.IsDead)
        {
            EndCombat(true);
            yield break;
        }

        activeRoutine = StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        CurrentTurn = CombatTurn.EnemyTurn;

        Debug.Log("Enemy turn.");

        yield return new WaitForSeconds(enemyTurnDelay);

        if (battleAnimator != null)
            yield return StartCoroutine(battleAnimator.PlayEnemyAttack());

        player.TakeDamage(enemyDamage);

        Debug.Log($"Enemy dealt {enemyDamage} damage.");

        if (player.IsDead)
        {
            EndCombat(false);
            yield break;
        }

        StartPlayerChoice();
    }

    private int GetDamageFromAttackResult(AttackResult result)
    {
        switch (result)
        {
            case AttackResult.Fail:
                return 0;

            case AttackResult.Weak:
                return weakDamage;

            case AttackResult.Strong:
                return strongDamage;

            case AttackResult.Special:
                return specialDamage;

            default:
                return 0;
        }
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

        if (playerWon)
            Debug.Log("Combat ended: player won.");
        else
            Debug.Log("Combat ended: player lost or escaped.");
    }
}