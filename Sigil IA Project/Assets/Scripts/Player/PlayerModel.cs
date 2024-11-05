using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity
{
    private bool _isDetectable = true;

    public bool IsDetectable
    {
        get { return _isDetectable; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DetectionSphere"))
        { 
            _isDetectable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DetectionSphere"))
        {
            _isDetectable = true;
        }
    }

    private void Update()
    {
        //Debug.Log($"Player is detectable is = {IsDetectable}");
    }

}
