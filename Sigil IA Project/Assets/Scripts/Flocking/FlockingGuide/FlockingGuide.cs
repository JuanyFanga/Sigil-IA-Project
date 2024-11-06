using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingGuide : Entity
{
    private FSM<StateEnum> fsm;
    private Transform newPatrolPosition;
    private StatePathfinding<StateEnum> _statePathfinding;
    private ITreeNode root;
    [SerializeField] private Rigidbody _target;
    private Animator _anim;
    private int indice = 0;
    [SerializeField] Transform[] patrolPoints;

    private void Start()
    {
        newPatrolPosition = patrolPoints[0];

        InitializeFSM();
        InitializeTree();
    }
    void InitializeFSM()
    {
        IMove entityMove = GetComponent<IMove>();
        _statePathfinding = new StatePathfinding<StateEnum>(transform, entityMove,newPatrolPosition.position,StateEnum.Move);
        var idle = new GenericIdleState<StateEnum>();
        var patrol = new EnemyPatrolState(entityMove, new Seek(newPatrolPosition, transform), transform, patrolPoints);

        idle.AddTransition(StateEnum.Patrol, patrol);
        patrol.AddTransition(StateEnum.Idle, idle);
        

        fsm = new FSM<StateEnum>(patrol);
    }

    void InitializeTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => fsm.Transition(StateEnum.Patrol));

        var qIsExist = new QuestionTree(() => _target != null, patrol, idle);

        root = qIsExist;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }
}
