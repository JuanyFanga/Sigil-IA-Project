using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            audioSource.Play();
            LoadLevel.LoadSceneByName(nextLevelName);
        }
    }
}
