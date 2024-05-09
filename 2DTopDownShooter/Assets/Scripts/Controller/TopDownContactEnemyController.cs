﻿using System;
using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 1000f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    private HealthSystem healthSystem;
    private HealthSystem collidingTargetHealthSystem;
    private TopDownMovement collidingMovement;


    protected override void Start()
    {
        base.Start();

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;

    }

    private void OnDamage()
    {
        followRange = 100f;
    }

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();


        if (isCollidingWithTarget)
        {
            ApplyHealthChange();
        }


        Vector2 direction = Vector2.zero;
        if(DistanceToTarget() < followRange) 
        {
            direction = DirectionToTarget();
        
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void ApplyHealthChange()
    {
        AttackSO attackSO = stats.CurrentStat.attackSO;
        bool isAttackable = collidingTargetHealthSystem.ChangeHealth(-attackSO.power); // if hit and not invincible true and give damage

        if (isAttackable && attackSO.isOnKnockBack && collidingMovement != null)
        {
            collidingMovement.ApplyKnockback(transform, attackSO.knockBackPower, attackSO.knockBackTime);
        }

    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Angle
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f; // character rotate flip
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag(targetTag)) // if not target , undo
        {
            return;
        }

        collidingTargetHealthSystem = collision.GetComponent<HealthSystem>();
        if (collidingTargetHealthSystem != null)
        {
            isCollidingWithTarget = true;
        }

        collidingMovement = collision.GetComponent<TopDownMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag)) { return; }// if not target , undo
        isCollidingWithTarget = false; // no Damage




    }


}