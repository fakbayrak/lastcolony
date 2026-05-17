using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;

    private static readonly Vector2Int[] spawnPoints =
    {
        new Vector2Int(2, 2),
        new Vector2Int(3, 3),
        new Vector2Int(4, 4),
        new Vector2Int(5, 5),
        new Vector2Int(6, 6),
    };

    private void Start()
    {
        foreach (Vector2Int gridPos in spawnPoints)
        {
            Vector3 worldPos = GridManager.Instance.GridToWorld(gridPos.x, gridPos.y);
            Instantiate(npcPrefab, worldPos, Quaternion.identity);
        }

        StartCoroutine(AssignTaskDelayed());
        StartCoroutine(LogAllNPCStates());
        StartCoroutine(TestResourceChain());
    }

    private IEnumerator AssignTaskDelayed()
    {
        yield return new WaitForSeconds(3f);
        NPCManager.Instance.AssignGatherTask(TaskType.GatherWood, new Vector2Int(15, 10), 3);
    }

    private IEnumerator TestResourceChain()
    {
        yield return new WaitForSeconds(5f);
        ResourceManager.Instance.AddResource(ResourceType.Wood, 20);
        bool result = ResourceChain.Instance.Process(ResourceType.Wood);
        Debug.Log($"[Test] İşleme sonucu: {result} | " +
                  $"Wood: {ResourceManager.Instance.GetResource(ResourceType.Wood)} | " +
                  $"Lumber: {ResourceManager.Instance.GetResource(ResourceType.Lumber)}");
    }

    private IEnumerator LogAllNPCStates()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            List<NPC> npcs = NPCManager.Instance.GetAllNPCs();
            if (npcs.Count == 0)
                continue;

            StringBuilder sb = new StringBuilder("[NPCManager] ");
            for (int i = 0; i < npcs.Count; i++)
            {
                if (i > 0) sb.Append(" | ");
                sb.Append($"NPC{i}: {npcs[i].CurrentState}");
            }

            Debug.Log(sb.ToString());
        }
    }
}
