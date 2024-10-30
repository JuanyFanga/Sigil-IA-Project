using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonkController : MonoBehaviour
{
    public Rigidbody target;
    public float timePrediction;
    public float angle;
    public float radius;
    [SerializeField] private GameObject flockingGuide;
    public LayerMask maskObs;
    FSM<StateEnum> _fsm;
    Monk _monk;
    private void Awake()
    {
        _monk = GetComponent<Monk>();
        InitializeFSM();
    }
    void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var obs = new ObstacleAvoidance(_monk.transform, angle, radius, maskObs);

        var idle = new MonkIdleState<StateEnum>();
        var steering = new MonkSteeringState<StateEnum>(_monk, GetComponent<FlockingManager>(), obs, flockingGuide);

        idle.AddTransition(StateEnum.Move, steering);
        steering.AddTransition(StateEnum.Idle, idle);

        _fsm.SetInitial(steering);
    }
    void Update()
    {
        _fsm.OnUpdate();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }
}
