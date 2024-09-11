using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCScapeState : State<StateEnum>
{
    private IMove _move;
    private ISteering _steering;
    //private Transform _entity;
    //private Transform _target;

    public Action OnScape = delegate { };

    public NPCScapeState(IMove move, ISteering steering)
    {
        _move = move;
        _steering = steering;
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
        //var lastPos = _entity.position;

        //Crear un overlapp sphere, preguntar
        //por todos los objetos del overlapp, si tienen el
        //script de enemigo o su interfaz se les pase a alguna funcion la ultima
        //ubicacion conocida para que ellos los busquen, charlarlo con guara

        OnScape();
    }

    public override void Exit()
    {
        base.Exit();

    }
}
