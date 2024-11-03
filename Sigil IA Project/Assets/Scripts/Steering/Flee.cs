using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : ISteering
{
    Transform _entity;
    Transform _target;

    //public Flee(Transform entity, Transform target)
    //{
    //    _entity = entity;
    //    Target = target;
    //}

    //public Vector3 GetDir()
    //{
    //    return -base.GetDir();
    //}

    public Flee(Transform target, Transform entity)
    {
        _target = target;
        _entity = entity;
    }


    public Vector3 GetDir()
    {
        // B -A

        return (_entity.position - _target.position).normalized;
    }
}