using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonkController : MonoBehaviour
{
    public Rigidbody target;
    [SerializeField] private GameObject player;
    public float timePrediction;
    public float angle;
    public float radius;
    [SerializeField] private GameObject flockingGuide;
    public LayerMask maskObs;
    FSM<StateEnum> _fsm;
    Monk _monk;
    private ITreeNode root;
    [SerializeField] private Transform safePlace;
    private StatePathfinding<StateEnum> pathfinding;
    public Action OnGoingHome = delegate { };
    private List<Vector3> pathtoDraw;

    private void Awake()
    {
        _monk = GetComponent<Monk>();
        InitializeFSM();
        InitializedTree();
    }
    void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        IMove entityMove = GetComponent<IMove>();
        var obs = new ObstacleAvoidance(_monk.transform, angle, radius, maskObs);
        //pathfinding = new StatePathfinding<StateEnum>(transform, entityMove, safePlace.position);
        pathfinding = new NPCGoHomePath<StateEnum>(transform, entityMove, safePlace.position);

        var idle = new GenericIdleState<StateEnum>();
        var steering = new MonkSteeringState<StateEnum>(_monk, GetComponent<FlockingManager>(), obs, flockingGuide);
        var alerted = pathfinding;
        var dead = new NPCDeadState(gameObject);

        idle.AddTransition(StateEnum.Move, steering);
        steering.AddTransition(StateEnum.Idle, idle);
        idle.AddTransition(StateEnum.GoHome, alerted);
        steering.AddTransition(StateEnum.GoHome, alerted);
        alerted.AddTransition(StateEnum.Dead, dead);

        pathfinding.SendList += drawPath;
        _fsm.SetInitial(steering);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => _fsm.Transition(StateEnum.Idle));
        var patrol = new ActionTree(() => _fsm.Transition(StateEnum.Patrol));
        var goHome = new ActionTree(() => _fsm.Transition(StateEnum.GoHome));
        var dead = new ActionTree(() => _fsm.Transition(StateEnum.Dead));

        var qIsDead = new QuestionTree(IsCloseToHouse, dead, goHome);

        var qIsAlerted = new QuestionTree(HasBeenAlerted, qIsDead, patrol);
        // Esta alertado por NPC?

        var qIsExist = new QuestionTree(() => target != null, qIsAlerted, idle);
        // existe el target?
        
        root = qIsExist;
    }

    private bool IsCloseToHouse()
    {
        Debug.Log(Vector3.Distance(safePlace.position, transform.position));
        return Vector3.Distance(safePlace.position, transform.position) <= 3;
    }

    private bool HasBeenAlerted()
    {
        return _monk.IsAlerted;
    }

    private void drawPath(List<Vector3> path)
    {
        pathtoDraw = path;
    }

    void Update()
    {
        _fsm.OnUpdate();
        root.Execute();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (pathtoDraw != null)
        {
            foreach (var item in pathtoDraw)
            {
                if (item != null)
                {
                    Gizmos.DrawSphere(item, 0.2f);
                }
                else
                {
                    Debug.Log("Path Empty");
                }
            }
        }
    }
}
