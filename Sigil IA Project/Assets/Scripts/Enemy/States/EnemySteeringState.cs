using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySteeringState : State<StateEnum>
{
    IMove _move;
    ISteering _steering;
    private AudioSource _audioSource;
    private AudioClip _alertedFX;
    public Action OnEnd = delegate { };
    public EnemySteeringState(IMove move, ISteering steering, AudioSource audioSource, AudioClip alertedFX)
    {
        _move = move;
        _steering = steering;
        _audioSource = audioSource;
        _alertedFX = alertedFX;
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
        Debug.Log("Entró al estado de CHASE");
        _audioSource.PlayOneShot(_alertedFX);
    }
    public override void Exit()
    {
        base.Exit();
        OnEnd();
    }
}