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

        if (_waitTime <= 0f)
        {
            OnwaitOver();
        }

        else
        {
            if (Vector3.Distance(_entity.position, _lastKnownLocation) <= 4f)
            {
                Debug.Log("Close to last position!!");
                _move.Move(Vector3.zero);
                _move.Velocity(Vector3.zero);
                _move.Look(_lastKnownLocation);
                _waitTime -= Time.deltaTime;
            }

            else
            {
                Vector3 dir = _steering.GetDir();
                _move.Move(dir.normalized);
            }
        }
    }
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("FIND FIND FIND");
        _waitTime = 5f;
        Debug.Log(_waitTime);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
