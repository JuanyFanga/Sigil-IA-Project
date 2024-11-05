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
    private StatePathfinding<StateEnum> _pathfinding;
    public Action OnArrived = delegate { };
    private int _index = 0;
    public EnemyPatrolState(IMove move, ISteering steering, Transform entity, Transform[] target,StatePathfinding<StateEnum> _statePathfinding)
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
        if (Vector3.Distance(_entity.position, _target[_index].position) <= 1f)
        {
            if(_index < _target.Length - 1){_index++;}else{_index = 0;}
            _pathfinding.SetPathAStarPlusVector(_target[_index].position,_entity.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
        _index = 0;
        _pathfinding.SetPathAStarPlusVector(_target[_index].position,_entity.position);
    }
}
