using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Transform npcSafeHouse;
    [SerializeField] private Rigidbody playerRb;
    public Transform SafeZoneTransform => npcSafeHouse;
    public Rigidbody PlayerRb => playerRb;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
