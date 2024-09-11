using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCModel : Entity
{
    [Header("Obstacle Avoidance")]
    public float radius;
    public float angle;
    public float personalArea;
    public float farAwayDistanceFromPlayer;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;
    protected override void Awake()
    {
        base.Awake();
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
    }

    public override void Move(Vector3 dir)
    {
        dir = _obs.GetDir(dir, false);
        dir.y = 0;
        Look(dir);
        base.Move(dir);
    }

    private void OnDrawGizmosSelected()
    {
        Color myColor = Color.cyan;
        myColor.a = 0.5f;
        Gizmos.color = myColor;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, personalArea);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, farAwayDistanceFromPlayer);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }
}
