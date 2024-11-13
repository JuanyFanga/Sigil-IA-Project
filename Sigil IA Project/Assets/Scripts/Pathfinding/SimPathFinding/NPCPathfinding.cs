using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class NPCPathfinding<T> : StatePathfinding<T>
{
    public Action OnScape = delegate { };
    public Action OnScream = delegate { };
    private Vector3 _playerpos;
    private Transform _playertransform;

    public NPCPathfinding(Transform entity, IMove move, Vector3 target, Transform player, float distanceToPoint = 0.2F)
        : base(entity, move, target, distanceToPoint)
    {
        _playertransform = player;
    }

    protected override float Heuristic(Vector3 node)
    {
        //Obtenemos el c�lculo de la heur�stica original
        float h = base.Heuristic(node);

        // Tener una distancia m�xima
        // Diferencia entre distancia actual menos la distancia m�xima
        // Que no se supere la distancia m�xima
        // Clampear de un m�nimo a un m�ximo
        // Math.Clamp(distance, 0, maxDistance) - maxDistance;

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
        OnScream();
        //Debug.Log(path.Count);
        //Debug.Log("Entered NPC Pathfinding");
    }
}

