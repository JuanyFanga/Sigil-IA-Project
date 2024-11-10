using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.Examples;
using UnityEngine;

public class PlayerModel : Entity
{
    private bool _isDetectable = true;

    public bool IsDetectable
    {
        get { return _isDetectable; }
    }


    private float _timeToAlert;
    public float TimerToAlert = 2f;
    DetectionSphere _currentDetector;

    private void Start()
    {
        _isDetectable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DetectionSphere"))
        { 
            _isDetectable = false;
            _timeToAlert = TimerToAlert;
            _currentDetector = other.GetComponent<DetectionSphere>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DetectionSphere"))
        {
            _isDetectable = true;
            _currentDetector = null;
        }
    }

    private void Update()
    {
        //Debug.Log($"Time to alert is: {_timeToAlert}");
        if (_isDetectable == true)
            return;

        _timeToAlert -= Time.deltaTime;
        
        if (_currentDetector != null && _timeToAlert <= 0)
        {
            _currentDetector.AlertMonks();
            _currentDetector.gameObject.SetActive(false);
            _isDetectable = true;
            _timeToAlert = 0;
        }
    }
}
