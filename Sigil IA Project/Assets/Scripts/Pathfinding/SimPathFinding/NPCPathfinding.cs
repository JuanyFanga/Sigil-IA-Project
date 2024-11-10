using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCPathfinding<T> : StatePathfinding<T>
{
    private Transform _player;
    public Action OnScape = delegate { };

    public NPCPathfinding(Transform entity, IMove move,Vector3 target,Transform player, float distanceToPoint = 0.2F) 
        : base(entity,move,target, distanceToPoint)
    {
        _player = player;
    }

    protected override float Heuristic(Vector3 node)
    {
        float h = 0;
        //h += Vector3.Distance(node, _target);
        h += -1 * Vector3.Distance(node, _player.position);
        return h;
    }

    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(_player.position, _entity.position) > 6f)
        {
            OnScape();
        }
    }
    
}

