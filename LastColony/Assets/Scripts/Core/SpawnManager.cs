using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public static event System.Action OnRaidStarted;

    [SerializeField] int minEnemiesPerWave = 2;
    [SerializeField] int maxEnemiesPerWave = 4;
    [SerializeField] float minDaysBetweenAttacks = 5f;
    [SerializeField] float maxDaysBetweenAttacks = 10f;
    [SerializeField] GameObject enemyPrefab;

    List<Vector2Int> spawnCorners = new List<Vector2Int>();
    int nextAttackDay;
    bool attackActive;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        spawnCorners.Add(new Vector2Int(0, 0));
        spawnCorners.Add(new Vector2Int(0, 19));
        spawnCorners.Add(new Vector2Int(19, 0));
        spawnCorners.Add(new Vector2Int(19, 19));

        nextAttackDay = (int)Random.Range(minDaysBetweenAttacks, maxDaysBetweenAttacks);
    }

    void OnEnable()
    {
        DayNightCycle.OnDayPassed += HandleDayPassed;
        Enemy.OnEnemyDied += HandleEnemyDied;
    }

    void OnDestroy()
    {
        DayNightCycle.OnDayPassed -= HandleDayPassed;
        Enemy.OnEnemyDied -= HandleEnemyDied;
    }

    void HandleDayPassed(int currentDay)
    {
        if (TimeController.Instance != null && TimeController.Instance.IsPaused) return;

        if (currentDay >= nextAttackDay && !attackActive)
        {
            SpawnWave(currentDay);
            float minInterval, maxInterval;
            if (currentDay <= 30)       { minInterval = 8f;  maxInterval = 12f; }
            else if (currentDay <= 60)  { minInterval = 6f;  maxInterval = 9f;  }
            else if (currentDay <= 90)  { minInterval = 4f;  maxInterval = 7f;  }
            else                        { minInterval = 5f;  maxInterval = 8f;  }
            nextAttackDay = currentDay + Mathf.RoundToInt(Random.Range(minInterval, maxInterval));
        }
    }

    void SpawnWave(int currentDay)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("SpawnManager: enemyPrefab atanmamış.");
            return;
        }

        attackActive = true;
        OnRaidStarted?.Invoke();

        // Güne göre zorluk skalası
        int count;
        float healthMultiplier;
        float speedMultiplier;

        if (currentDay <= 30)
        {
            // Yaz: kolay
            count           = Random.Range(1, 3);
            healthMultiplier = 0.6f;
            speedMultiplier  = 0.8f;
        }
        else if (currentDay <= 60)
        {
            // Sonbahar: orta
            count           = Random.Range(2, 4);
            healthMultiplier = 0.9f;
            speedMultiplier  = 1.0f;
        }
        else if (currentDay <= 90)
        {
            // Kış: zor
            count           = Random.Range(3, 6);
            healthMultiplier = 1.3f;
            speedMultiplier  = 1.2f;
        }
        else
        {
            // İlkbahar: orta-zor
            count           = Random.Range(2, 5);
            healthMultiplier = 1.1f;
            speedMultiplier  = 1.1f;
        }

        for (int i = 0; i < count; i++)
        {
            Vector2Int corner = spawnCorners[Random.Range(0, spawnCorners.Count)];
            Vector3 spawnPos  = GridManager.Instance.GridToWorld(corner.x, corner.y);
            spawnPos.y += 0.5f;

            GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // Can ve hız çarpanlarını uygula
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
                enemy.ApplyDifficultyModifiers(healthMultiplier, speedMultiplier);
        }

        Debug.Log($"[SpawnManager] Gün {currentDay}: {count} düşman, canx{healthMultiplier}, hızx{speedMultiplier}");
    }

    void HandleEnemyDied(Enemy enemy)
    {
        Debug.Log($"Düşman öldü: {enemy.name}");
    }
}
