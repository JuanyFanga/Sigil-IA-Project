using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Rigidbody target;
    public LineOfSight los;
    [SerializeField] private float timePrediction;
    private FSM<StateEnum> fsm;
    private ITreeNode root;
    private ISteering steering;

    private bool hasItSeenYou = false;

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    private void InitializedSteering()
    {
        var flee = new Flee(transform, target.transform);
        var evade = new Evade(transform, target, timePrediction);
        steering = evade;
    }

    public void ChangeSteering(ISteering _steering)
    {
        steering = _steering;
    }

    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();

        var idle = new NPCIdleState();
        var scape = new NPCScapeState(entityMove, steering);

        idle.AddTransition(StateEnum.Scape, scape);
        scape.AddTransition(StateEnum.Idle, idle);

        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var scape = new ActionTree(() => fsm.Transition(StateEnum.Scape));

        var qInView = new QuestionTree(HasItSeenYou, scape, idle);
        var qIsExist = new QuestionTree(() => target != null, qInView, idle);

        root = qIsExist;
    }

    private bool HasItSeenYou()
    {
        return hasItSeenYou;
    }

    //private bool InProximty()
    //{
    //    return Vector3.Distance(target.transform.position, transform.position) <= 10f;
    //}

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
        if (!hasItSeenYou)
        {
            if (los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform))
            {
                hasItSeenYou = true;
            }
        }
    }
}
