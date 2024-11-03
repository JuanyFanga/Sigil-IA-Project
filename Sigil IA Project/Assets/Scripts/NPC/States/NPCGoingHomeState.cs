using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCGoingHomeState : State<StateEnum>
{
    private IMove _move;
    private Transform _entity;
    private ISteering _steering;
    private StatePathfinding<StateEnum> _pathfinding;
    private Vector3 _target;

    public NPCGoingHomeState(IMove move, Transform entity,Vector3 safehouse,  StatePathfinding<StateEnum> pathfinding)
    {
        _move = move;
        _entity = entity;
        _target = safehouse;
        _pathfinding = pathfinding;
    }

    public override void Execute()
    {
        base.Execute();
        _pathfinding.Execute();
        //if (Vector3.Distance(_entity.position, Target) < 0.5f) {}
    }

    public override void Enter()
    {
        base.Enter();
        _pathfinding.SetPathAStarPlusVector(_target,_entity.position);
    }
}
