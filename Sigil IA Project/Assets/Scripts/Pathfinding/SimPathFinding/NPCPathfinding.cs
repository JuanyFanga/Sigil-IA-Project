using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class NPCPathfinding<T> : StatePathfinding<T>
{
    public Action OnScape = delegate { };
    private Vector3 _playerpos;
    private Transform _playertransform;

    public NPCPathfinding(Transform entity, IMove move, Vector3 target, Transform player, float distanceToPoint = 0.2F)
        : base(entity, move, target, distanceToPoint)
    {
        _playertransform = player;
        _npcView = npcView;
    }

    protected override float Heuristic(Vector3 node)
    {
        float h = 0;
        h += Vector3.Distance(node, _target);
        float distanceToPlayer = Vector3.Distance(node, _playerpos);
        float proximityCost = 1 / Mathf.Max(distanceToPlayer, 1);
        return h;
    }

    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(_playerpos, _entity.position) > 6f)
        {
            OnScape();
        }
    }

    public override void Enter()
    {
        base.Enter();
        _playerpos = _playertransform.position;
        //Debug.Log(path.Count);
        Debug.Log("Entered NPC Pathfinding");
    }
}

