using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : State<StateEnum>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entre al estado");
    }


}
