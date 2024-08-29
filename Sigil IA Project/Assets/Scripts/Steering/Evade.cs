using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : ISteering
{
    Transform _entity;
    Rigidbody _target;
    float _timePrediction;

    public Evade(Transform entity, Rigidbody target, float timePrediction)
    {
        _entity = entity;
        _target = target;
        _timePrediction = timePrediction;
    }

    public Vector3 GetDir()
    {
        Vector3 point = _target.position + _target.transform.forward * _target.velocity.magnitude * _timePrediction;
        Vector3 dirEvade = (_entity.position - point).normalized;

        Vector3 dirFlee = (_entity.position - _target.position).normalized;

        //Devuelve qué tipo de ángulo es (si es agudo(-90°) = 1; si es perpendicular (90°) = 0, si es obtuso(+90°) = -1)
        if (Vector3.Dot(dirEvade, dirFlee) < 0)
        {
            return dirFlee;
        }
        else
        {
            return dirEvade;
        }
    }

    public float TimePrediction
    {
        set
        {
            _timePrediction = value;
        }
    }
}
