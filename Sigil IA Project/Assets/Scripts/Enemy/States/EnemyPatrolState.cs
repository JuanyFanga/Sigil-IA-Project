using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State<StateEnum>
{
    private List<Transform> _patrolPoints;
    private IMove _move;
    private Transform _entity;
    public EnemyPatrolState(List<Transform> patrolPoints, IMove move, Transform entity)
    {
        _patrolPoints = patrolPoints;
        _move = move;
        _entity = entity;
    }
    public override void Execute()
    {
        base.Execute();
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Patrol State");
    }
}
