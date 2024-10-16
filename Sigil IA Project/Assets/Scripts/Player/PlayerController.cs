using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMove _move;
    FSM<StateEnum> _fsm;

    private void Awake()
    {
        _move = GetComponent<IMove>();
    }

    private void Start()
    {
        InitializeFSM();
    }

    public void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var idle = new PlayerIdleState<StateEnum>(_fsm, StateEnum.Move, _move);
        var move = new PlayerMoveState(_fsm, _move, Camera.main.transform);

        idle.AddTransition(StateEnum.Move, move);
        move.AddTransition(StateEnum.Idle, idle);

        _fsm.SetInitial(idle);
    }

    void Update()
    {
        _fsm.OnUpdate();
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