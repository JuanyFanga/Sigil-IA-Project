using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : Entity
{
    [Header("Obstacle Avoidance")]
    public float radius;
    public float angle;
    public float personalArea;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;

    IAttack _attack;
    protected override void Awake()
    {
        base.Awake();
        _obs = new ObstacleAvoidance(transform, radius, angle, obsMask, personalArea);
    }

    public override void Move(Vector3 dir)
    {
        dir = _obs.GetDir(dir, false);
        dir.y = 0;
        Look(dir);
        base.Move(dir);
    }

    public void Attack()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Color myColor = Color.cyan;
        myColor.a = 0.5f;
        Gizmos.color = myColor;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, personalArea);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }
}
