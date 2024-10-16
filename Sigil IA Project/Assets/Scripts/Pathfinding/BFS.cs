using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS
{
    public static List<T> Run<T>(T start, Func<T, bool> isSatisfies, Func<T, List<T>> getConnections, int watchdog = 500)
    {
        Dictionary<T, T> parents = new Dictionary<T, T>();
        Queue<T> pending = new Queue<T>();
        HashSet<T> visited = new HashSet<T>();

        pending.Enqueue(start);
        while (pending.Count > 0)
        {
            watchdog--;
            if (watchdog <= 0) break;
            T current = pending.Dequeue();
            Debug.Log("BFS");
            if (isSatisfies(current))
            {
                //Path
                List<T> path = new List<T>();
                path.Add(current);
                while (parents.ContainsKey(path[path.Count - 1]))
                {
                    path.Add(parents[path[path.Count - 1]]);
                }
                path.Reverse();
                return path;
            }
            else
            {
                visited.Add(current);
                List<T> connections = getConnections(current);
                for (int i = 0; i < connections.Count; i++)
                {
                    T child = connections[i];
                    if (visited.Contains(child)) continue;
                    pending.Enqueue(child);
                    parents[child] = current;
                }
            }
        }
        return new List<T>();
    }
}
