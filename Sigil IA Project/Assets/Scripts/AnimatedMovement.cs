using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMovement : MonoBehaviour
{
    protected Animator _anim;
    protected Rigidbody _rb;

    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        _anim.SetFloat("Velocity", _rb.velocity.magnitude);
    }
}
