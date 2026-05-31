using System.Collections.Generic;
using UnityEngine;

// Tier görsellerini yöneten tüm Visual scriptlerin uyguladığı arayüz.
public interface IBuildingVisual
{
    void UpgradeTo(int tier);
}

public class BuildingUpgradeManager : MonoBehaviour
{
    public static BuildingUpgradeManager Instance { get; private set; }

    // key: grid pozisyonu, value: mevcut tier (1,2,3)
    private Dictionary<Vector2Int, int> buildingTiers = new Dictionary<Vector2Int, int>();
    // key: grid pozisyonu, value: BuildingType string
    private Dictionary<Vector2Int, string> buildingTypes = new Dictionary<Vector2Int, string>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RegisterBuilding(Vector2Int gridPos, string buildingType)
    {
        buildingTiers[gridPos] = 1;
        buildingTypes[gridPos] = buildingType;
    }

    public int GetTier(Vector2Int gridPos)
    {
        return buildingTiers.ContainsKey(gridPos) ? buildingTiers[gridPos] : 1;
    }

    public string GetBuildingType(Vector2Int gridPos)
    {
        return buildingTypes.ContainsKey(gridPos) ? buildingTypes[gridPos] : "";
    }

    public bool CanUpgrade(Vector2Int gridPos)
    {
        return buildingTiers.ContainsKey(gridPos) && buildingTiers[gridPos] < 3;
    }

    // Upgrade maliyetlerini döndürür (Lumber, ProcessedStone, Metal)
    public (int lumber, int processedStone, int metal) GetUpgradeCost(Vector2Int gridPos)
    {
        if (!buildingTiers.ContainsKey(gridPos)) return (0,0,0);
        int tier = buildingTiers[gridPos];
        string type = buildingTypes[gridPos];

        if (type == "Baraka")
            return tier == 1 ? (30,0,0) : (20,15,0);
        if (type == "Depo")
            return tier == 1 ? (25,0,0) : (15,20,0);
        if (type == "Atolye")
            return tier == 1 ? (35,0,10) : (0,25,15);
        if (type == "Kule")
            return tier == 1 ? (0,20,10) : (0,30,20);
        return (0,0,0);
    }

    public bool TryUpgrade(Vector2Int gridPos)
    {
        if (!CanUpgrade(gridPos)) return false;
        var cost = GetUpgradeCost(gridPos);
        var rm = ResourceManager.Instance;
        if (rm.GetResource("Lumber") < cost.lumber) return false;
        if (rm.GetResource("ProcessedStone") < cost.processedStone) return false;
        if (rm.GetResource("Metal") < cost.metal) return false;

        rm.RemoveResource("Lumber", cost.lumber);
        rm.RemoveResource("ProcessedStone", cost.processedStone);
        rm.RemoveResource("Metal", cost.metal);
        buildingTiers[gridPos]++;
        return true;
    }

    public void RemoveBuilding(Vector2Int gridPos)
    {
        buildingTiers.Remove(gridPos);
        buildingTypes.Remove(gridPos);
    }
}
