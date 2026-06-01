using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    private readonly List<NPC> allNPCs = new List<NPC>();

    // Hangi NPC hangi kaynak tipini topluyor — node tükenince yeniden atamak için saklanır
    private Dictionary<NPC, ResourceType> npcGatherTypes = new Dictionary<NPC, ResourceType>();

    private float reassignTimer = 0f;
    private float reassignInterval = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterNPC(NPC npc)
    {
        if (!allNPCs.Contains(npc))
            allNPCs.Add(npc);
    }

    public void UnregisterNPC(NPC npc)
    {
        allNPCs.Remove(npc);
        npcGatherTypes.Remove(npc);
    }

    private void Update()
    {
        reassignTimer += Time.deltaTime;
        if (reassignTimer >= reassignInterval)
        {
            reassignTimer = 0f;
            TryReassignIdleNPCs();
        }
    }

    // Boşta kalmış ama bir toplama görevi olan NPC'leri en yakın uygun node'a yeniden ata
    private void TryReassignIdleNPCs()
    {
        if (npcGatherTypes.Count == 0) return;

        ResourceNode[] nodes = FindObjectsByType<ResourceNode>(FindObjectsSortMode.None);

        foreach (var kvp in npcGatherTypes)
        {
            NPC npc = kvp.Key;
            if (npc == null || npc.CurrentState != NPCState.Idle) continue;

            ResourceType wanted = kvp.Value;
            ResourceNode nearest = null;
            float bestSqr = float.MaxValue;

            foreach (ResourceNode node in nodes)
            {
                if (node == null || !node.IsAvailable() || node.ResourceType != wanted) continue;

                float sqr = (node.transform.position - npc.transform.position).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    nearest = node;
                }
            }

            if (nearest != null)
                npc.SetGatherTarget(nearest);
        }
    }

    public int GetIdleNPCCount()
    {
        int count = 0;
        foreach (NPC npc in allNPCs)
            if (npc.CurrentState == NPCState.Idle)
                count++;
        return count;
    }

    public List<NPC> GetAllNPCs() => new List<NPC>(allNPCs);

    public void AssignGatherTask(TaskType taskType, Vector2Int targetGrid, int npcCount)
    {
        int assigned = 0;
        foreach (NPC npc in allNPCs)
        {
            if (assigned >= npcCount)
                break;
            if (npc.CurrentState == NPCState.Idle)
            {
                TaskManager.Instance.AssignTask(npc, taskType, targetGrid);
                assigned++;
            }
        }

        if (assigned < npcCount)
            Debug.Log($"NPCManager: {npcCount} NPC istendi, yalnızca {assigned} atanabildi (yeterli Idle NPC yok).");
    }

    public void AssignGatherTask(NPC npc, string resourceKey)
    {
        // Node tükenince yeniden atayabilmek için bu NPC'nin kaynak tipini sakla
        if (System.Enum.TryParse<ResourceType>(resourceKey, out ResourceType gatherType))
            npcGatherTypes[npc] = gatherType;

        // Haritada o tipte uygun bir ResourceNode bul
        ResourceNode[] nodes = FindObjectsByType<ResourceNode>(FindObjectsSortMode.None);
        ResourceNode target = null;

        foreach (ResourceNode node in nodes)
        {
            if (node.ResourceType.ToString() == resourceKey && node.IsAvailable())
            {
                target = node;
                break;
            }
        }

        if (target != null)
        {
            npc.SetGatherTarget(target);
            Debug.Log($"[NPCManager] {npc.name} → {resourceKey} görevine atandı");
        }
        else
        {
            Debug.LogWarning($"[NPCManager] {resourceKey} için ResourceNode bulunamadı");
            npc.SetIdle();
        }
    }
}
