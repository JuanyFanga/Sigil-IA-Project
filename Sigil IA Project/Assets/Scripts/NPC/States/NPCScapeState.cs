using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScapeState : State<StateEnum>
{
    private IMove move;
    private ISteering steering;

    public NPCScapeState(IMove move, ISteering steering)
    {
        this.move = move;
        this.steering = steering;
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 dir = steering.GetDir();
        move.Move(dir.normalized);
    }
}
