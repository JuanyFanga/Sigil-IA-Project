using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PatrolPathFinding<T> : StatePathfinding<T>
{
    public PatrolPathFinding(Transform entity, IMove move, List<Vector3> waypoints, float distanceToPoint = 1f)
        : base(entity, move, waypoints, distanceToPoint) { }

    public override void Enter()
    {
        SetWaypoints(_waypoints);
        OnStart(_waypoints);
    }
    

    public override void Exit()
    {
    }

    protected override void MovethePlayer()
    {
        Vector3 point = _waypoints[_index];
        point.y = _entity.position.y; 
        Vector3 dir = point - _entity.position;
        if (dir.magnitude < _distanceToPoint)
        {
            if (_index + 1 < _waypoints.Count)
                _index++;
            else
            {
                _index = 0;
            }
        }
        OnMove(dir.normalized);
    }
}