using UnityEngine;

public class AtolyeAutoProcessor : MonoBehaviour
{
    [SerializeField] private BuildingPlacement buildingPlacement;

    private void OnEnable()
    {
        DayNightCycle.OnDayPassed += OnDayPassed;
    }

    private void OnDisable()
    {
        DayNightCycle.OnDayPassed -= OnDayPassed;
    }

    private void OnDayPassed(int day)
    {
        if (!HasAtolye()) return;

        int processCount = HasAssignedNPC() ? 2 : 1;

        for (int i = 0; i < processCount; i++)
        {
            ResourceChain.Instance.Process(ResourceType.Wood);
            ResourceChain.Instance.Process(ResourceType.Stone);
            ResourceChain.Instance.Process(ResourceType.MetalOre);
        }

        Debug.Log($"[AtolyeAutoProcessor] Gün {day}: {processCount}x işleme tamamlandı.");
    }

    private bool HasAssignedNPC()
    {
        foreach (NPC npc in NPCManager.Instance.GetAllNPCs())
        {
            if (npc.CurrentState == NPCState.AssignedToBuilding &&
                npc.AssignedBuildingType == "Atolye")
                return true;
        }
        return false;
    }

    private bool HasAtolye()
    {
        if (buildingPlacement == null) return false;

        foreach (var record in buildingPlacement.GetPlacedBuildings())
        {
            if (record.buildingType == "Atolye")
                return true;
        }
        return false;
    }
}
