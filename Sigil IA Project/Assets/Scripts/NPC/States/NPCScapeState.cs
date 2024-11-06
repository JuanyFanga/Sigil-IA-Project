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
    private NPCView _npcView;
    public Action OnScape = delegate { };

    public NPCScapeState(IMove move, ISteering steering, Transform entityPos, float sphereradius, NPCView npcView)
    {
        _move = move;
        _steering = steering;
        _entityPos = entityPos;
        _sphereRadius = sphereradius;
        _npcView = npcView;
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
        
        Debug.Log("NPCScape entered");
        _npcView.PlayScreamSound();

        Collider[] enemies = Physics.OverlapSphere(_entityPos.position, _sphereRadius);

        foreach (Collider enemyCollider in enemies)
        {
            IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>();

            if (enemy != null)
            {
                enemy.KnowingLastPosition();
            }
        }
        OnScape();
    }
}
