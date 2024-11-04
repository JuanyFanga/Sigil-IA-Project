using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCGoingHomeState : State<StateEnum>
{
    private IMove _move;
    private Transform _entity;
    private ISteering _steering;
    private StatePathfinding<StateEnum> _pathfinding;
    private Vector3 _target;
    private float _timer;
    private float _sphereRadius;

    public NPCGoingHomeState(IMove move, Transform entity,Vector3 safehouse,  StatePathfinding<StateEnum> pathfinding,float SphereRadius)
    {
        _move = move;
        _entity = entity;
        _target = safehouse;
        _pathfinding = pathfinding;
        _sphereRadius = SphereRadius;
    }

    public override void Execute()
    {
        base.Execute();
        _pathfinding.Execute();
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Detect();
            _timer = 2f;
        }
    }

    public override void Enter()
    {
        base.Enter();
        _timer = 2f;
        _pathfinding.SetPathAStarPlusVector(_target,_entity.position);
    }


    private void Detect()
    {
        Collider[] enemies = Physics.OverlapSphere(_entity.position, _sphereRadius);

        foreach (Collider enemyCollider in enemies)
        {
            IViolentEnemy enemy = enemyCollider.GetComponent<IViolentEnemy>();

            if (enemy != null)
            {
                enemy.KnowingLastPosition();
            }
        }
    }
}
