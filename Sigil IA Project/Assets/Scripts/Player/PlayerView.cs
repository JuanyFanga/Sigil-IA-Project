using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    Animator _anim;
    Rigidbody _rb;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        _anim.SetFloat("Velocity", _rb.velocity.magnitude);
    }
}
