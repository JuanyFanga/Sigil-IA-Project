using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
public class EnemyFindState : State<StateEnum>
{
    private Vector3 _lastKnownLocation;
    private IMove _move;
    private ISteering _steering;
    private Transform _entity;
    private float _waitTime = 5f;
    public Action OnwaitOver = delegate{};



    public EnemyFindState(Vector3 lastKnownLocation, IMove move, Transform entity, ISteering steering)
    {
        _lastKnownLocation = lastKnownLocation;
        _move = move;
        _entity = entity;
        _steering = steering;
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log(_waitTime);
        if (Vector3.Distance(_entity.position, _lastKnownLocation) <= 8f)
        {
            _move.Move(Vector3.zero);
            _move.Look(_lastKnownLocation);
            _waitTime -= Time.deltaTime;
        }
        else
        {
            Vector3 dir = _steering.GetDir();
            _move.Move(dir.normalized);
        }
        if(_waitTime <= 0.9f) 
        {
            OnwaitOver();
        } 
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("FIND FIND FIND");
        _waitTime = 5f;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
