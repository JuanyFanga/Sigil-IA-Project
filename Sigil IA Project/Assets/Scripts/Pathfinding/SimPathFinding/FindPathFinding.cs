using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathFinding<T> : StatePathfinding<T>
{

    private float _timer = 5f;
    public FindPathFinding(Transform entity, IMove move, Vector3 target, float distanceToPoint = 0.2F)
        : base(entity, move, target, distanceToPoint) { }

    public override void Enter()
    {
        base.Enter();
    }

    protected override void OnFinishPath()
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        base.OnFinishPath();
    }
}