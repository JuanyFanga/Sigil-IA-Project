using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Que esté en la entidad
public class ObstacleAvoidance
{
    Transform _entity;
    float _radius;
    float _angle;
    Collider[] _colls;
    LayerMask _obsMask;
    float _personalArea;

    public ObstacleAvoidance(Transform entity, float radius, float angle, float personalArea, LayerMask obsMask, int countMaxObs = 5)
    {
        _entity = entity;
        _radius = radius;
        //_radius = Mathf.Min(_radius, 1);
        _angle = angle;
        _colls = new Collider[countMaxObs];
        _obsMask = obsMask;
        _personalArea = personalArea;
    }

    public Vector3 GetDir(Vector3 currDir, bool calculateY = true)
    {
        //Se usa el OverlapSphereNonAlloc para que no se cree un array cada vez que se llame al método
        int count = Physics.OverlapSphereNonAlloc(_entity.position, _radius, _colls, _obsMask);

        Collider nearColl = null;
        float nearCollDistance = 0;
        Vector3 nearClosestPoint = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            var currentColl = _colls[i];
            Vector3 closestPoint = currentColl.ClosestPoint(_entity.position);

            if (!calculateY) closestPoint.y = _entity.position.y;

            Vector3 dirToColl = closestPoint - _entity.position;

            float distance = dirToColl.magnitude;
            float currAngle = Vector3.Angle(dirToColl, currDir);

            if (currAngle > _angle / 2) continue;

            //Guardar el collider más cercano
            if (nearColl == null || distance < nearCollDistance)
            {
                nearColl = currentColl;
                nearCollDistance = distance;
                nearClosestPoint = closestPoint;
            }
        }


        if (nearColl == null)
        {
            return currDir;
        }

        //Devuelve un punto en el espacio pero no en el mundo, sino relativo a la entidad (Como si estuviese adentro de la entidad). 
        Vector3 relativePos = _entity.InverseTransformPoint(nearClosestPoint);
        Vector3 dirToClosestPoint = (nearClosestPoint - _entity.position).normalized;
        Vector3 newDir;

        if (relativePos.x < 0)
        {
            //Left
            newDir = Vector3.Cross(_entity.up, dirToClosestPoint);
        }
        else
        {
            //Right
            newDir = -Vector3.Cross(_entity.up, dirToClosestPoint);
        }
        return Vector3.Lerp(currDir, newDir, (_radius - Mathf.Clamp(nearCollDistance - _personalArea, 0, _radius)) / _radius);
    }
}