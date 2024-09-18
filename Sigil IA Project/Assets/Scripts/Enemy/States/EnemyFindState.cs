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
    public Action OnwaitOver = delegate{};



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
        Debug.Log(_lastKnowLocation);
        if (_waitTime <= 0f)
        {
            OnwaitOver();
        }

        else
        {
            if (Vector3.Distance(_entity.position, _lastKnowLocation) <= 15f)
            {
                Debug.Log("Close to last position!!");
                _move.Move(Vector3.zero);
                _move.Velocity(Vector3.zero);
                _move.Look(_lastKnowLocation);
                _waitTime -= Time.deltaTime;
            }

            else
            {
                Vector3 dir = _steering.GetDir();
                _move.Move(dir.normalized);
            }
        }
        
        //Debug.Log($"Distancia entre el enemigo y el Last point es:{Vector3.Distance(_entity.position, _lastKnownLocation)} ");

    }
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("FIND FIND FIND");
        _waitTime = 5f;
        _lastKnowLocation = _lastKnownTransform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
