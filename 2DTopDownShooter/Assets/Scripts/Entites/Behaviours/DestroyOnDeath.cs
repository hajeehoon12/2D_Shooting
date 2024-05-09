using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rigidbody = GetComponent<Rigidbody2D>();
        healthSystem.OnDeath += OnDeath;
    }

    private void OnDeath() // When Died
    {
        rigidbody.velocity = Vector2.zero;

        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        { 
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color; // can't access to alpha directly so copy color

        }

        foreach (Behaviour behaviour in GetComponentsInChildren<Behaviour>())
        { 
            behaviour.enabled = false;
        }

        Destroy(gameObject, 2f); // Destroy


    }
}
