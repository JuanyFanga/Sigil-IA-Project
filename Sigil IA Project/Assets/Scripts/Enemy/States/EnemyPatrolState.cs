using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPatrolState : State<StateEnum>
{
    private IMove _move;
    private ISteering _steering;
    private Transform _entity;
    private Transform _target;
    public Action OnArrived = delegate { };
    public EnemyPatrolState(IMove move, ISteering steering, Transform entity, Transform target)
    {
        _move = move;
        _steering = steering;
        _entity = entity;
        _target = target;
    }
    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);
        //Debug.Log(_target.position);
        if (Vector3.Distance(_entity.position, _target.position) <= 0.5f)
        {
            //Debug.Log(_target.position);
            OnArrived();
        }
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemigo entro al estado de patrullajeee");
    }


}
