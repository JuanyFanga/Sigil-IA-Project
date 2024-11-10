using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyIdleState<T> : State<T>
{
    public Action OnFinishedIdle = delegate { };
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Idle State");
    }
}
