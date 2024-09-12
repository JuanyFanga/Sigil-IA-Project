using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilSpawn : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Transform _playerTransform;

    public void SetSpawn()
    {
        Dictionary<Transform, float> _spawners = new Dictionary<Transform, float>();

        for (int i = 0; i < _spawnPoints.Count; i++) 
        {
            _spawners[_spawnPoints[i]] = Vector3.Distance(_spawnPoints[i].position, _playerTransform.position);
        }

        MyRandoms.Roulette(_spawners);
    }
}