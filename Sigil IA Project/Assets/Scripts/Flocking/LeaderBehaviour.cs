using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public Transform target;
    public float timePrediction;
    public float multiplier;
    Seek _seek;
    Pursuit _pursuit;
    bool _isSeek;
    private void Awake()
    {
        _seek = new Seek(transform, target);
        _pursuit = new Pursuit(transform, null, timePrediction);
        SetTarget(target);
    }
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        if (target == null) return Vector3.zero;
        if (_isSeek) return _seek.GetDir() * multiplier;
        return _pursuit.GetDir() * multiplier;
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null) return;
        target = newTarget;
        var rb = newTarget.GetComponent<Rigidbody>();
        if (rb)
        {
            _pursuit.Target = rb;
            _isSeek = false;
        }
        else
        {
            _seek.Target = newTarget;
            _isSeek = true;
        }
    }
}
