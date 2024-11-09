using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWaypointsInfo
{
    //Poner el punto incial de spawn
    public Transform _originPoint;

    //Y los puntos de patrulla
    public Transform[] _waypoints;
}

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Rigidbody _player;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float enemyCount;
    [SerializeField] private EnemyWaypointsInfo[] _enemyWaypointsInfos;

    private void Start()
    {
        Dictionary<EnemyWaypointsInfo, float> _spawners = new Dictionary<EnemyWaypointsInfo, float>();

        for (int i = 0; i < _enemyWaypointsInfos.Length; i++)
        {
            _spawners[_enemyWaypointsInfos[i]] = Vector3.Distance(_enemyWaypointsInfos[i]._originPoint.position, _playerTransform.position);
        }

        for (int i = 0; i <= enemyCount; i++)
        {
            EnemyWaypointsInfo newEnemyWaypointsInfo = MyRandoms.Roulette(_spawners);

            if (newEnemyWaypointsInfo._originPoint.childCount == 0)
            {
                if (newEnemyWaypointsInfo._waypoints.Length == 0)
                {
                    Instantiate(_enemies[0], newEnemyWaypointsInfo._originPoint);
                }

                else
                {
                    GameObject newEnemy = Instantiate(_enemies[1], newEnemyWaypointsInfo._originPoint);
                    newEnemy.GetComponent<EnemyController>().InitializeEnemy(newEnemyWaypointsInfo, _player);
                }
            }
        }
    }
}