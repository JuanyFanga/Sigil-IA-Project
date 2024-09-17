using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
public class EnemyFindState : State<StateEnum>
{
    private Vector3 _lastKnownLocation;
    private IMove _move;
    private Transform _entity;
    private float _waitTime = 10f;
    public Action OnwaitOver = delegate{};



    public EnemyFindState(Vector3 lastKnownLocation, IMove move, Transform entity)
    {
        _lastKnownLocation = lastKnownLocation;
        _move = move;
        _entity = entity;
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log(Vector3.Distance(_entity.position, _lastKnownLocation));
        if (Vector3.Distance(_entity.position, _lastKnownLocation) <= 8f)
        {
            _move.Move(Vector3.zero);
            _move.Look(_lastKnownLocation);
            _waitTime -= Time.deltaTime;
        }
        if(_waitTime < 0) 
        {
            OnwaitOver();
            _waitTime = 10;
        } 
    }
    public override void Enter()
    {
        base.Enter();
        //if(_waitTime < 0) { _waitTime = 10f; }
        Debug.Log("FIND FIND FIND");
        _move.Move((_lastKnownLocation - _entity.position).normalized);
    }
}
