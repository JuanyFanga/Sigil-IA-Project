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
    [SerializeField] private float callingSphereRadius;
    private NPCView npcView;
    private FSM<StateEnum> fsm;
    private ITreeNode root;
    private Animator _anim;
    private StatePathfinding<StateEnum> _pathfinding;
    private List<Vector3> pathtoDraw;

    //Bools
    private bool alreadyScaped = false;

    private void Awake()
    {
        npcView = GetComponent<NPCView>();
    }

    private void Start()
    {
        if (LevelManager.Instance != null)
        {
            safeHouse = LevelManager.Instance.SafeZoneTransform;
            target = LevelManager.Instance.PlayerRb;
        }

        InitializedFSM();
        InitializedTree();
    }

    private void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();
        
        _pathfinding = new StatePathfinding<StateEnum>(transform, entityMove,safeHouse.position,StateEnum.GoHome);
        var idle = new NPCIdleState();
        var scape = new NPCScapeState(entityMove, new Evade(transform, target, timePrediction), transform, callingSphereRadius, npcView);
        var goHome = _pathfinding;
        //var goHome = new NPCGoingHomeState(entityMove, transform,safeHouse.position,_pathfinding,callingSphereRadius );
        var dead = new NPCDeadState(gameObject);

        idle.AddTransition(StateEnum.Scape, scape);
        idle.AddTransition(StateEnum.GoHome, goHome);
        scape.AddTransition(StateEnum.GoHome, goHome);
        goHome.AddTransition(StateEnum.Dead, dead);

        scape.OnScape += OnScape;
        _pathfinding.SendList += drawPath;

        fsm = new FSM<StateEnum>(idle);
    }

    private void InitializedTree()
    {
        var idle = new ActionTree(() => fsm.Transition(StateEnum.Idle));
        var scape = new ActionTree(() => fsm.Transition(StateEnum.Scape));
        var goHome = new ActionTree(() => fsm.Transition(StateEnum.GoHome));
        var dead = new ActionTree(() => fsm.Transition(StateEnum.Dead));

        var qIsDead = new QuestionTree(IsCloseToHouse, dead, goHome);
        var qIsScaping = new QuestionTree(HasAlreadyScaped, qIsDead, idle);
        var qInView = new QuestionTree(InView, scape, qIsScaping);
        var qIsExist = new QuestionTree(() => target != null, qInView, idle);

        root = qIsExist;
    }

    private bool InView()
    {
        return los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform) && target.GetComponent<PlayerModel>().IsDetectable == true;
    }

    private bool InProximity()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= maxFarDistance;
    }

    private bool HasAlreadyScaped()
    {
        return alreadyScaped;
    }

    private void OnScape()
    {
        alreadyScaped = true;
    }

    private bool IsCloseToHouse()
    {
        Debug.Log(Vector3.Distance(safeHouse.position, transform.position));
        return Vector3.Distance(safeHouse.position, transform.position) <= 5;
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Se gira hacia quien lo choca, si el LOS funcionase bien deberia detectar que es el player y seguir con el ritmo del arbol
        //pero como eso no pasa tengo que forzar la transicion hacia el escape
        //transform.rotation = Quaternion.LookRotation((collision.transform.position - transform.position).normalized);

        if (collision.gameObject.layer == 3)
        {
            fsm.Transition(StateEnum.Scape);
            //InView();
        }
    }
    
    private void drawPath(List<Vector3> path)
    {
        pathtoDraw = path;
    }
    

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
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
