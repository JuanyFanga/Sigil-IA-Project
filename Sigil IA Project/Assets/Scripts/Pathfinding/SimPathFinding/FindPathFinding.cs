using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathFinding<T> : StatePathfinding<T>
{
    IMove _move;
    Vector3 _target;
    StateEnum _state;
    public FindPathFinding(Transform entity, IMove move, Vector3 target, float distanceToPoint = 0.2F)
        : base(entity, move, target, distanceToPoint)
    {
        _move = move;
        _target = target;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entro al findpathfinding");
    }
}