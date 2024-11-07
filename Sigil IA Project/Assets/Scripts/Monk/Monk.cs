using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : Entity, IBoid
{
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;

    private bool _isAlerted = false;

    public bool IsAlerted
    {
        get { return _isAlerted; }
        set { _isAlerted = value; }
    }
}
