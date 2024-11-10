using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPatrolState : State<StateEnum>
{
    private IMove _move;
    private ISteering _steering;
    private Transform _entity;
    private Transform[] _target;
    private int _index = 0;
    private Vector3 _dir;
    public EnemyPatrolState(IMove move, ISteering steering, Transform entity, Transform[] target)
    {
        _move = move;
        _steering = steering;
        _entity = entity;
        _target = target;
    }
    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(_entity.position, _target[_index].position) <= 1f)
        {
            if(_index < _target.Length - 1){_index++;}else{_index = 0;}
        }
        _dir = _target[_index].position - _entity.position;
        _move.Move(_dir);
    }

    public override void Enter()
    {
        base.Enter();
        _index = 0;
        //Debug.Log("Patrol State");
    }
}
