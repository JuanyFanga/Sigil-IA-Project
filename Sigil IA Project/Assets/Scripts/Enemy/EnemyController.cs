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

        var idle = new NPCIdleState();

        idle.AddTransition(StateEnum.Idle, idle);

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
