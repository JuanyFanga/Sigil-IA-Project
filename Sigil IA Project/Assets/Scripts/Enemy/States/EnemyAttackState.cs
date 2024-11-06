using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttackState : State<StateEnum>
{
    IAttack _attack;
    IMove _move;
    Transform _transform;
    public Action OnAttack = delegate { };
    public EnemyAttackState(IAttack attack, IMove move, Transform transform)
    {
        _attack = attack;
        _move = move;
        _transform = transform;
    }
    public override void Enter()
    {
        base.Enter();
        OnAttack();
        Debug.Log("Attack State");
    }
    public override void Execute()
    {
        base.Execute();
        _move.Move(_transform.position.normalized);
    }
}