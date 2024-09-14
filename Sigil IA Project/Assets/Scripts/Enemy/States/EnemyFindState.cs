using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyFindState : State<StateEnum>
{
    private Transform _lastKnownLocation;
    private IMove _move;
    private Transform _entity;
    private bool _hasReachedLocation = false;
    private float _waitTime = 10f;
    public Action OnwaitOver = delegate();


    public EnemyFindState(Transform lastKnownLocation, IMove move, Transform entity)
    {
        _lastKnownLocation = lastKnownLocation;
        _move = move;
        _entity = entity;
    }
    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(_entity.position, _lastKnownLocation.position) <= 0.5f)
        {
          _waitTime -= _waitTime.deltaTime;
        }
        if(_waitTime <= 0)
        {
            OnwaitOver();
        }
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Find state");
        _move.Move(_lastKnownLocation.position);
    }
}
