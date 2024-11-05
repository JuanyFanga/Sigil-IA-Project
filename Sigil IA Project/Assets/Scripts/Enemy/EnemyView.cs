using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : AnimatedMovement, IPlayFootSteep
{
    [SerializeField] private AudioClip steepFX;
    [SerializeField] private AudioClip alertedFX;
    private AudioSource audioSource;

    private EnemyController enemyController;
    protected override void Awake()
    {
        base.Awake();
        enemyController = GetComponent<EnemyController>();
        audioSource = GetComponent<AudioSource>();
        enemyController.OnAttacking += IsAttacking;
    }

    public void PlayAlertedSound()
    {
        audioSource.PlayOneShot(alertedFX);
    }

    void IPlayFootSteep.PlayFootStepSound()
    {
        audioSource.PlayOneShot(steepFX);
    }

    public void IsAttacking()
    {
        _anim.SetTrigger("Attacking");
    }
}
