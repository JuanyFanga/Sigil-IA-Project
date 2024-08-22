using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState<T> : State<T>
{
    FSM<T> _fsm;
    T _inputToMove;
    IMove _move;

    public PlayerIdleState(FSM<T> fsm, T inputToMove, IMove move)
    {
        _fsm = fsm;
        _inputToMove = inputToMove;
        _move = move;
    }

    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector3.zero);
    }

    public override void Execute()
    {
        base.Execute();

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            _fsm.Transition(_inputToMove);
        }
    }
}
