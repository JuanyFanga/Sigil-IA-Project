using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatePathfinding<T> : StateFollowPoints<T>
{
    
    IMove _move;
    public Vector3 _target;
    private Transform _current;
    public Action OnArrived = delegate { };
    public Action<List<Vector3>> SendList = delegate { };
    protected private List<Vector3> path;
    private StateEnum _state;
    
    
    public StatePathfinding(Transform entity, IMove move,Vector3 target, float distanceToPoint = 0.2F) 
        : base(entity, distanceToPoint)
    {
        _move = move;
        _target= target;
    }
    public StatePathfinding(Transform entity, IMove move, List<Vector3> waypoints, float distanceToPoint = 0.2f)
        : base(entity, waypoints, distanceToPoint)
    {
        _move = move;
    }

    public override void Enter()
    {
        SetPathAStarPlusVector();
        SendList(path);
    }

    protected override void OnFinishPath()
    {
        base.OnFinishPath();
        OnArrived();
    }
    protected override void OnMove(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            _move.Move(dir);
            _move.Look(dir);
        }
    }
    protected override void OnStartPath()
    {
        base.OnStartPath();
        _move.SetPosition(_waypoints[0]);
    }
    public void SetPathAStarPlusVector()
    {
        //var start = GetPoint(_entity.position);
        path = Astar.Run<Vector3>(_entity.position, IsSatisfies,GetConnections,GetCost,Heuristic);
        if (path.Count <= 0)
        {
            Debug.Log("No Path");
            return;
        }
        SetWaypoints(path);
    }
    float GetCost(Vector3 parent, Vector3 child)
    {
        float multiplierDist = 1;
        float cost = 0;
        cost += Vector3.Distance(parent, child) * multiplierDist;
        return cost;
    }
    protected virtual float Heuristic(Vector3 node)
    {
        float h = 0;
        h += Vector3.Distance(node, _target);
        return h;
    }
    //Vector3 GetPoint(Vector3 point)
    //{
    //    return Vector3Int.RoundToInt(point);
    //}
    bool IsSatisfies(Vector3 current)
    {
        //var pointToGoal = GetPoint(_target);
        return Vector3.Distance(current, _entity.position) <= 1f;
    }
    List<Vector3> GetConnections(Vector3 current)
    {
        List<Vector3> connections = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if(x == 0 && z == 0) continue;
                var point = new Vector3(current.x + x, current.y, current.z + z);
                if (!ObstacleManager.Singleton.IsRightPos(point)) continue;
                connections.Add(point);
            }
        }
        return connections;
    }

    public void Reconfig(Vector3 target)
    {
        _target = target;
    }
}

