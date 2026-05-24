using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

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
        if (currentDay >= nextAttackDay && !attackActive)
        {
            SpawnWave(currentDay);
            nextAttackDay = currentDay + Mathf.RoundToInt(Random.Range(minDaysBetweenAttacks, maxDaysBetweenAttacks));
        }
    }

    void SpawnWave(int currentDay)
    {
        Debug.Log($"SpawnWave çağrıldı! Gün: {currentDay}");
        if (enemyPrefab == null)
        {
            Debug.LogError("SpawnManager: enemyPrefab atanmamış.");
            return;
        }

        attackActive = true;
        int count = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2Int corner = spawnCorners[Random.Range(0, spawnCorners.Count)];
            Vector3 spawnPos = GridManager.Instance.GridToWorld(corner.x, corner.y);
            spawnPos.y += 0.5f;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }

        Debug.Log($"Düşman dalgası: {count} düşman spawn oldu, Gün: {currentDay}");
    }

    void HandleEnemyDied(Enemy enemy)
    {
        Debug.Log($"Düşman öldü: {enemy.name}");
    }
}
