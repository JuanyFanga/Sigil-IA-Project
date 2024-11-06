using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeadState : State<StateEnum>
{
    IMove _move;

    public PlayerDeadState(IMove move)
    {
        _move = move;
    }

    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector3.zero);
    }
}
