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

        int totalProduced = 0;
        ResourceType[] recipes = { ResourceType.Wood, ResourceType.Stone, ResourceType.MetalOre };

        foreach (ResourceType raw in recipes)
        {
            int outAmount = ResourceChain.Instance.GetOutputAmount(raw);
            if (outAmount <= 0) continue;

            int produced = 0;
            while (produced + outAmount <= maxUnitsPerDay)
            {
                if (!ResourceChain.Instance.Process(raw)) break;
                produced += outAmount;
            }
            totalProduced += produced;
        }
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
