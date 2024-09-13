using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoingHomeState : State<StateEnum>
{
    private IMove _move;
    private Transform _entity;
    private ISteering _steering;

    public NPCGoingHomeState(IMove move, Transform entity, ISteering steering)
    {
        _move = move;
        _entity = entity;
        _steering = steering;
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
        Debug.Log("Yendo a la safezone");
    }
}
