using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Idle State");
    }
}
