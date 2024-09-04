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
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
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

    public void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Move));
        var move = new ActionTree(() => fsm.Transition(StateEnum.Idle));

        var qDistance = new QuestionTree(InProximty, move, idle);
        var qInView = new QuestionTree(InView, qDistance, idle);
        var qIsExist = new QuestionTree(() => transform.position != null, qInView, idle);

        root = qIsExist;
    }

    private bool InView()
    {
        return true;
    }

    private bool InProximty()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= 10f; // ACA DESPUES PONEMOS UNA VARIABLE DE LA DISTANCIA. ¿QUE POR QUE NO PUSE LA VARIABLE YO? EH... NO SE, DIJE YA FUE PONGO UN FLOAT ASI NOMAS. BUENO, PORFA NO ME BORREN, SOY UN CONCEPTO TEMPORAL :).
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }
}
