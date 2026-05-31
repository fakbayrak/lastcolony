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

        bool wood    = ResourceChain.Instance.Process(ResourceType.Wood);
        bool stone   = ResourceChain.Instance.Process(ResourceType.Stone);
        bool metal   = ResourceChain.Instance.Process(ResourceType.MetalOre);

        if (wood || stone || metal)
            Debug.Log($"[AtolyeAutoProcessor] Gün {day}: Otomatik işleme tamamlandı.");
        else
            Debug.Log($"[AtolyeAutoProcessor] Gün {day}: İşlenecek ham madde yok.");
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
