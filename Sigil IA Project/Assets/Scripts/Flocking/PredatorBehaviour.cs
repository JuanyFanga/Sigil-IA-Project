using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier = 1;
    public int maxPredators;
    public float rangePredator;
    public LayerMask maskPredator;
    Collider[] _colls;
    private void Awake()
    {
        _colls = new Collider[maxPredators];
    }
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 predatorDir = Vector3.zero;
        int count = Physics.OverlapSphereNonAlloc(self.Position, rangePredator, _colls, maskPredator);
        for (int i = 0; i < count; i++)
        {
            Vector3 diff = self.Position - _colls[i].ClosestPoint(self.Position);
            float distance = diff.magnitude;
            predatorDir += diff.normalized * (rangePredator - distance);
        }
        return predatorDir.normalized * multiplier;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, rangePredator);
    }
}