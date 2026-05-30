using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    private readonly List<NPC> allNPCs = new List<NPC>();

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
            npc.SetGatherTarget(target.transform.position, resourceKey);
            Debug.Log($"[NPCManager] {npc.name} → {resourceKey} görevine atandı");
        }
        else
        {
            Debug.LogWarning($"[NPCManager] {resourceKey} için ResourceNode bulunamadı");
            npc.SetIdle();
        }
    }
}
