using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using TMPro;

public class EnemyController : MonoBehaviour, IViolentEnemy
{
    //Privates
    private LineOfSight _los;
    private Rigidbody _rb;
    private FSM<StateEnum> fsm;
    private ITreeNode root;
    private ISteering steering;
    private EnemyView enemyView;
    private Animator _anim;
    private Vector3 LastPlayerPosition;
    private Transform _lastPlayerPos;
    private List<Vector3> pathtoDraw;
    private List<Vector3> pathtoDraw2;
    private Transform newPatrolPosition;
    //private StatePathfinding<StateEnum> pathfinding;

    //Serializable
    [SerializeField] private Rigidbody _target;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float timePrediction;
    [SerializeField] private bool patroller;
    [SerializeField] private Transform originPoint;
    [SerializeField] List<Vector3> _patrolPoints = new List<Vector3>();
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private TextMeshPro _text;
    
    //EVENTOS
    public Action OnAttacking = delegate { };
    
    //BOOLEANOS
    private bool isAlerted = false;
    private bool IsOverWaitTime = false;
    private bool hasArrived = false;
    private bool ArrivedtoPatrol = false;
    private bool ArrivedtoFind = false;
    

    private void Awake()
    {
        _los = GetComponentInChildren<LineOfSight>();
        _rb = GetComponent<Rigidbody>();
        enemyView = GetComponent<EnemyView>();
    }

    public void InitializeEnemy(EnemyWaypointsInfo newEnemyWaypontsInfo, Rigidbody player)
    {
        originPoint = newEnemyWaypontsInfo._originPoint;
        patrolPoints = newEnemyWaypontsInfo._waypoints;
        _target = player;
        _lastPlayerPos = _target.transform;

        foreach (Transform point in patrolPoints)
        {
            _patrolPoints.Add(point.position);
        }

        OnAttacking += GameManager.Instance.OnPlayerDie;
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        if (originPoint != null)
        {
            transform.position = originPoint.position;
            transform.rotation = originPoint.rotation;

            newPatrolPosition = patrolPoints[0];
            
            InitializedFSM();
            InitializedTree();
        }
    }
    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();
        IAttack entityAttack = GetComponent<IAttack>();
        
        
        var idle = new EnemyIdleState<StateEnum>();
        var patrol = new PatrolPathFinding<StateEnum>(transform,entityMove, _patrolPoints);
        var chase = new EnemySteeringState(entityMove,new Pursuit(transform, _target, timePrediction), enemyView);
        var find = new FindPathFinding<StateEnum>(transform, entityMove, _lastPlayerPos.position);
        var attack = new EnemyAttackState(entityAttack, entityMove, transform, _rb);
        var pathfinding = new StatePathfinding<StateEnum>(transform, entityMove, _patrolPoints[0]);


        idle.AddTransition(StateEnum.Patrol, patrol);
        idle.AddTransition(StateEnum.Path,pathfinding);
        idle.AddTransition(StateEnum.Chase,chase);

        patrol.AddTransition(StateEnum.Chase,chase);
        patrol.AddTransition(StateEnum.Idle,idle);
        patrol.AddTransition(StateEnum.Path,pathfinding);

        chase.AddTransition(StateEnum.Find,find);
        chase.AddTransition(StateEnum.Attack,attack);
        chase.AddTransition(StateEnum.Path,pathfinding);

        find.AddTransition(StateEnum.Chase,chase);
        find.AddTransition(StateEnum.Patrol, patrol);
        find.AddTransition(StateEnum.Path,pathfinding);
        
        pathfinding.AddTransition(StateEnum.Chase, chase);
        pathfinding.AddTransition(StateEnum.Patrol, patrol);
        pathfinding.AddTransition(StateEnum.Find, find);
        
        
        //find.OnwaitOver += WaitisOver;
        pathfinding.SendList += DrawPath;
        pathfinding.OnArrived += Reached;
        chase.OnEnd += OnEndofChase;
        chase.OnStart += chaseStarted;
        attack.OnAttack += IsAttacking;
        idle.OnFinishedIdle += StartAgain;
        patrol.OnStart += DrawPath2;

        fsm = new FSM<StateEnum>(idle);
    }
    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => fsm.Transition(StateEnum.Patrol));
        var chase = new ActionTree(() => fsm.Transition(StateEnum.Chase));
        var find = new ActionTree(() => fsm.Transition(StateEnum.Find));
        var attack = new ActionTree(() => fsm.Transition(StateEnum.Attack));
        var pathfind = new ActionTree(() => fsm.Transition(StateEnum.Path));


        var qisInSpot = new QuestionTree(()=>ArrivedtoPatrol, patrol, pathfind);
        //Estoy en la zona de patrullaje?
        

        var qIsReachedFind = new QuestionTree(()=> ArrivedtoFind, find, pathfind);
        //llego a la ultima ubicacion del player?
        
        var qIsOverFind = new QuestionTree(chaseCheck,qIsReachedFind, qisInSpot);
        //Sigo en tiempo de busqueda?
        
        var qIsChase = new QuestionTree(PreviousState, qIsOverFind, qisInSpot);
        // Lo estaba persiguiendo? 

        var qIsInRange = new QuestionTree(InRange, attack, chase);
        // Lo tengo en rango de ataque? 
        
        var qIsAlerted = new QuestionTree(IsAlerted, pathfind , qIsOverFind);
        // Esta alertado por NPC?

        var qInView = new QuestionTree(InView, qIsInRange, qIsAlerted);
        // Lo estoy viendo? 

        var qIsExist = new QuestionTree(() => _target != null, qInView, qisInSpot); 
        // existe el target?

        root = qIsExist;
    }
    private bool InView()
    {
        if (_los.CheckRange(_target.transform) && _los.CheckAngle(_target.transform) && _los.CheckView(_target.transform) && _target.GetComponent<PlayerModel>().IsDetectable == true)
        {
            Debug.Log("Lo ve");
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool InRange()
    {
        return Vector3.Distance(_target.transform.position, transform.position) <= _attackRange;
    }

    private bool IsAlerted()
    {
        return isAlerted;
    }
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
        isAlerted = false;
        ArrivedtoPatrol = false;
        ArrivedtoFind = false;
        //pathfinding.Reconfig(patrolPoints[0].position);
    }
    private void OnEndofChase() 
    {
        IsOverWaitTime = false;
        LastPlayerPosition = _target.transform.position;
        //pathfinding.Reconfig(LastPlayerPosition);
    }
    private void StartAgain()
    {
        ArrivedtoPatrol = false;
    }

    private bool chaseCheck()
    {
        if (fsm.currentState is EnemyFindState || fsm.PreviousState is EnemyFindState && !ArrivedtoPatrol)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Reached()
    {
        ArrivedtoPatrol = true;
    }
    public void KnowingLastPosition()
    {
        isAlerted = true;
        //pathfinding.Reconfig(_target.transform.position);
    }
    public void IsAttacking()
    {
        OnAttacking();
    }
    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
        _text.text = fsm.currentState.ToString();
    }
    private void DrawPath(List<Vector3> path)
    {
        pathtoDraw = path;
    }
    private void DrawPath2(List<Vector3> path)
    {
        pathtoDraw = path;
    }
    private void chaseStarted()
    {
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (pathtoDraw != null)
        {
            foreach (var item in pathtoDraw)
            {
                if (item != null)
                {
                    Gizmos.DrawSphere(item, 0.2f);
                }
            }
        }
        Gizmos.color = Color.yellow;
        if (pathtoDraw2 != null)
        {
            foreach (var item in pathtoDraw2)
            {
                if (item != null)
                {
                    Gizmos.DrawSphere(item, 0.2f);
                }
            }
        }
    }
}
