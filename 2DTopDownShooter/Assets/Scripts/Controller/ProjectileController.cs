using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;
    

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    private RangedAttackSO attackData;
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    public bool fxOnDestroy = true; // projectile visible

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    
    
    
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration *= Time.deltaTime;

        if (currentDuration > attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rigidbody.velocity = direction * attackData.speed;
    
    }

    public void InitializeAttack(Vector2 direction, RangedAttackSO attackData)
    {
        this.attackData = attackData;
        this.direction = direction;

        UpdateProjectileSprite(); // arrow size update with attackSO data size
        trailRenderer.Clear(); // trailRender = arrow tail
        currentDuration = 0;
        spriteRenderer.color = attackData.projectileColor;

        transform.right = this.direction;

        isReady = true; 
    }

    private void DestroyProjectile(Vector3 position, bool createFx) // fx = effect
    {
        if (createFx)
        {
            ParticleSystem particleSystem = GameManager.Instance.EffectParticle;

            particleSystem.transform.position = position;
            ParticleSystem.EmissionModule em = particleSystem.emission;
            em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));
            ParticleSystem.MainModule mm = particleSystem.main;
            mm.startSpeedMultiplier = attackData.size * 10f;
            particleSystem.Play();



            //Todo: particleSystem에 대해서 배우고, 무기 NameTag로 해당하는 Fx가져오기
        }
        gameObject.SetActive(false);
    }

    private void UpdateProjectileSprite()
    {
        transform.localScale = Vector3.one * attackData.size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLayerMatched(levelCollisionLayer.value, collision.gameObject.layer))
        {
            Vector2 destroyPosition = collision.ClosestPoint(transform.position) - direction * 0.2f;
            DestroyProjectile(destroyPosition, fxOnDestroy);

        }
        else if (isLayerMatched(attackData.target.value, collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                bool isAttackApplied = healthSystem.ChangeHealth(-attackData.power); // give damage so change value is negative

                if (isAttackApplied && attackData.isOnKnockBack)
                {
                    ApplyKnockback(collision);
                }

            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    private void ApplyKnockback(Collider2D collision)
    {
        TopDownMovement movement = collision.GetComponent<TopDownMovement>();
        if (movement != null)
        {
            movement.ApplyKnockback(transform, attackData.knockBackPower, attackData.knockBackTime); // give knockback with data of knockback
        }
    }

    private bool isLayerMatched(int value, int layer) // int value = layerMask, Check if else bit come in, using layermask
    {
        return value == (value | 1 << layer); // value or 1<< layer
    }
}
