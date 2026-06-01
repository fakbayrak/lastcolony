using System.Collections.Generic;
using UnityEngine;

public class BuildingAssignmentManager : MonoBehaviour
{
    public static BuildingAssignmentManager Instance { get; private set; }

    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private DefenseTower defenseTower;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

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
        AssignIdleNPCsToBuildings();
    }

    private void AssignIdleNPCsToBuildings()
    {
        if (buildingPlacement == null) return;

        var placedBuildings = buildingPlacement.GetPlacedBuildings();
        if (placedBuildings.Count == 0) return;

        // Önce kule bonusunu sıfırla
        if (defenseTower != null)
            defenseTower.RemoveNPCBonus();

        // Baraka NPC'lerinin enerji bonusunu sıfırla
        ResetBarakaBonus();

        // Idle NPC listesi
        List<NPC> idleNPCs = new List<NPC>();
        foreach (NPC npc in NPCManager.Instance.GetAllNPCs())
        {
            if (npc.CurrentState == NPCState.Idle)
                idleNPCs.Add(npc);
        }

        if (idleNPCs.Count == 0) return;

        int npcIndex = 0;

        foreach (var record in placedBuildings)
        {
            if (npcIndex >= idleNPCs.Count) break;

            Vector2Int buildingGrid = new Vector2Int(record.gridX, record.gridY);
            NPC npc = idleNPCs[npcIndex];
            npc.SetBuildingTarget(buildingGrid, record.buildingType);

            // Bina tipine göre bonus uygula
            ApplyBuildingBonus(record.buildingType, npc);

            Debug.Log($"[BuildingAssignmentManager] {npc.name} → {record.buildingType} binasına atandı.");
            npcIndex++;
        }
    }

    private void ApplyBuildingBonus(string buildingType, NPC npc)
    {
        switch (buildingType)
        {
            case "Kule":
                if (defenseTower != null)
                    defenseTower.ApplyNPCBonus();
                break;
            case "Baraka":
                // Baraka bonusu enerji sistemiyle birlikte kaldırıldı
                break;
            // Depo bonusları NPC üzerinden uygulanır
        }
    }

    private void ResetBarakaBonus()
    {
        // Enerji sistemi kaldırıldı; sıfırlanacak bonus yok
    }
}
