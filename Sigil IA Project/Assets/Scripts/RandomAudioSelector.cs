using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSelector : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audios;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            if (audios.Count > 0)
            {
                audioSource.clip = audios[Random.Range(0, audios.Count)];
                audioSource.Play();
            }
        }
    }
}
