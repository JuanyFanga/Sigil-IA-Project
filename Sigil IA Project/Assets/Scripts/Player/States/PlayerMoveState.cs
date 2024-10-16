using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State<StateEnum>
{
    IMove _move;
    FSM<StateEnum> _fsm;
    Transform _camera;

    public PlayerMoveState(FSM<StateEnum> fsm, IMove move, Transform camera)
    {
        _move = move;
        _fsm = fsm;
        _camera = camera;
    }

    public override void FixedExecute()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        if (h == 0 && v == 0)
        {
            _fsm.Transition(StateEnum.Idle);
        }
        else
        {
            _move.Move(dir.normalized);
            _move.Look(dir);
        }
    }
}