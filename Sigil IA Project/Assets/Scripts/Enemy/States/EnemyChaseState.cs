using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State<StateEnum>
{
    IMove _move;
    Transform _entity;
    Transform _target;
    public EnemyChaseState(IMove move, Transform entity, Transform target)
    {
        _move = move;
        _entity = entity;
        _target = target;
    }
    public override void Execute()
    {
        base.Execute();
        Vector3 dirToTarget = _target.position - _entity.position;
        _move.Move(dirToTarget.normalized);
        dirToTarget.y = 0;
        _move.Look(dirToTarget);
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemigo entro al estado");
    }
}
