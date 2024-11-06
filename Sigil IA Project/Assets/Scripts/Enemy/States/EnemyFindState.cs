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
        if (_persuitTime <= 0f) { OnwaitOver(); }
        else { _persuitTime -= Time.deltaTime; }
        if (_persuitTime == 5f) { _move.Look(Vector3.left); }
        if(_persuitTime == 3f){_move.Look(Vector3.back);}

    }
    public override void Enter()
    {
        base.Enter();
        _persuitTime = 8f;
        Debug.Log("Find State");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
