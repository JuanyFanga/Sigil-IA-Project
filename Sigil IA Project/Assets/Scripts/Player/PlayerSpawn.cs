using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] Transform[] spawnPositions;

    float Xvariation = 2f;
    float Zvariation = 2f;

    void Start()
    {
        RandomSpawn();
    }

    private void RandomSpawn()
    {
        if (spawnPositions.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPositions.Length);
            Transform newSpawnPosition = spawnPositions[randomIndex];

            float newXvariation = Random.Range(-Xvariation, Xvariation);
            float newZvariation = Random.Range(-Zvariation, Zvariation);

            Debug.Log($"New variation in X is: {newXvariation}");
            Debug.Log($"New variation in Z is: {newZvariation}");

            transform.position = new Vector3(
                newSpawnPosition.position.x + newXvariation,
                0,
                newSpawnPosition.position.z + newZvariation);
        }
    }
}
