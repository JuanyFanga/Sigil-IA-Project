using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour, IViolentEnemy
{
    [SerializeField] private Rigidbody _target;
    private LineOfSight _los;
    [SerializeField] private float timePrediction;
    [SerializeField] Transform[] patrolPoints;
    private FSM<StateEnum> fsm;
    private ITreeNode root;
    private ISteering steering;
    private ISteering seekSteering;
    private int indice = 0;
    [SerializeField] private bool patroller;
    private bool IsOverWaitTime = false;
    private bool hasArrived = false;
    [SerializeField] private bool isAlerted = false;
    [SerializeField] private Transform originPoint;
    private Transform newPatrolPosition;
    private Vector3 LastPlayerPosition;
    private Transform _lastPlayerPos;

    private void Awake()
    {
        _lastPlayerPos = _target.transform;
        _los = GetComponentInChildren<LineOfSight>();
    }

    private void Start()
    {
        newPatrolPosition = patrolPoints[0];
        
        InitializeEnemy();
        InitializedFSM();
        InitializedTree();
    }

    private void InitializeEnemy()
    {
        transform.position = originPoint.position;
        transform.rotation = originPoint.rotation;
    }

    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();
        IAttack entityAttack = GetComponent<IAttack>();

        var idle = new EnemyIdleState();
        var patrol = new EnemyPatrolState(entityMove, new Seek(newPatrolPosition, transform), transform, newPatrolPosition);
        var chase = new EnemySteeringState(entityMove,new Pursuit(transform, _target, timePrediction));
        var find = new EnemyFindState(_lastPlayerPos, entityMove, transform,new Seek(_lastPlayerPos, transform));
        var attack = new EnemyAttackState(entityAttack);


        idle.AddTransition(StateEnum.Patrol, patrol);
        idle.AddTransition(StateEnum.Chase, chase);

        patrol.AddTransition(StateEnum.Idle,idle);
        patrol.AddTransition(StateEnum.Chase,chase);
        patrol.AddTransition(StateEnum.Find,find);

        chase.AddTransition(StateEnum.Find,find);
        chase.AddTransition(StateEnum.Attack,attack);

        find.AddTransition(StateEnum.Chase,chase);
        find.AddTransition(StateEnum.Patrol, patrol);

        attack.AddTransition(StateEnum.Chase,chase);
        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Patrol, patrol);
        attack.AddTransition(StateEnum.Find, find);

        patrol.OnArrived += IndiceController;
        find.OnwaitOver += WaitisOver;
        chase.OnEnd += OnEndofChase;

        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => fsm.Transition(StateEnum.Patrol));
        var chase = new ActionTree(() => fsm.Transition(StateEnum.Chase));
        var find = new ActionTree(() => fsm.Transition(StateEnum.Find));
        var attack = new ActionTree(() => fsm.Transition(StateEnum.Attack));


        var qIsPatrol = new QuestionTree(() => patroller, patrol, idle);
        // Soy un Enemigo que patrulla? - Si(Patrulla) -No(Idle)

        var qIsOverFind = new QuestionTree(FindOverCheck, qIsPatrol, find);

        var qIsChase = new QuestionTree(PreviousState, qIsOverFind, qIsPatrol);
        // Lo estaba persiguiendo? - Si(Busca al PJ) - No(Se fija si es patrullante?)

        var qIsInRange = new QuestionTree(InRange, attack, chase);
        // Lo tengo en rango de ataque? - Si(Ataca) - No(Persigue)
        
        var qIsAlerted = new QuestionTree(IsAlerted, find , qIsChase);
        // Esta alertado?

        var qInView = new QuestionTree(InView, qIsInRange, qIsAlerted);
        // Lo estoy viendo? - Si(Se fija si esta a alcance de ataque) - No(Se fija si lo estaba persiguiendo)

        var qIsExist = new QuestionTree(() => _target != null, qInView, idle); 
        // existe el target? - Si(Se fija si lo ve) - No(Se fija si es patrullante)

        root = qIsExist;
    }

    private bool InView()
    {
        if(_los.CheckRange(_target.transform) && _los.CheckAngle(_target.transform) && _los.CheckView(_target.transform)) 
        {
            return true;
        }

        else 
        { 
            return false; 
        }
    }

    private bool InRange()
    {
        return Vector3.Distance(_target.transform.position, transform.position) <= 2f;
    }

    private bool IsAlerted() { return isAlerted; }

    private bool PreviousState()
    {
        if((fsm.PreviousState is EnemySteeringState || fsm.currentState is EnemySteeringState) && !InView()) 
        { 
            return true; 
        }

        else
        {
            return false;
        }
    }

    private void WaitisOver() 
    { 
        IsOverWaitTime = true;
        isAlerted = false;
    }

    private void OnEndofChase() 
    {
        IsOverWaitTime = false;
    }
    private bool FindOverCheck() 
    { 
        if( fsm.PreviousState is EnemySteeringState && IsOverWaitTime == true) {  return true; }
        if (fsm.PreviousState is EnemyFindState && IsOverWaitTime == true) { return true; }
        return false;
    }

    private void OnArrivedToPatrol()
    {
        hasArrived = true;
        if (indice >= patrolPoints.Length){ indice = 0; } else{ indice ++; } 
    }
    private bool HasArrived() { return hasArrived; }

    private void IndiceController()
    {
        if (indice >= patrolPoints.Length - 1) { indice = 0; } else { indice++; }
        newPatrolPosition.position = patrolPoints[indice].position;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }

    public void KnowingLastPosition()
    {
        isAlerted = true;
    }
}
