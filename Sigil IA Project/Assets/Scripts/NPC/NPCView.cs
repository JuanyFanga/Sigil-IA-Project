using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCView : AnimatedMovement, IPlayFootSteep
{
    [SerializeField] private AudioClip steepFX;
    [SerializeField] private AudioClip screamFX;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayScreamSound()
    {
        audioSource.PlayOneShot(screamFX);
    }

    void IPlayFootSteep.PlayFootStepSound()
    {
        audioSource.PlayOneShot(steepFX);
    }
}
