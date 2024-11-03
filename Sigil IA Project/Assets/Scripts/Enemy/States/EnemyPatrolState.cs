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
    private StatePathfinding<StateEnum> _pathfinding;
    public Action OnArrived = delegate { };
    public EnemyPatrolState(IMove move, ISteering steering, Transform entity, Transform target,StatePathfinding<StateEnum> _statePathfinding)
    {
        _move = move;
        _steering = steering;
        _entity = entity;
        _target = target;
        _pathfinding = _statePathfinding;
    }
    public override void Execute()
    {
        base.Execute();
        _pathfinding.Execute();
        if (Vector3.Distance(_entity.position, _target.position) <= 0.5f)
        {
            OnArrived();
            _pathfinding.SetPathAStarPlusVector(_target.position,_entity.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
        _pathfinding.SetPathAStarPlusVector(_target.position,_entity.position);
    }
}
