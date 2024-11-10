using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PatrolPathFinding<T> : StatePathfinding<T>
{
    IMove _move;
    int index = 0;
    public PatrolPathFinding(Transform entity, IMove move, List<Vector3> waypoints, float distanceToPoint = 1f)
        : base(entity, move, waypoints, distanceToPoint)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entro al patrolpathfinding");
    }

    protected override void OnFinishPath()
    {
        index++;
        if (index > _waypoints.Count - 1)
        {
            index = 0;
        }
        Reconfig(_waypoints[index]);
        Debug.Log("Enemigo llego al waypoint: " + index);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Salgo de patrolpathfinding");
    }
}