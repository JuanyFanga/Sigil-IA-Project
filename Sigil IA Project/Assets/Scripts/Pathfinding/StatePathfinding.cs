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
    private List<Vector3> path;
    private StateEnum _state;
    private float _sphereRadius = 10f;
    private float _timer;
    
    
    public StatePathfinding(Transform entity, IMove move,Vector3 target,StateEnum state, float distanceToPoint = 0.2F) 
        : base(entity, distanceToPoint)
    {
        _move = move;
        _target= target;
        _state = state;
    }
    public StatePathfinding(Transform entity, IMove move, List<Vector3> waypoints, float distanceToPoint = 0.2f)
        : base(entity, waypoints, distanceToPoint)
    {
        _move = move;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Pathfinding");
        SetPathAStarPlusVector();
        SendList(path);
        _timer = 2f;
    }

    protected override void OnFinishPath()
    {
        base.OnFinishPath();
        OnArrived();
    }
    protected override void OnMove(Vector3 dir)
    {
        base.OnMove(dir);
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
        var start = GetPoint(_entity.position);
        path = Astar.Run<Vector3>(start,IsSatisfies,GetConnections,GetCost,Heuristic);
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
    float Heuristic(Vector3 node)
    {
        float h = 0;
        h += Vector3.Distance(node, _target);
        return h;
    }
    Vector3 GetPoint(Vector3 point)
    {
        return Vector3Int.RoundToInt(point);
    }
    bool IsSatisfies(Vector3 current)
    {
        var pointToGoal = GetPoint(_target);
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

    public virtual void Reconfig(Vector3 target)
    {
        _target = target;
    }
    public override void Execute()
    {
        base.Execute();
        if (_state == StateEnum.GoHome)
        {
            if (_timer <= 0)
            {
                Detect();
                _timer = 2f;
            }
            else
            {
                _timer -= Time.deltaTime;
            }
            
        }
    }
    private void Detect()
    {
        Collider[] enemies = Physics.OverlapSphere(_entity.position, _sphereRadius);
        Debug.Log("Alerting");
        foreach (Collider enemyCollider in enemies)
        {
            IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>();

            if (enemy != null)
            {
                enemy.KnowingLastPosition();
            }
        }
    }
}

