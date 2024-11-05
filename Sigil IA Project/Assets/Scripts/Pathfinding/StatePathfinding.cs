using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePathfinding<T> : StateFollowPoints<T>
{
    
    IMove _move;
    Animator _anim;
    public Transform Target;
    private Transform _current;
    
    
    public StatePathfinding(Transform entity, IMove move, Animator anim, float distanceToPoint = 0.2F) 
        : base(entity, distanceToPoint)
    {
        _move = move;
        _anim = anim;
    }
    public StatePathfinding(Transform entity, IMove move, Animator anim, List<Vector3> waypoints, float distanceToPoint = 0.2f)
        : base(entity, waypoints, distanceToPoint)
    {
        _move = move;
        _anim = anim;
    }
    
    protected override void OnMove(Vector3 dir)
    {
        base.OnMove(dir);
        if (dir != Vector3.zero)
        {
            _move.Move(dir);
            _move.Look(dir);
        }
        else
        {
            //Debug.Log("OnMove skipped as direction vector is zero.");
        }
    }
    protected override void OnStartPath()
    {
        base.OnStartPath();
        _move.SetPosition(_waypoints[0]);
        //_anim.SetFloat("Vel", 1);
    }
    protected override void OnFinishPath()
    {
        base.OnFinishPath();
        //_anim.SetFloat("Vel", 0);
    }
    
    public void SetPathAStarPlusVector(Vector3 posTarget, Vector3 start)
    {
        List<Vector3> path = Astar.Run<Vector3>(start,(x)=>IsSatisfies(x,posTarget),GetConnections,GetCost,(y)=>Heuristic(y,posTarget));
        if (path.Count <= 0)
        {
            Debug.Log("No Path");
            return;
        }
        SetWaypoints(path);
    }

    public void Execute()
    {
        base.Execute();
    }
    
    
    float GetCost(Vector3 parent, Vector3 child)
    {
        float multiplierDist = 1;
        float cost = 0;
        cost += Vector3.Distance(parent, child) * multiplierDist;
        return cost;
    }

    float Heuristic(Vector3 node, Vector3 targetPos)
    {
        float h = 0;
        h += Vector3.Distance(node, targetPos);
        return h;
    }
    Vector3 GetPoint(Vector3 point)
    {
        return Vector3Int.RoundToInt(point);
    }

    bool IsSatisfies(Vector3 current, Vector3 targetPos)
    {
        var pointToGoal = GetPoint(targetPos);
        return Vector3.Distance(current,pointToGoal) <= 1f;
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
}

