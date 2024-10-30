using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : AnimatedMovement, IPlayFootSteep
{
    [SerializeField] private AudioClip steepFX;
    [SerializeField] private AudioClip alertedFX;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAlertedSound()
    {
        audioSource.PlayOneShot(alertedFX);
    }

    void IPlayFootSteep.PlayFootStepSound()
    {
        audioSource.PlayOneShot(steepFX);
    }
}
