using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeadState : State<StateEnum>
{
    private GameObject _gameObject;

    public NPCDeadState(GameObject gameObject)
    {
        _gameObject = gameObject;
    }

    public override void Enter()
    {
        base.Enter();

        _gameObject.SetActive(false);
    }
}
