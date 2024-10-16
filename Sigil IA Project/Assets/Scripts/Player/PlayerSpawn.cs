using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] Vector2 spawnZoneX;
    [SerializeField] Vector2 spawnZoneZ;

    void Start()
    {
        transform.position = new Vector3(Random.Range(spawnZoneX.x, spawnZoneX.y), 0, Random.Range(spawnZoneZ.x, spawnZoneZ.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3 (6,0,3));
    }
}
