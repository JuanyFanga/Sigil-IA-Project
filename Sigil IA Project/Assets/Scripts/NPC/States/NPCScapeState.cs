using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScapeState : State<StateEnum>
{
    private IMove _move;
    private ISteering _steering;
    private Transform _entity;
    private Transform _target;

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
        Debug.Log("Entre a escapar");
        //Aca crear evento para que los enemigos se acerquen
        //Sphere Overlap para alertar a los enemigos de dentro de esa esfera con un evento y de ahi se encarga guara
    }
}
