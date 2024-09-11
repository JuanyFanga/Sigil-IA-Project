using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySteeringState : State<StateEnum>
{
    IMove _move;
    ISteering _steering;
    public EnemySteeringState(IMove move, ISteering steering)
    {
        _move = move;
        _steering = steering;
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);
    }
}