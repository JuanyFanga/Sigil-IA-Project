using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCScapeState : State<StateEnum>
{
    private IMove _move;
    private ISteering _steering;
    private Transform _entityPos;
    private float _sphereRadius;

    public Action OnScape = delegate { };

    public NPCScapeState(IMove move, ISteering steering, Transform entityPos, float SphereRadius)
    {
        _move = move;
        _steering = steering;
        _entityPos = entityPos;
        _sphereRadius = SphereRadius;
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entre a escapar aaaaa");
        Collider[] enemies = Physics.OverlapSphere(_entityPos.position, _sphereRadius);
        foreach (var enemyCollider in enemies)
        {
            IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>();
            if (enemy != null)
            {
                enemy.KnowingLastPosition(_entityPos);
                Debug.Log("Ya envie la ultima localizacion");
            }
            else
            {
                Debug.Log("No pude enviar la ultima localizacion pq no hay nadie cerca");
            }
        }
        OnScape();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
