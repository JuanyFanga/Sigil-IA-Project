using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS
{
    public static List<T> Run<T>(T start, Func<T, bool> isSatisfies, Func<T, List<T>> getConnections, int watchdog = 500)
    {
        Dictionary<T, T> parents = new Dictionary<T, T>();
        Stack<T> pending = new Stack<T>();
        HashSet<T> visited = new HashSet<T>();

        pending.Push(start);
        while (pending.Count > 0)
        {
            watchdog--;
            if (watchdog <= 0) break;
            T current = pending.Pop();
            Debug.Log("DFS");
            if (isSatisfies(current))
            {
                //Path
                List<T> path = new List<T> { current };
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
                    pending.Push(child);
                    parents[child] = current;
                }
            }
        }
        return new List<T>();
    }
}