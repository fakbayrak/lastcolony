using TMPro;
using UnityEngine;

public class ColonyManager : MonoBehaviour
{
    public static ColonyManager Instance { get; private set; }

    [SerializeField] private NPCManager npcManager;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private TimeController timeController;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private SeasonManager seasonManager;

    private bool colonyCollapsed = false;
    private float hungerDamageInterval = 5f;
    private float hungerDamageTimer = 0f;
    private float hungerDamageAmount = 10f;
    private float starvationThreshold = 10f;
    private float foodPerNPCPerDay = 2f;
    private float foodProductionPerFarm = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        DayNightCycle.OnDayPassed += OnDayPassed;
    }

    private void OnDestroy()
    {
        DayNightCycle.OnDayPassed -= OnDayPassed;
    }

    private void Update()
    {
        if (colonyCollapsed) return;
        if (timeController != null && timeController.IsPaused) return;

        hungerDamageTimer += Time.deltaTime;
        if (hungerDamageTimer >= hungerDamageInterval)
        {
            hungerDamageTimer = 0f;
            ApplyStarvationDamage();
        }

        CheckColonyStatus();
    }

    private void OnDayPassed(int day)
    {
        if (colonyCollapsed) return;
        ConsumeFood();
        ProduceFood();
        ApplyHungerFromFood();
    }

    private void ConsumeFood()
    {
        if (npcManager == null || resourceManager == null) return;
        int aliveNPCs = 0;
        foreach (NPC npc in npcManager.GetAllNPCs())
        {
            if (npc != null && npc.Health > 0f) aliveNPCs++;
        }
        float multiplier = (seasonManager != null) ? seasonManager.ConsumptionMultiplier : 1f;
        float totalConsumption = aliveNPCs * foodPerNPCPerDay * multiplier;
        resourceManager.RemoveResource("Food", (int)totalConsumption);
    }

    private void ProduceFood()
    {
        if (resourceManager == null) return;
        int farmCount = CountBuildings("Tarla");
        if (farmCount <= 0) return;
        float multiplier = (seasonManager != null) ? seasonManager.ProductionMultiplier : 1f;
        float totalProduction = farmCount * foodProductionPerFarm * multiplier;
        resourceManager.AddResource("Food", (int)totalProduction);
    }

    private int CountBuildings(string buildingType)
    {
        if (BuildingUpgradeManager.Instance == null) return 0;
        return BuildingUpgradeManager.Instance.CountBuildingsOfType(buildingType);
    }

    private void ApplyHungerFromFood()
    {
        if (resourceManager == null || npcManager == null) return;
        float currentFood = resourceManager.GetResource("Food");
        foreach (NPC npc in npcManager.GetAllNPCs())
        {
            if (npc == null || npc.Health <= 0f) continue;
            if (currentFood <= 0f)
            {
                npc.Hunger = Mathf.Max(0f, npc.Hunger - 20f);
            }
            else
            {
                npc.Hunger = Mathf.Min(100f, npc.Hunger + 5f);
            }
        }
    }

    private void ApplyStarvationDamage()
    {
        var allNPCs = npcManager.GetAllNPCs();
        foreach (NPC npc in allNPCs)
        {
            if (npc == null) continue;
            if (npc.Hunger < starvationThreshold)
            {
                npc.TakeDamage(hungerDamageAmount);
            }
        }
    }

    private void CheckColonyStatus()
    {
        var allNPCs = npcManager.GetAllNPCs();
        if (allNPCs == null || allNPCs.Count == 0) return;

        int aliveCount = 0;
        foreach (NPC npc in allNPCs)
        {
            if (npc != null && npc.Health > 0f)
                aliveCount++;
        }

        if (aliveCount == 0 && !colonyCollapsed)
            TriggerColonyCollapse();
    }

    private void TriggerColonyCollapse()
    {
        colonyCollapsed = true;

        if (timeController != null)
            timeController.Pause();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "KOLONİ ÇÖKTÜ\n\nTüm koloniciler hayatını kaybetti.";
    }

    public bool IsCollapsed => colonyCollapsed;
    public void ResetCollapse() { colonyCollapsed = false; }
}
