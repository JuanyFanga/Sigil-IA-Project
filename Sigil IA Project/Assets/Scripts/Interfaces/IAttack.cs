using System;

public interface IAttack
{
    void Attack();
    float GetAttackRange { get; }
    Action OnAttack { get; set; }
    Cooldown Cooldown { get; }
}
