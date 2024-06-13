using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopDownShooting : MonoBehaviour
{
    private TopDownController controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 aimDirection = Vector2.right;

    [SerializeField] private AudioClip ShootingClip;


    private void Awake()
    {
        controller = GetComponent<TopDownController>();
        
    }

    private void Start()
    {
        controller.OnAttackEvent += OnShoot;

        controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 direction)
    {
        aimDirection = direction;
    }

    private void OnShoot(AttackSO attackSO) // Shoot = Click fire
    {
        RangedAttackSO rangedAttackSO = attackSO as RangedAttackSO; // only need rangedattackSO
        
        if (rangedAttackSO == null) return; // try attackSO -> RangedAttackSO
        
        float projectilesAngleSpace = rangedAttackSO.multipleProjectilesAngle;
        int numberOfProjectilesPerShot = rangedAttackSO.numberOfProjectilesPerShot;

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace + 0.5f * rangedAttackSO.multipleProjectilesAngle; // Angle of projectile from weapon
        
        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            
            float angle = minAngle * i * projectilesAngleSpace;
            float randomSpread = Random.Range(-rangedAttackSO.spread, rangedAttackSO.spread);
            angle += randomSpread;
            CreateProjectile(rangedAttackSO, angle);
        }
    }

    private void CreateProjectile(RangedAttackSO rangedAttackSO, float angle ) // create bullet
    {

        GameObject obj = GameManager.Instance.ObjectPool.SpawnFromPool(rangedAttackSO.bulletNameTag);
        obj.transform.position = projectileSpawnPosition.position; // arrow prefab position initialize
        ProjectileController attackController = obj.GetComponent<ProjectileController>();
        attackController.InitializeAttack(RotateVector2(aimDirection, angle), rangedAttackSO); // initializing attack projectile

        if (ShootingClip) SoundManager.PlayClip(ShootingClip);
    }

    private static Vector2 RotateVector2(Vector2 v, float angle)
    {
        return Quaternion.Euler(0f, 0f, angle) * v;
    }

}
