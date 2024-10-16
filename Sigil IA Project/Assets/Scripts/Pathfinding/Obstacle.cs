using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Start()
    {
        //transform.position = Vector3Int.RoundToInt(transform.position);
        ObstacleManager.Singleton.AddCollider(GetComponent<Collider>());
    }
    private void OnDestroy()
    {
        if (ObstacleManager.Singleton != null)
        {
            ObstacleManager.Singleton.RemoveCollider(GetComponent<Collider>());
        }
    }
}
