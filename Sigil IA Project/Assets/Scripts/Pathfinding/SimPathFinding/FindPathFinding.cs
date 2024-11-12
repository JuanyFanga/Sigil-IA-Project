using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FindPathFinding<T> : StatePathfinding<T>
{

    private float _timer = 5f;
    private Transform _playerT;
    private Rigidbody _rb;

    public FindPathFinding(Transform entity, IMove move, Vector3 target, Transform player,Rigidbody rb, float distanceToPoint = 0.2F)
        : base(entity, move, target, distanceToPoint)
    {
        _playerT = player;
        _rb = rb;
    }

    public override void Enter()
    {
        _target = _playerT.position;
        base.Enter();
        _timer = 5f;
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
                _rb.velocity = Vector3.zero;
                Debug.Log(_timer);
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                }

                if (_timer <= 0)
                {
                    _timer = 5f;
                    Exit();
                }
            }
        }
        if(_timer == 5) { OnMove(dir.normalized); }
        
    }

    public override void Exit()
    {
        OnArrived();
    }
}