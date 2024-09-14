using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindState : State<StateEnum>
{
    private Transform _lastKnownLocation;
    private IMove _move;
    private Transform _entity;

    public EnemyFindState(Transform lastKnownLocation, IMove move, Transform entity)
    {
        _lastKnownLocation = lastKnownLocation;
        _move = move;
        _entity = entity;
    }
    public override void Execute()
    {
        base.Execute();
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemigo entro al estado");
    }
}
