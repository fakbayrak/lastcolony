using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Moving, Attacking, Dead }

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth      = 80f;  // 50 → 80  (daha sağlam ama kule öldürebilir)
    [SerializeField] float moveSpeed      = 1.4f; // 2 → 1.4  (daha yavaş)
    [SerializeField] float attackDamage   = 8f;   // 10 → 8   (biraz daha az hasar)
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 2.0f; // 1.5 → 2  (daha seyrek saldırı)

    public static event Action<Enemy> OnEnemyDied;

    public EnemyState CurrentState => currentState;

    EnemyState currentState;
    NPC targetNPC;
    List<Vector3> currentPath;
    int pathIndex;
    float attackTimer;
    float health;

    void Start()
    {
        health = maxHealth;
        if (GetComponent<EnemyVisual>() == null)
            gameObject.AddComponent<EnemyVisual>();
        currentState = EnemyState.Moving;

        Vector2Int startGrid = GridManager.Instance.WorldToGrid(transform.position);
        Vector2Int endGrid = new Vector2Int(10, 10);
        List<Vector2Int> gridPath = Pathfinder.FindPath(startGrid, endGrid);

        if (gridPath != null)
        {
            currentPath = new List<Vector3>(gridPath.Count);
            foreach (Vector2Int cell in gridPath)
                currentPath.Add(GridManager.Instance.GridToWorld(cell.x, cell.y));
        }
        else
        {
            currentPath = new List<Vector3>();
        }

        pathIndex = 0;
    }

    void Update()
    {
        if (TimeController.Instance != null && TimeController.Instance.IsPaused) return;

        switch (currentState)
        {
            case EnemyState.Moving:
                HandleMoving();
                break;
            case EnemyState.Attacking:
                HandleAttacking();
                break;
            case EnemyState.Dead:
                break;
        }
    }

    void HandleMoving()
    {
        if (currentPath == null || pathIndex >= currentPath.Count)
        {
            targetNPC = FindNearestNPC();
            if (targetNPC != null)
                currentState = EnemyState.Attacking;
            return;
        }

        Vector3 target = currentPath[pathIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
            pathIndex++;
    }

    void HandleAttacking()
    {
        if (targetNPC == null || targetNPC.Health <= 0f)
        {
            targetNPC = null;
            currentState = EnemyState.Moving;
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            targetNPC.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentState == EnemyState.Dead) return;
        health -= amount;
        if (health <= 0f)
            Die();
    }

    void Die()
    {
        currentState = EnemyState.Dead;
        OnEnemyDied?.Invoke(this);
        Destroy(gameObject, 0.5f);
    }

    NPC FindNearestNPC()
    {
        List<NPC> npcs = NPCManager.Instance.GetAllNPCs();
        NPC nearest = null;
        float nearestDist = float.MaxValue;

        foreach (NPC npc in npcs)
        {
            if (npc == null || npc.Health <= 0f) continue;
            float dist = Vector3.Distance(transform.position, npc.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = npc;
            }
        }

        return nearest;
    }
}
