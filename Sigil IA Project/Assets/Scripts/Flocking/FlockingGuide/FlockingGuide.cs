using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingGuide : Entity
{
    private FSM<StateEnum> fsm;
    private Transform newPatrolPosition;

    private ITreeNode root;
    [SerializeField] private Rigidbody _target;

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

        var idle = new EnemyIdleState();
        var patrol = new EnemyPatrolState(entityMove, new Seek(newPatrolPosition, transform), transform, newPatrolPosition);

        idle.AddTransition(StateEnum.Patrol, patrol);
        patrol.AddTransition(StateEnum.Idle, idle);

        patrol.OnArrived += IndiceController;

        fsm = new FSM<StateEnum>(patrol);
    }

    void InitializeTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => fsm.Transition(StateEnum.Patrol));

        var qIsExist = new QuestionTree(() => _target != null, patrol, idle);

        root = qIsExist;
    }

    private void IndiceController()
    {
        if (indice >= patrolPoints.Length - 1) 
            { indice = 0; } 
        else 
            { indice++; }
        newPatrolPosition.position = patrolPoints[indice].position;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }
}
