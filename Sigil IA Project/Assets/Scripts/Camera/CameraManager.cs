using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private Transform _newCameraPosition;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void SetCameraPosition()
    {
        _cam.transform.position = _newCameraPosition.position;
        _cam.transform.rotation = _newCameraPosition.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entrooo");
            SetCameraPosition();
        }
    }
}