using System;
using UnityEngine;

public class TopDownAnimationController : AnimationController
{
    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int isHit = Animator.StringToHash("IsHit");
    private static readonly int Attack = Animator.StringToHash("attack");

    private readonly float magnituteThreshold = 0.5f; // minimal length of moving of sensor

    private HealthSystem healthSystem;

    

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move;

        if (healthSystem != null)
        {
            healthSystem.OnDamage += Hit;
            healthSystem.OnInvincibilityEnd += InvincibilityEnd;
        }

    }

    private void Move(Vector2 vector)
    {
        animator.SetBool(isRunning, vector.magnitude > magnituteThreshold); // when walking , if walk rang is more than threshold true  else false
    }

    private void Attacking(AttackSO sO)
    {
        animator.SetTrigger(Attack);
    }

    private void Hit()
    {
        animator.SetBool(isHit, true);
    }

    private void InvincibilityEnd() // when hit invincible for a short time
    {
        animator.SetBool(isHit, false); 
    }

}