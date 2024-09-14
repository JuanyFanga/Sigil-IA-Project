using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State<StateEnum>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemigo entro al estado idle");
    }
}
