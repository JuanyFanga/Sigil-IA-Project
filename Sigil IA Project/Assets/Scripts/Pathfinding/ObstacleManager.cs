using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public bool skipY = true;
    Dictionary<Vector3, int> _obs = new Dictionary<Vector3, int>();
    static ObstacleManager _instance;
    public static ObstacleManager Singleton
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObstacleManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    public void AddCollider(Collider coll)
    {
        var points = GetPointsInCollider(coll, skipY);
        for (int i = 0; i < points.Count; i++)
        {
            if (_obs.ContainsKey(points[i]))
            {
                _obs[points[i]] += 1;
            }
            else
            {
                _obs[points[i]] = 1;
            }
        }
    }
    public void RemoveCollider(Collider coll)
    {
        var points = GetPointsInCollider(coll, skipY);
        for (int i = 0; i < points.Count; i++)
        {
            if (_obs.ContainsKey(points[i]))
            {
                _obs[points[i]] -= 1;
                if (_obs[points[i]] <= 0)
                {
                    _obs.Remove(points[i]);
                }
            }
        }
    }
    public bool IsRightPos(Vector3 point)
    {
        //print(point);
        //print(!_obs.ContainsKey(point));
        return !_obs.ContainsKey(point);
    }
    List<Vector3> GetPointsInCollider(Collider coll, bool skipY = true)
    {
        List<Vector3> points = new List<Vector3>();
        Bounds bounds = coll.bounds;

        int minX = Mathf.FloorToInt(bounds.min.x);
        int maxX = Mathf.CeilToInt(bounds.max.x);

        int minY = skipY ? 0 : Mathf.FloorToInt(bounds.min.y);
        int maxY = skipY ? 0 : Mathf.CeilToInt(bounds.max.y);

        int minZ = Mathf.FloorToInt(bounds.min.z);
        int maxZ = Mathf.CeilToInt(bounds.max.z);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    Vector3 point = new Vector3(x, y, z);
                    if (bounds.Contains(point))
                    {
                        points.Add(point);
                    }
                }
            }
        }
        return points;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var item in _obs)
        {
            Gizmos.DrawSphere(item.Key, 0.2f);
        }
    }
}