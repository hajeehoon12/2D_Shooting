using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]private string playerTag;

    public Transform Player { get; private set; }

    public ObjectPool ObjectPool { get; private set; }
    public ParticleSystem EffectParticle;

    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private int currentWaveIndex = 0;
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0;
    private int waveSpawnPosCount = 0;

    public float spawnInterval = .5f;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();

    [SerializeField] private List<GameObject> Rewards = new List<GameObject>();




    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null) Destroy(gameObject); // Defense Coding, Singleton
        Instance = this;

        Player = GameObject.FindGameObjectWithTag(playerTag).transform; // finding player object in start
        ObjectPool = GetComponent<ObjectPool>();
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();


        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));

        }

    }

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            if (currentSpawnCount == 0)
            {
                UpdateWaveUI();

                yield return new WaitForSeconds(2f);

                ProcessWaveConditions();

                yield return StartCoroutine(SpawnEnemiesInWave());

                currentWaveIndex++;

                
            }

            yield return null;
        }
    }

    

    IEnumerator SpawnEnemiesInWave()
    {
        for (int i = 0; i < waveSpawnPosCount; i++)
        {
            int posIdx = Random.Range(0, spawnPositions.Count);
            for (int j = 0; j < waveSpawnCount; j++)
            {
                SpawnEnemyAtPosition(posIdx);
                yield return new WaitForSeconds(spawnInterval);
            }

        }

        yield return null;
    }

    private void SpawnEnemyAtPosition(int posIdx)
    {
        Debug.Log("Enemy Spawn");
        int prefabIdx = Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
        currentSpawnCount++;
    }

    private void OnEnemyDeath()
    {
        Debug.Log("OnEnemyDeath");
    }

    void ProcessWaveConditions()
    {
        if (currentWaveIndex % 20 == 0)
        {
            RandomUpgrade();
        }

        if (currentWaveIndex % 10 == 0)
        {
            IncreaseSpawnPositions();
        }

        if (currentWaveIndex % 5 == 0)
        {
            CreateReawrd();
        }

        if (currentWaveIndex % 3 == 0)
        {
            IncreaseWaveSpawnCount();
        }


    }

    

    private void IncreaseWaveSpawnCount()
    {
        waveSpawnCount++;
    }

    private void CreateReawrd()
    {
        int selectedRewardIndex = Random.Range(0, Rewards.Count);
        int randomPositionIndex = Random.Range(0, spawnPositions.Count);

        GameObject obj = Rewards[selectedRewardIndex];
        Instantiate(obj, spawnPositions[randomPositionIndex].position, Quaternion.identity);

    }

    private void IncreaseSpawnPositions()
    {
        waveSpawnPosCount = waveSpawnCount + 1 > spawnPositions.Count ? waveSpawnCount : waveSpawnCount + 1;
        waveSpawnCount = 0;
    }

    private void RandomUpgrade()
    {
        Debug.Log("Random Upgrade 호출");
    }

    private void GameOver()
    {
        // UI 켜주고 게임 멈추기
        gameOverUI.SetActive(true);

    }

    private void UpdateHealthUI()
    {
        // 
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }

    private void UpdateWaveUI()
    {
        waveText.text = (currentWaveIndex + 1).ToString();
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // self scene activate
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
