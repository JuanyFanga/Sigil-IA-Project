using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity
{
    public Rigidbody _rb;
    public float Speed;

    private void Update()
    {
        Debug.Log($"Velocity is {_rb.velocity}");
    }
}
