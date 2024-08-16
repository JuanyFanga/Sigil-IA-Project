using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float range;
    public float angle;
    public Transform reference;
    public LayerMask obsMask;

    public bool CheckRange(Transform target)
    {
        float distanceToTarget = Vector3.Distance(target.position, Origin);

        return distanceToTarget <= range;
    }

    public bool CheckAngle(Transform target)
    {
        Vector3 dirToTarget = target.position - Origin;

        float angleToTarget = Vector3.Angle(dirToTarget, Forward);

        //Se divide por 2 para que haya 45° de un lado y 45° del otro
        return angleToTarget <= angle / 2;
    }

    public bool CheckView(Transform target)
    {
        Vector3 dirToTarget = target.position - Origin;

        return !Physics.Raycast(Origin, dirToTarget.normalized, dirToTarget.magnitude, obsMask);
    }

    Vector3 Origin
    {
        get
        {
            if (reference == null)
            {
                return transform.position;
            }
            else
            {
                return reference.position;
            }
        }
    }

    Vector3 Forward
    {
        get
        {
            if (reference == null)
            {
                return transform.forward;
            }
            else
            {
                return reference.forward;
            }
        }
    }

    //Solo vemos los gizmos del objeto seleccionado
    private void OnDrawGizmosSelected()
    {
        Color myColor = Color.blue; ;
        myColor.a = 0.3f;

        Gizmos.color = myColor;
        Gizmos.DrawSphere(Origin, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -angle / 2, 0) * Forward * range);
    }
}
