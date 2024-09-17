using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IMove
{
    private Rigidbody _rb;
    public float speed;
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public virtual void Move(Vector3 dir)
    {
        dir = dir.normalized;
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }
    public void Look(Vector3 dir)
    {
        transform.forward = dir;
    }
    public void Look(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        Look(dir);
    }

    public void Velocity(Vector3 dir)
    {
        _rb.velocity = dir;
    }
}