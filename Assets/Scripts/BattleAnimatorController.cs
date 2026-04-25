using System.Collections;
using UnityEngine;

public class BattleAnimatorController : MonoBehaviour
{
    [Header("Animators")]
    public Animator playerAnimator;
    public Animator enemyAnimator;

    [Header("Timing")]
    public float hurtDelay = 0.3f;

    public IEnumerator PlayPlayerAttack()
    {
        playerAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(hurtDelay);

        enemyAnimator.SetTrigger("Hurt");
    }

    public IEnumerator PlayEnemyAttack()
    {
        enemyAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(hurtDelay);

        playerAnimator.SetTrigger("Hurt");
    }
}