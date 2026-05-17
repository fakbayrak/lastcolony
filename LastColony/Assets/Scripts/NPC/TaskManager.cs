using System.Collections.Generic;
using UnityEngine;

public enum TaskType { GatherWood, GatherStone, GatherMetal, Rest, Idle }

public class NPCTask
{
    public TaskType taskType;
    public Vector2Int targetGrid;
}

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public Dictionary<NPC, NPCTask> ActiveTasks { get; private set; } = new Dictionary<NPC, NPCTask>();

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

    public void AssignTask(NPC npc, TaskType taskType, Vector2Int targetGrid)
    {
        var task = new NPCTask { taskType = taskType, targetGrid = targetGrid };
        ActiveTasks[npc] = task;
        npc.MoveTo(targetGrid);
    }
}
