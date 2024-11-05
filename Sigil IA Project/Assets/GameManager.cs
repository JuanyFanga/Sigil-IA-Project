using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<EnemyController> enemiesController;
    public Action OnLoseGame = delegate { };

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
        foreach (EnemyController controller in enemiesController)
        {
            controller.OnAttacking += OnPlayerDie;
        }
    }

    public void OnPlayerDie()
    {
        OnLoseGame();
    }
}
