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
    [SerializeField] private float callingSphereRadius;
    private FSM<StateEnum> fsm;
    private ITreeNode root;

    //Bools
    private bool hasItSeenYou = false;
    private bool alreadyScaped = false;
    private bool isSoClose = false;



    private void Start()
    {
        InitializedFSM();
        InitializedTree();
    }

    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();

        var idle = new NPCIdleState();
        var scape = new NPCScapeState(entityMove, new Evade(transform, target, timePrediction), transform, callingSphereRadius);
        var goHome = new NPCGoingHomeState(entityMove, transform, new Seek(safeHouse, transform));

        idle.AddTransition(StateEnum.Scape, scape);
        idle.AddTransition(StateEnum.GoHome, goHome);
        scape.AddTransition(StateEnum.GoHome, goHome);
        goHome.AddTransition(StateEnum.Scape, scape);

        scape.OnScape += OnScape;

        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var scape = new ActionTree(() => fsm.Transition(StateEnum.Scape));
        var goHome = new ActionTree(() => fsm.Transition(StateEnum.GoHome));

        var qInProximity = new QuestionTree(InProximity, goHome, scape);
        var qIsScaping = new QuestionTree(HasAlreadyScaped, qInProximity, idle);
        var qInView = new QuestionTree(InView, scape, qIsScaping);
       // var qIsClose = new QuestionTree(IsSoClose, qInView, idle);
        var qIsExist = new QuestionTree(() => target != null, qInView, idle);

        root = qIsExist;
    }
    
    private bool InView()
    {
        return los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform);
    }

    private bool InProximity()
    {
        return Vector3.Distance(target.transform.position, transform.position) >= maxFarDistance;
    }

    private bool HasAlreadyScaped()
    {
        return alreadyScaped;
    }

    private void OnScape()
    {
        alreadyScaped = true;
    }

    private bool IsSoClose()
    {
        return isSoClose;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Se gira hacia quien lo choca, si el LOS funcionase bien deberia detectar que es el player y seguir con el ritmo del arbol

        transform.rotation = Quaternion.LookRotation((collision.transform.position - transform.position).normalized);
        if (collision.gameObject.layer == 3)
        {
            fsm.Transition(StateEnum.Scape);
        }
    }
}
