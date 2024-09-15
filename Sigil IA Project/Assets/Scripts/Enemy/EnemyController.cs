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
    private bool wasChasing = false;
    private bool hasArrived = false;
    [SerializeField] private bool isAlerted = false;
    [SerializeField] private Transform originPoint;
    private Transform newPatrolPosition;

    private void Awake()
    {
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
        var find = new EnemyFindState(transform,entityMove, _target.transform);
        var attack = new EnemyAttackState(entityAttack);


        idle.AddTransition(StateEnum.Patrol, patrol);
        idle.AddTransition(StateEnum.Chase, chase);

        patrol.AddTransition(StateEnum.Idle,idle);
        patrol.AddTransition(StateEnum.Chase,chase);

        chase.AddTransition(StateEnum.Find,find);
        chase.AddTransition(StateEnum.Attack,attack);
        chase.AddTransition(StateEnum.Patrol,patrol); //esto lo hice para que no siga de largo si me pierde de vista por ahora, cuando funcione todo de CHASE deberia ir a FIND

        find.AddTransition(StateEnum.Chase,chase);
        find.AddTransition(StateEnum.Attack,attack);
        find.AddTransition(StateEnum.Idle,idle);

        attack.AddTransition(StateEnum.Chase,chase);

        patrol.OnArrived += IndiceController;


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

        var qIsChase = new QuestionTree(PreviousState, find, qIsPatrol);
        // Lo estaba persiguiendo? - Si(Busca al PJ) - No(Se fija si es patrullante?)

        var qIsInRange = new QuestionTree(InRange, attack, chase);
        // Lo tengo en rango de ataque? - Si(Ataca) - No(Persigue)

        var qInView = new QuestionTree(InView, qIsInRange, qIsChase);
        // Lo estoy viendo? - Si(Se fija si esta a alcance de ataque) - No(Se fija si lo estaba persiguiendo)

        //var qIsAlerted = new QuestionTree(InAlerted, chase , idle); 
        // Esta alertado?
        
        var qIsExist = new QuestionTree(() => _target != null, qInView, idle); 
        // existe el target? - Si(Se fija si lo ve) - No(Se fija si es patrullante)

        root = qIsExist;
    }

    private bool InView()
    {
        //Debug.Log($"Enemy is trying to see youuu");

        return _los.CheckRange(_target.transform) && _los.CheckAngle(_target.transform) && _los.CheckView(_target.transform);
    }

    private bool InRange()
    {
        //Debug.Log($"The distance is: {Vector3.Distance(_target.transform.position, transform.position)}");
        return Vector3.Distance(_target.transform.position, transform.position) <= 4f;
    }

    private bool InAlerted()
    {
        Debug.Log($"Enemy has been alerted");

        return isAlerted;
    }

    private bool PreviousState() 
    {
        Debug.Log(fsm.PreviousState);
        bool res = fsm.PreviousState is EnemySteeringState;
        Debug.Log(res);
        if (fsm.PreviousState is EnemySteeringState) { return true; }
        else { return false; }
    }

    private void OnArrivedToPatrol()
    {
        hasArrived = true;
        if (indice >= patrolPoints.Length)
        {
            indice = 0;
        }

        else
        {
            indice ++;
        }

        
    }
    private bool HasArrived()
    {
        return hasArrived;
    }

    private void IndiceController()
    {
        if (indice >= patrolPoints.Length - 1)  // Check if it reached the last point
        {
            indice = 0; 
        }
        else
        {
            indice++; 
        }

        //Debug.Log($"El position del patrol point es: {patrolPoints[indice].position}");
        //Debug.Log($"El patrol point es: {patrolPoints[indice]}");
        //Debug.Log($"El indice es {indice}");
        newPatrolPosition.position = patrolPoints[indice].position;

    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }

    public void KnowingLastPosition(Transform lastKnownPosition)
    {
        _target.position = lastKnownPosition.position;
        isAlerted = true;
        //fsm.Transition(StateEnum.Chase)
    }
}
