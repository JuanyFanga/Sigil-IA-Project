using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCView : AnimatedMovement, IPlayFootSteep
{
    [SerializeField] private AudioClip steepFX;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    void IPlayFootSteep.PlayFootStepSound()
    {
        audioSource.PlayOneShot(steepFX);
    }
}
