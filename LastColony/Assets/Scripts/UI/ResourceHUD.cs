using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHUD : MonoBehaviour
{
    [Header("Ham Maddeler")]
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text metalOreText;

    [Header("İşlenmiş Maddeler")]
    [SerializeField] private TMP_Text lumberText;
    [SerializeField] private TMP_Text processedStoneText;
    [SerializeField] private TMP_Text metalText;

    [Header("Gün / Mevsim")]
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private SeasonManager seasonManager;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text seasonText;

    [Header("Kritik Uyarılar")]
    [SerializeField] private Image[] criticalWarnings;

    private ResourceManager resourceManager;
    private const int criticalThreshold = 15;

    private void Start()
    {
        resourceManager = ResourceManager.Instance;
        resourceManager.OnResourceChanged += HandleResourceChanged;

        SetupTextStyle(woodText,           "Odun: ");
        SetupTextStyle(stoneText,          "Taş: ");
        SetupTextStyle(metalOreText,       "Cevher: ");
        SetupTextStyle(lumberText,         "Kereste: ");
        SetupTextStyle(processedStoneText, "İşlenmiş Taş: ");
        SetupTextStyle(metalText,          "Metal: ");
        SetupTextStyle(dayText,            "");
        SetupTextStyle(seasonText,         "");

        UpdateText(ResourceType.Wood,          resourceManager.GetResource(ResourceType.Wood));
        UpdateText(ResourceType.Stone,         resourceManager.GetResource(ResourceType.Stone));
        UpdateText(ResourceType.MetalOre,      resourceManager.GetResource(ResourceType.MetalOre));
        UpdateText(ResourceType.Lumber,        resourceManager.GetResource(ResourceType.Lumber));
        UpdateText(ResourceType.ProcessedStone, resourceManager.GetResource(ResourceType.ProcessedStone));
        UpdateText(ResourceType.Metal,         resourceManager.GetResource(ResourceType.Metal));
    }

    private void Update()
    {
        UpdateDaySeasonDisplay();
        UpdateCriticalWarnings();
    }

    private void OnDestroy()
    {
        if (resourceManager != null)
            resourceManager.OnResourceChanged -= HandleResourceChanged;
    }

    private void UpdateDaySeasonDisplay()
    {
        if (dayNightCycle != null && dayText != null)
        {
            dayText.text = $"Gün {dayNightCycle.CurrentDay}";
            dayText.fontSize = 22;
        }

        if (seasonManager != null && seasonText != null)
        {
            string seasonName = seasonManager.CurrentSeason switch
            {
                SeasonManager.Season.Summer => "YAZ",
                SeasonManager.Season.Autumn => "SONBAHAR",
                SeasonManager.Season.Winter => "KIŞ",
                SeasonManager.Season.Spring => "İLKBAHAR",
                _ => "?"
            };
            seasonText.text = seasonName;
            seasonText.fontSize = 20;

            Color seasonColor = seasonManager.CurrentSeason switch
            {
                SeasonManager.Season.Summer => new Color(1f, 0.9f, 0.3f),
                SeasonManager.Season.Autumn => new Color(1f, 0.5f, 0.1f),
                SeasonManager.Season.Winter => new Color(0.7f, 0.9f, 1f),
                SeasonManager.Season.Spring => new Color(0.5f, 1f, 0.5f),
                _ => Color.white
            };
            seasonText.color = seasonColor;
        }
    }

    private void UpdateCriticalWarnings()
    {
        if (resourceManager == null || criticalWarnings == null) return;

        var inventory = resourceManager.GetInventory();
        string[] watchedResources = { "Wood", "Stone", "MetalOre" };

        for (int i = 0; i < criticalWarnings.Length && i < watchedResources.Length; i++)
        {
            if (criticalWarnings[i] == null) continue;

            string key = watchedResources[i];
            bool isCritical = inventory.ContainsKey(key) && inventory[key] < criticalThreshold;
            criticalWarnings[i].color = isCritical
                ? Color.red
                : new Color(1f, 1f, 1f, 0.3f);
        }
    }

    private void HandleResourceChanged(ResourceType type, int newAmount)
    {
        UpdateText(type, newAmount);
    }

    private void UpdateText(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:           if (woodText           != null) woodText.text           = $"Odun:          {amount}"; break;
            case ResourceType.Stone:          if (stoneText          != null) stoneText.text          = $"Taş:           {amount}"; break;
            case ResourceType.MetalOre:       if (metalOreText       != null) metalOreText.text       = $"Cevher:        {amount}"; break;
            case ResourceType.Lumber:         if (lumberText         != null) lumberText.text         = $"Kereste:       {amount}"; break;
            case ResourceType.ProcessedStone: if (processedStoneText != null) processedStoneText.text = $"İşlenmiş Taş: {amount}"; break;
            case ResourceType.Metal:          if (metalText          != null) metalText.text          = $"Metal:         {amount}"; break;
        }
    }

    private void SetupTextStyle(TMP_Text t, string label)
    {
        if (t == null) return;
        t.fontSize = 18;
        t.color = Color.white;
        t.outlineWidth = 0.2f;
        t.outlineColor = new Color32(0, 0, 0, 200);
    }
}
