using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody target;
    public LineOfSight los;
    [SerializeField] private float timePrediction;
    private FSM<StateEnum> fsm;
    private ITreeNode root;
    private ISteering steering;
    private ISteering seekSteering;

    private bool hasItSeenYou = false;

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    private void InitializedSteering()
    {
        var pursuit = new Pursuit(transform, target, timePrediction);
        var seek = new Seek(target.transform, transform);
        steering = pursuit;
    }

    public void ChangeSteering(ISteering _steering)
    {
        steering = _steering;
    }

    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();
        IAttack attack1 = GetComponent<IAttack>();

        var idle = new EnemyIdleState();
        var patrol = new EnemyPatrolState(patrolPoints,entityMove,transform);
        var chase = new EnemyChaseState(entityMove,transform,target.transform);
        var find = new EnemyFindState(transform,entityMove, target.transform);
        var attack = new EnemyAttackState(attack1);


        idle.AddTransition(StateEnum.Patrol, patrol);
        idle.AddTransition(StateEnum.Chase, chase);
        patrol.AddTransition(StateEnum.Idle,idle);
        patrol.AddTransition(StateEnum.Chase,chase);
        chase.AddTransition(StateEnum.Find,find);
        chase.AddTransition(StateEnum.Attack,attack);
        find.AddTransition(StateEnum.Chase,chase);
        find.AddTransition(StateEnum.Attack,attack);
        attack.AddTransition(StateEnum.Chase,chase);


        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => fsm.Transition(StateEnum.Patrol));
        var chase = new ActionTree(() => fsm.Transition(StateEnum.Chase));
        var find = new ActionTree(() => fsm.Transition(StateEnum.Find));
        var attack = new ActionTree(() => fsm.Transition(StateEnum.Attack));


        var qIsChasing = new QuestionTree(InProximity, chase, find); 
        var qInView = new QuestionTree(InView, qIsChasing, idle); 
        var qIsPatrol = new QuestionTree(() => patroller, patrol, idle); 
        var qIsExist = new QuestionTree(() => target != null, qInView, qIsPatrol); 
        var qIsInRange = new QuestionTree(InRange, attack, chase);
    }

    private bool InView()
    {
        return los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform);
    }

    private bool InProximity()
    {
        ChangeSteering(seekSteering);
        Debug.Log($"The distance is: {Vector3.Distance(target.transform.position, transform.position)}");
        return Vector3.Distance(target.transform.position, transform.position) >= 5f;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }
}
