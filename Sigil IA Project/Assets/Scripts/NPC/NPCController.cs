using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Rigidbody target;
    public LineOfSight los;
    [SerializeField] private float timePrediction;
    [SerializeField] private float maxFarDistance;
    [SerializeField] private Transform safeHouse;
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
        seekSteering = new Seek(transform, safeHouse);
        var evade = new Evade(transform, target, timePrediction);
        steering = evade;
    }

    public void ChangeSteering(ISteering _steering)
    {
        steering = _steering;
        Debug.Log(steering);
    }

    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();

        var idle = new NPCIdleState();
        var scape = new NPCScapeState(entityMove, steering);
        var goHome = new NPCGoingHomeState(entityMove, safeHouse, transform);

        idle.AddTransition(StateEnum.Scape, scape);
        idle.AddTransition(StateEnum.GoHome, goHome);
        scape.AddTransition(StateEnum.GoHome, goHome);
        goHome.AddTransition(StateEnum.Scape, scape);

        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var scape = new ActionTree(() => fsm.Transition(StateEnum.Scape));
        var goHome = new ActionTree(() => fsm.Transition(StateEnum.GoHome));

        var qIsScaping = new QuestionTree(InProximity, goHome, scape);
        var qInView = new QuestionTree(InView, qIsScaping, idle);
        var qIsExist = new QuestionTree(() => target != null, qInView, idle);

        root = qIsExist;
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
