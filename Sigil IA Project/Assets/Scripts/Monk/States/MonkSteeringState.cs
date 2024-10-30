using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkSteeringState<T> : State<T>
{
    ISteering _steering;
    Monk _monk;
    ObstacleAvoidance _obs;
    GameObject _flockingGuide;
    public MonkSteeringState(Monk monk, ISteering steering, ObstacleAvoidance obs, GameObject flockingGuide)
    {
        _monk = monk;
        _steering = steering;
        _obs = obs;
        _flockingGuide = flockingGuide;
    }
    public override void Execute()
    {
        if (_steering == null) return;
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _monk.Move(dir);
        _monk.Look(_flockingGuide.transform);
    }
}
