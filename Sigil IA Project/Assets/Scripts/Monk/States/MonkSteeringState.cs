using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkSteeringState<T> : State<T>
{
    ISteering _steering;
    Monk _monk;
    ObstacleAvoidance _obs;
    public MonkSteeringState(Monk monk, ISteering steering, ObstacleAvoidance obs)
    {
        _monk = monk;
        _steering = steering;
        _obs = obs;
    }
    public override void Execute()
    {
        if (_steering == null) return;
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _monk.Move(dir);
        _monk.Look(dir);
    }
}
