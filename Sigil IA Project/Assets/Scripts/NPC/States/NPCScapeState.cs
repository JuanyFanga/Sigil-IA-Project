using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;
using System.Collections.Generic;

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
        Debug.Log("Entro al estado de Escape");
        
        Collider[] enemies = Physics.OverlapSphere(_entityPos.position, _sphereRadius);

        //Debug.Log($"Los enemigos collideados fueron {enemies}");

        foreach (Collider enemyCollider in enemies)
        {
            IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>();

            //IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>;
            if (enemy != null)
            {
                Debug.Log($"Los enemigos collideados fueron {enemyCollider.name}");
                enemy.KnowingLastPosition(_entityPos);
            }
        }
        OnScape();
    }
}
