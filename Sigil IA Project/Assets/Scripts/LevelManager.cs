using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Transform npcSafeHouse;
    [SerializeField] private Rigidbody playerRb;
    public Transform SafeZoneTransform => npcSafeHouse;
    public Rigidbody PlayerRb => playerRb;
    [SerializeField] private Transform[] _violentEnemySpawnPoint;

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

    public void Defeat()
    {
        SceneManager.LoadScene("DefeatScreen");
    }

    public Transform SetRandomOrigin()
    {
        return _violentEnemySpawnPoint[Random.Range(0, _violentEnemySpawnPoint.Length)];
    }
}
