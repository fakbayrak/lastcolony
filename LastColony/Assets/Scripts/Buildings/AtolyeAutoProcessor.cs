using UnityEngine;

public class AtolyeAutoProcessor : MonoBehaviour
{
    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private int maxUnitsPerDay = 5;

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

        ResourceType[] order = { ResourceType.Wood, ResourceType.Stone, ResourceType.MetalOre };
        int produced = 0;

        // Günlük çıktı maxUnitsPerDay birimle sınırlı; bütçeye sığan reçeteler işlenir
        bool processedSomething = true;
        while (produced < maxUnitsPerDay && processedSomething)
        {
            processedSomething = false;
            foreach (ResourceType raw in order)
            {
                if (produced >= maxUnitsPerDay) break;

                int outAmount = ResourceChain.Instance.GetOutputAmount(raw);
                if (outAmount <= 0) continue;
                if (produced + outAmount > maxUnitsPerDay) continue; // bu reçete bütçeyi aşar

                if (ResourceChain.Instance.Process(raw))
                {
                    produced += outAmount;
                    processedSomething = true;
                }
            }
        }

        Debug.Log($"[AtolyeAutoProcessor] Gün {day}: {produced}/{maxUnitsPerDay} birim işlendi.");
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
