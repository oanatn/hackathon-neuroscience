using System.Collections;
using UnityEngine;

public class BattleAnimationTester : MonoBehaviour
{
    [Header("Character Animators")]
    public Animator character1Animator;
    public Animator character2Animator;

    [Header("Animation Timing")]
    public float hurtDelay = 0.3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Character1Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Character2Attack();
        }
    }

    public void Character1Attack()
    {
        StartCoroutine(AttackRoutine(character1Animator, character2Animator));
    }

    public void Character2Attack()
    {
        StartCoroutine(AttackRoutine(character2Animator, character1Animator));
    }

    private IEnumerator AttackRoutine(Animator attacker, Animator defender)
    {
        attacker.SetTrigger("Attack");

        yield return new WaitForSeconds(hurtDelay);

        defender.SetTrigger("Hurt");
    }
}