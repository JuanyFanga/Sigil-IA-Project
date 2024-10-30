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
    private AudioSource _audioSource;
    private AudioClip _screamFX;
    public Action OnScape = delegate { };

    public NPCScapeState(IMove move, ISteering steering, Transform entityPos, float SphereRadius, AudioSource audioSource, AudioClip screamFX)
    {
        _move = move;
        _steering = steering;
        _entityPos = entityPos;
        _sphereRadius = SphereRadius;
        _audioSource = audioSource;
        _screamFX = screamFX;
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

        Debug.Log("Entro al estado de ESCAPE");

        _audioSource.PlayOneShot(_screamFX);

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
