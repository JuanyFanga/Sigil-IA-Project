using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySteeringState : State<StateEnum>
{
    IMove _move;
    ISteering _steering;
    private EnemyView _enemyView;
    public Action OnEnd = delegate { };
    public EnemySteeringState(IMove move, ISteering steering, EnemyView enemyView)
    {
        _move = move;
        _steering = steering;
        _enemyView = enemyView;
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);
    }

    public override void Enter()
    {
        base.Enter();
        _enemyView.PlayAlertedSound();
        Debug.Log("Entró al estado de CHASE");
    }
    public override void Exit()
    {
        base.Exit();
        OnEnd();
    }
}