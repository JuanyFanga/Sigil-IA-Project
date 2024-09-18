using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;

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
        Collider[] enemies = Physics.OverlapSphere(_entityPos.position, _sphereRadius);
        foreach (IViolentEnemy enemyCollider in enemies)
        {
            Debug.Log("Intento de alertar");
            //IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>;
            //if (enemy != null)
            //{
            enemyCollider.KnowingLastPosition(_entityPos);
            //}
        }
        _move.Move(dir.normalized);
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entro al estado de Escape");
        Collider[] enemies = Physics.OverlapSphere(_entityPos.position, _sphereRadius);
        foreach (IViolentEnemy enemyCollider in enemies)
        {
            Debug.Log("Intento de alertar");
            //IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>;
            //if (enemy != null)
            //{
            enemyCollider.KnowingLastPosition(_entityPos);
            //}
        }
        OnScape();
    }
}
