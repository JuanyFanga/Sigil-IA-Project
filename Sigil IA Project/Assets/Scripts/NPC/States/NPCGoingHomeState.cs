using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoingHomeState : State<StateEnum>
{
    private IMove _move;
    private Transform _safeHouse;
    private Transform _entity;

    public NPCGoingHomeState(IMove move, Transform safeHouse, Transform entity)
    {
        _move = move;
        _safeHouse = safeHouse;
        _entity = entity;
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _safeHouse.position - _entity.position;
        _move.Move(dir.normalized);
        _move.Look(dir);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Yendo a la safezone");
    }
}
