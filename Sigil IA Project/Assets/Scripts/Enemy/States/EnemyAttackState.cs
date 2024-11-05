using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttackState : State<StateEnum>
{
    IAttack _attack;
    public Action OnAttack = delegate { };
    public EnemyAttackState(IAttack attack)
    {
        _attack = attack;
    }
    public override void Enter()
    {
        base.Enter();
        OnAttack();
        //Debug.Log("Entro al ataque");
    }
    public override void Execute()
    {
        base.Execute();
    }
}