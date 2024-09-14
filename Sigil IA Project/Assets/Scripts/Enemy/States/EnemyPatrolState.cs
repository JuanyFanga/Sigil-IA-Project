using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State<StateEnum>
{
    private Transform _patrolPoints;
    private IMove _move;
    private Transform _entity;
    public EnemyPatrolState(Transform patrolPoints, IMove move, Transform entity)
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
        Debug.Log("Enemigo entro al estado");
        _move.Move(_patrolPoints.position);
        if(Vector3.Distance(_entity.position , _patrolPoints.position) < 2f)
        {
            //mover a estado Idle con Timer
        }

    }
}
