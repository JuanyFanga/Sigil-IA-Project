using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IMove
{
    private Rigidbody _rb;
    public float speed;
    public float speedRot = 10;
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
        if (Vector3.Angle(transform.forward, dir) > (Mathf.PI * Mathf.Rad2Deg) / 2)
        {
            transform.forward = dir;
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, speedRot * Time.deltaTime);
        }
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
    
    public void SetPosition(Vector3 pos){}
}