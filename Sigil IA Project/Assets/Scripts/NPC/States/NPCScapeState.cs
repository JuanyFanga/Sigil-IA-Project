using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScapeState : State<StateEnum>
{
    private IMove _move;
    private ISteering _steering;

    public NPCScapeState(IMove move, ISteering steering)
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

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entre a escapar aaaa que miedo");
    }
}
