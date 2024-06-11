using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]private string playerTag;

    public Transform Player { get; private set; }

    public ObjectPool ObjectPool { get; private set; }
    public ParticleSystem EffectParticle;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null) Destroy(gameObject); // Defense Coding, Singleton
        Instance = this;

        Player = GameObject.FindGameObjectWithTag(playerTag).transform; // finding player object in start
        ObjectPool = GetComponent<ObjectPool>();
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();

    }

    
}
