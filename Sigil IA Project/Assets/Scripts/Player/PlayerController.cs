using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMove _move;

    private void Awake()
    {
        _move = GetComponent<IMove>();
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        if (h == 0 && v == 0)
        {
            _move.Move(Vector3.zero);
        }
        else
        {
            _move.Move(dir.normalized);
            _move.Look(dir);
        }
    }
}