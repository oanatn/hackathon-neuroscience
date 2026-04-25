public enum AttackElement
{
    Fire,
    Earth,
    Water,
    Air
}

public interface IAttack
{
    string AttackName { get; }

    AttackElement Element { get; }

    int BaseDamage { get; }

    string AnimationTrigger { get; }

    void Execute(
        CharacterStats attacker,
        CharacterStats defender,
        AttackResult concentrationResult
    );
}