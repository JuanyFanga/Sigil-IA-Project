using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
public class EnemyFindState : State<StateEnum>
{
    private Transform _lastKnownTransform;
    private Vector3 _lastKnowLocation;
    private IMove _move;
    private ISteering _steering;
    private Transform _entity;
    private float _waitTime = 5f;
    private float _persuitTime = 5f;
    public Action OnwaitOver = delegate{};
    private Vector3 _dir;



    public EnemyFindState(Transform lastKnownLocation, IMove move, Transform entity, ISteering steering)
    {
        _lastKnownTransform = lastKnownLocation;
        _move = move;
        _entity = entity;
        _steering = steering;

    }
    public override void Execute()
    {
        base.Execute();
        if (_waitTime <= 0f || _persuitTime <= 0f)
        {
            OnwaitOver();
        }
        else
        {
            _persuitTime -= Time.deltaTime;
            if (Vector3.Distance(_entity.position, _lastKnowLocation) <= 2f)
            {
                _waitTime -= Time.deltaTime;
            }
            if (Vector3.Distance(_lastKnowLocation, _lastKnownTransform.position) >= 4f && _persuitTime <= 0f)
            {
                _lastKnowLocation = _lastKnownTransform.position;
            }
        }
        _dir = _entity.position - _lastKnownTransform.position;
        _move.Move(_dir);
        
    }
    public override void Enter()
    {
        base.Enter();
        _waitTime = 5f;
        _lastKnowLocation = _lastKnownTransform.position;
        Debug.Log("Find State");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
