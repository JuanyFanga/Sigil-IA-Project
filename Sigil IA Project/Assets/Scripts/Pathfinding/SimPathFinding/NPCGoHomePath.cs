using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoHomePath<T> : StatePathfinding<T>
{
    private float _timer = 2f;
    private float _sphereRadius = 1.5f;
    public NPCGoHomePath(Transform entity, IMove move, List<Vector3> waypoints, float distanceToPoint = 1f)
        : base(entity, move, waypoints, distanceToPoint) { }

    public override void Execute()
    {
        base.Execute();
        if (_timer <= 0)
        {
            Detect();
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }

    private void Detect()
    {
        //_npcView.PlayScreamSound();

        Collider[] enemies = Physics.OverlapSphere(_entity.position, _sphereRadius);

        foreach (Collider enemyCollider in enemies)
        {
            IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>();

            if (enemy != null)
            {
                enemy.KnowingLastPosition();
            }
        }
        //OnScape();
    }
}
