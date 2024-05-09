using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRangeEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange = 15f;
    [SerializeField][Range(0f, 100f)] private float shootRange = 10f;

    private int layerMaskTarget;

    protected override void Start()
    {
        base.Start();
        layerMaskTarget = stats.CurrentStat.attackSO.target;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distanceToTarget = DistanceToTarget();
        Vector2 directionToTarget = DirectionToTarget();
        UpdateEnemeyState(distanceToTarget, directionToTarget); // To Check Enemy can attack every frame
    }

    private void UpdateEnemeyState(float distanceToTarget, Vector2 directionToTarget)
    {
        isAttacking = false;
        if (distanceToTarget < followRange)
        {
            CheckIfNear(distanceToTarget, directionToTarget);
        }

    }

    private void CheckIfNear(float distance, Vector2 direction)
    {
        if (distance <= shootRange)
        {
            TryShootAtTarget(direction);
        }
        else
        {
            CallMoveEvent(direction);
        }
    }

    private void TryShootAtTarget(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, layerMaskTarget);

        if (hit.collider != null)
        {
            PerformAttackAction(direction);
        }
        else
        {
            CallMoveEvent(direction);
        }
    }

    private void PerformAttackAction(Vector2 direction) // take action of attacking
    {
        CallLookEvent(direction);   // look direction
        CallMoveEvent(Vector2.zero); // don't move
        isAttacking = true;
    }
}
