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

    private void Start()
    {
        
    }

    void InitializedSteering()
    {
        var flee = new Flee(transform, target.transform);
        var evade = new Evade(transform, target, timePrediction);
        steering = evade;
    }

    public void ChangeSteering(ISteering _steering)
    {
        steering = _steering;
    }

    void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();

        var idle = new NPCIdleState();
        var scape = new NPCScapeState(entityMove, steering);

        idle.AddTransition(StateEnum.Move, scape);
        scape.AddTransition(StateEnum.Idle, idle);

        fsm = new FSM<StateEnum>(idle);
    }

    public void InitializeTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Move));
        var move = new ActionTree(() => fsm.Transition(StateEnum.Idle));

        //var qIsMoving = new QuestionTree(IsMoving, move, idle);
        var qInView = new QuestionTree(InView, move, idle);
        var qIsExist = new QuestionTree(() => transform.position != null, qInView, idle);

        //root = qIsExist;
    }

    private bool InView()
    {
        return true;
    }
}
