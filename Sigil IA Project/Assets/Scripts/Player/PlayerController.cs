using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    IMove _move;
    FSM<StateEnum> _fsm;
    ITreeNode _root;

    private void Awake()
    {
        _move = GetComponent<IMove>();
    }

    private void Start()
    {
        InitializeFSM();
        InitializeTree();
    }

    public void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var idle = new PlayerIdleState<StateEnum>(_fsm, StateEnum.Move, _move);
        var move = new PlayerMoveState(_fsm, _move);

        idle.AddTransition(StateEnum.Move, move);

        move.AddTransition(StateEnum.Idle, idle);

        _fsm.SetInitial(idle);
    }

    public void InitializeTree()
    {
        //Actions
        var idle = new ActionTree(() => _fsm.Transition(StateEnum.Move));
        var move = new ActionTree(() => _fsm.Transition(StateEnum.Idle));

        //Questions
        var qIsMoving = new QuestionTree(IsMoving, move, idle);
        var qIsExist = new QuestionTree(() => transform.position != null, qIsMoving, idle);

        _root = qIsExist;
    }

    private bool IsMoving()
    {
        return _move.rb.velocity != Vector3.zero;
    }

    void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    private void FixedUpdate()
    {
        _fsm.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        _fsm.OnLateUpdate();
    }
}