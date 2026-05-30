using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState { Idle, Moving, Working, Resting, Dead }

public class NPC : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float hunger = 0f;
    [SerializeField] private float energy = 100f;

    public event Action OnDeath;

    public float Health       => health;
    public float Hunger       => hunger;
    public float Energy       => energy;
    public NPCState CurrentState => state;

    private NPCState state = NPCState.Idle;
    private GridManager gridManager;
    private Coroutine moveCoroutine;
    private ResourceNode currentResourceNode;

    private const float HungerPerSecond      = 0.5f;
    private const float EnergyDrainPerSecond  = 10f;
    private const float EnergyRestorePerSecond = 15f;
    private const float MoveStepDelay         = 0.3f;
    private const float RestEnergyThreshold   = 20f;
    private const float ReadyEnergyThreshold  = 80f;
    private const float HungerWorkBlock       = 80f;

    private void Awake()
    {
        gridManager = GridManager.Instance;
        NPCManager.Instance.RegisterNPC(this);
    }

    private void Update()
    {
        if (TimeController.Instance != null && TimeController.Instance.IsPaused) return;

        hunger = Mathf.Min(100f, hunger + HungerPerSecond * Time.deltaTime);

        UpdateStateMachine();

    }

    private void UpdateStateMachine()
    {
        switch (state)
        {
            case NPCState.Idle:
                if (energy < RestEnergyThreshold)
                    SetState(NPCState.Resting);
                break;

            case NPCState.Working:
                energy = Mathf.Max(0f, energy - EnergyDrainPerSecond * Time.deltaTime);

                if (energy < RestEnergyThreshold)
                {
                    SetState(NPCState.Resting);
                    break;
                }
                if (hunger >= HungerWorkBlock)
                    SetState(NPCState.Idle);
                break;

            case NPCState.Resting:
                energy = Mathf.Min(100f, energy + EnergyRestorePerSecond * Time.deltaTime);

                if (energy >= ReadyEnergyThreshold)
                    SetState(NPCState.Idle);
                break;

            // Moving: hareketi kesme, varışta karar ver (FollowPath sonu)
        }
    }

    public void MoveTo(Vector2Int targetGrid)
    {
        // Doğrudan hareket (genel görev) — kaynak toplama hedefi yok
        currentResourceNode = null;
        MoveToInternal(targetGrid);
    }

    private void MoveToInternal(Vector2Int targetGrid)
    {
        if (state == NPCState.Resting || hunger >= HungerWorkBlock)
            return;

        Vector2Int currentGrid = gridManager.WorldToGrid(transform.position);
        List<Vector2Int> path = Pathfinder.FindPath(currentGrid, targetGrid);

        if (path == null)
            return;

        if (path.Count <= 1)
        {
            // Zaten hedefte
            SetState(NPCState.Working);
            return;
        }

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        SetState(NPCState.Moving);
        moveCoroutine = StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<Vector2Int> path)
    {
        for (int i = 1; i < path.Count; i++)
        {
            transform.position = gridManager.GridToWorld(path[i].x, path[i].y);
            yield return new WaitForSeconds(MoveStepDelay);
        }

        moveCoroutine = null;

        if (energy < RestEnergyThreshold)
            SetState(NPCState.Resting);
        else if (hunger >= HungerWorkBlock)
            SetState(NPCState.Idle);
        else
            SetState(NPCState.Working);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0f);

        if (health <= 0f)
            Die();
    }

    void Die()
    {
        state = NPCState.Dead;
        Debug.Log("[NPC] Bir kolonici hayatını kaybetti.");
        NPCManager.Instance.UnregisterNPC(this);
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetIdle()
    {
        state = NPCState.Idle;
    }

    public void SetGatherTarget(ResourceNode node)
    {
        if (node == null)
            return;

        // Toplanacak node'u sakla; hedefe varıp Working state'e geçince gather tetiklenir
        currentResourceNode = node;

        // Dünya konumunu grid hücresine çevir ve mevcut pathfinding + state machine ile git
        Vector2Int targetGrid = gridManager.WorldToGrid(node.transform.position);
        Debug.Log($"[NPC] {name} → {node.ResourceType} hedefine yönlendiriliyor ({targetGrid})");
        MoveToInternal(targetGrid);
    }

    private void SetState(NPCState newState)
    {
        state = newState;

        // Hedefe varıp çalışmaya başlayınca kaynak toplamayı tetikle
        if (newState == NPCState.Working)
            TryStartGathering();
    }

    private void TryStartGathering()
    {
        // Yalnızca bir kaynak hedefi atanmışsa topla (genel görevlerde currentResourceNode null'dur)
        if (currentResourceNode == null || !currentResourceNode.IsAvailable())
            return;
        if (currentResourceNode.IsGathering)
            return;

        currentResourceNode.Gather(this);
    }
}
