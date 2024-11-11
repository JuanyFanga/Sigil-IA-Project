using System.Collections;
using System.Collections.Generic;
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

    protected override void OnFinishPath()
    {
        _rb.velocity = Vector3.zero;
        Debug.Log(_timer);
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if (_timer <= 0)
        {
            _timer = 5f;
            Exit();
        }
    }

    public override void Exit()
    {
        OnArrived();
    }
}