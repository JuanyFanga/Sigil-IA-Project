using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSelector : MonoBehaviour
{
    public static RandomAudioSelector Instance;

    private AudioSource audioSource;
    private int index;

    [SerializeField] private List<AudioClip> audios;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            if (audios.Count > 0)
            {
                index = Random.Range(0, audios.Count);
                audioSource.clip = audios[index];
                audioSource.Play();
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
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
