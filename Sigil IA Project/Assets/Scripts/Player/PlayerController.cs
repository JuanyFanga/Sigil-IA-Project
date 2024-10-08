using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMove _move;
    FSM<StateEnum> _fsm;
    ITreeNode _root;
    Rigidbody _rb;

    private void Awake()
    {
        _move = GetComponent<IMove>();
        _rb = GetComponent<Rigidbody>();
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
        var move = new PlayerMoveState(_fsm, _move, Camera.main.transform);

        idle.AddTransition(StateEnum.Move, move);
        move.AddTransition(StateEnum.Idle, idle);

        _fsm.SetInitial(idle);
    }

    public void InitializeTree()
    {
        var idle = new ActionTree(() => _fsm.Transition(StateEnum.Idle));
        var move = new ActionTree(() => _fsm.Transition(StateEnum.Move));

        var qIsMoving = new QuestionTree(IsMoving, move, idle);
        var qIsExist = new QuestionTree(() => transform.position != null, qIsMoving, idle);

        _root = qIsExist;
    }

    private bool IsMoving()
    {
        //Revisar por qu� esto anda cuando quiere
        return _rb.velocity.magnitude >= 0;
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