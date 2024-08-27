using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IMove
{
    public Rigidbody _rb;
    public float Speed;

    Rigidbody IMove.rb { get => _rb;}

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log($"Velocity is {_rb.velocity}");
    }

    public void Move(Vector3 dir)
    {
        dir = dir.normalized;
        dir *= Speed;
        dir.y = _rb.velocity.y;

        _rb.velocity = dir;
    }

    public void Look(Vector3 dir)
    {
        transform.forward = dir;
    }
}
