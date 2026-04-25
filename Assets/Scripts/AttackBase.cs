using UnityEngine;

public abstract class AttackBase : MonoBehaviour, IAttack
{
    public abstract string AttackName { get; }
    public abstract AttackElement Element { get; }
    public abstract int BaseDamage { get; }
    public abstract string AnimationTrigger { get; }

    public abstract void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    );

    protected int CalculateDamage(AttackResult result)
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
}