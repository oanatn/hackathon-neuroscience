using System.Collections;
using UnityEngine;

public class BattleAnimationTester : MonoBehaviour
{
    [Header("Character Animators")]
    public Animator character1Animator;
    public Animator character2Animator;

    [Header("Health")]
    public SimpleHealth character1Health;
    public SimpleHealth character2Health;

    [Header("Combat Settings")]
    public int damage = 10;
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
        StartCoroutine(AttackRoutine(character1Animator, character2Animator, character2Health));
    }

    public void Character2Attack()
    {
        StartCoroutine(AttackRoutine(character2Animator, character1Animator, character1Health));
    }

    private IEnumerator AttackRoutine(Animator attacker, Animator defender, SimpleHealth defenderHealth)
    {
        attacker.SetTrigger("Attack");

        yield return new WaitForSeconds(hurtDelay);

        defender.SetTrigger("Hurt");

        // 👇 THIS is the new part
        defenderHealth.TakeDamage(damage);
    }
}