using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;

    private CharacterStatsHandler statHandler;
    private float timeSinceLastChange = float.MaxValue; // time calculate from last hit

    private bool isAttacked = false;


    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; } // property large case camel
    public float MaxHealth => statHandler.CurrentStat.maxHealth;


    private void Awake()
    {
        statHandler = GetComponent<CharacterStatsHandler>();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
                isAttacked = false;
                
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (timeSinceLastChange < healthChangeDelay) // if not attacked
        {
            return false;
        }

        timeSinceLastChange = 0f;

        CurrentHealth += change; // health change value
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth); // restrict health range for 0<= health <= maxHealth

        if (CurrentHealth <= 0f)
        {
            CallDeath();
            return true;
        }
        if (change >= 0) // when change is positive = Healing character
        {
            OnHeal?.Invoke();
        }
        else // When Damage to Player
        {
            OnDamage?.Invoke();
        }

        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}
