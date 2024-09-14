using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody target;
    public LineOfSight los;
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
    

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    private void InitializedSteering()
    {
        var pursuit = new Pursuit(transform, target, timePrediction);
        var seek = new Seek(patrolPoints[indice], transform);
        steering = seek;
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
        var patrol = new EnemyPatrolState(entityMove, new Seek(patrolPoints[indice], transform), transform, patrolPoints[indice]);
        var chase = new EnemySteeringState(entityMove,new Pursuit(transform, target, timePrediction));
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
        find.AddTransition(StateEnum.Idle,idle);

        attack.AddTransition(StateEnum.Chase,chase);

        patrol.OnArrived += OnArrivedToPatrol;


        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => fsm.Transition(StateEnum.Patrol));
        var chase = new ActionTree(() => fsm.Transition(StateEnum.Chase));
        var find = new ActionTree(() => fsm.Transition(StateEnum.Find));
        var attack = new ActionTree(() => fsm.Transition(StateEnum.Attack));


        var qIsPatrol = new QuestionTree(() => patroller, patrol, idle); // Soy un Enemigo que patrulla? - Si(Patrulla) -No(Idle)
        var qIsInRange = new QuestionTree(InRange, attack, chase); // Lo tengo en rango de ataque? - Si(Ataca) - No(Persigue)
        var qIsChase = new QuestionTree(() => wasChasing, find , qIsPatrol); // Lo estaba persiguiendo? - Si(Busca al PJ) - No(Se fija si es patrullante?)
        var qInView = new QuestionTree(InView, qIsInRange, qIsChase);// Lo estoy viendo? - Si(Se fija si esta a alcance de ataque) - No(Se fija si lo estaba persiguiendo)
        var qIsExist = new QuestionTree(() => target != null, qInView, qIsPatrol); // existe el target? - Si(Se fija si lo ve) - No(Se fija si es patrullante)
    }

    private bool InView()
    {
        return los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform);
    }

    private bool InRange()
    {
        ChangeSteering(seekSteering);
        Debug.Log($"The distance is: {Vector3.Distance(target.transform.position, transform.position)}");
        return Vector3.Distance(target.transform.position, transform.position) >= 2f;
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
        if(Vector3.Distance(transform.position , patrolPoints[indice].position) < 2f)
        {
            if(indice == patrolPoints.Length){
                indice = 0;
            }else{
                indice ++;
            }
        }
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }
}
