using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttackState : State<StateEnum>
{
    IAttack _attack;
    IMove _move;
    Transform _transform;
    Rigidbody _rigidbody;
    public Action OnAttack = delegate { };
    public EnemyAttackState(IAttack attack, IMove move, Transform transform, Rigidbody rb)
    {
        _attack = attack;
        _move = move;
        _transform = transform;
        _rigidbody = rb;
    }
    public override void Enter()
    {
        base.Enter();
        OnAttack();
        _rigidbody.velocity = Vector3.zero;
        Debug.Log("Attack State");
    }
    public override void Execute()
    {
        base.Execute();
    }
}