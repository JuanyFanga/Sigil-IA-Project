using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool hasBeenSeen;

    float cooldown = 2;

    private bool hasItSeenYou = false;

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    private void InitializedSteering()
    {
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



        var qInProximityAfterSeeing = new QuestionTree(InProximityAfterSeeing, goHome, idle);

        var qInView = new QuestionTree(InView, scape, qInProximityAfterSeeing);
   
        var qIsClose = new QuestionTree(InProximity, qInView, qInProximityAfterSeeing);
        
        var qIsExist = new QuestionTree(() => target != null, qIsClose, idle);

        root = qIsExist;
    }
    
    private bool InView()
    {
        if (los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform))
        {
            hasBeenSeen = true;
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool InProximity()
    {
        //return true;
        return Vector3.Distance(transform.position, target.position) <= 5;
        //return isInProximity;
    }

    private bool InProximityAfterSeeing()
    {
        if (hasBeenSeen)
        {
            return Vector3.Distance(transform.position, target.position) <= 5;
        }
        else
        { 
            return false; 
        }
        //return isInProximity;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();

        //if (Vector3.Distance(target.transform.position, transform.position) >= 5f)
        //{
        //    isInProximity = true;
        //}

        ////Debug.Log($"The distance is: {Vector3.Distance(target.transform.position, transform.position)}");
        //Debug.Log($"In proximity: {isInProximity}");
    }
}
