using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            LoadLevel.LoadSceneByName(nextLevelName);
        }
    }
}
