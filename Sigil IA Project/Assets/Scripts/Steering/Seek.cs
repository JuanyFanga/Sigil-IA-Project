using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;

    public Seek(Transform target, Transform entity)
    {
        _target = target;
        _entity = entity;
    }

    public Vector3 GetDir()
    {
        // B -A

        return (_target.position - _entity.position).normalized;
    }

    public Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }
}