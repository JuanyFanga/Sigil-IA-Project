using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public static List<T> Run<T>(T start, Func<T, bool> isSatisfies, Func<T, List<T>> getConnections, Func<T, T, float> getCost, Func<T, float> heuristic, int watchdog = 500)
    {
        Dictionary<T, T> parents = new Dictionary<T, T>();
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, float> cost = new Dictionary<T, float>();

        pending.Enqueue(start, 0);
        cost[start] = 0;
        while (!pending.IsEmpty)
        {
            watchdog--;
            if (watchdog <= 0) break;
            T current = pending.Dequeue();
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
                    var currentCost = cost[current] + getCost(current, child);
                    if (cost.ContainsKey(child) && cost[child] <= currentCost) continue;
                    cost[child] = currentCost;
                    pending.Enqueue(child, currentCost + heuristic(child));
                    parents[child] = current;
                }
            }
        }
        return new List<T>();
    }
}
