using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHUD : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private SeasonManager seasonManager;

    [Header("Gün / Mevsim")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text seasonText;

    [Header("Resource Slot Container")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private TooltipUI tooltipUI;

    [Header("Kaynak İkonları")]
    [SerializeField] private Sprite iconWood;
    [SerializeField] private Sprite iconStone;
    [SerializeField] private Sprite iconMetalOre;
    [SerializeField] private Sprite iconLumber;
    [SerializeField] private Sprite iconProcessedStone;
    [SerializeField] private Sprite iconMetal;
    [SerializeField] private Sprite iconFood;

    private List<ResourceSlotUI> slots = new List<ResourceSlotUI>();

    private void Start()
    {
        if (resourceManager == null)
            resourceManager = ResourceManager.Instance;

        BuildSlots();
    }

    private void BuildSlots()
    {
        if (slotContainer == null || slotPrefab == null) return;

        var defs = new (string key, string name, Color color, Sprite icon)[]
        {
            ("Wood",           "Odun — Ham kereste",         new Color(0.6f,  0.35f, 0.1f), iconWood),
            ("Stone",          "Taş — Ham yapı malzemesi",   new Color(0.6f,  0.6f,  0.6f), iconStone),
            ("MetalOre",       "Cevher — Ham metal",         new Color(0.4f,  0.7f,  0.4f), iconMetalOre),
            ("Lumber",         "Kereste — İşlenmiş odun",    new Color(0.85f, 0.55f, 0.2f), iconLumber),
            ("ProcessedStone", "İşlenmiş Taş",               new Color(0.8f,  0.8f,  0.9f), iconProcessedStone),
            ("Metal",          "Metal — İşlenmiş cevher",    new Color(0.5f,  0.8f,  1.0f), iconMetal),
            ("Food",           "Yiyecek — Koloni erzakı",    new Color(0.9f,  0.7f,  0.2f), iconFood),
        };

        foreach (var def in defs)
        {
            GameObject go = Instantiate(slotPrefab, slotContainer);
            go.name = $"Slot_{def.key}";

            ResourceSlotUI slot = go.GetComponent<ResourceSlotUI>();
            if (slot != null)
            {
                slot.Initialize(def.key, def.name, def.color, def.icon, tooltipUI);
                slots.Add(slot);
            }
        }
    }

    private void Update()
    {
        UpdateResources();
        UpdateDaySeasonDisplay();
    }

    private void UpdateResources()
    {
        if (resourceManager == null) return;
        var inventory = resourceManager.GetInventory();

        foreach (var slot in slots)
        {
            if (inventory.ContainsKey(slot.ResourceKey))
                slot.UpdateAmount(inventory[slot.ResourceKey]);
        }
    }

    private void UpdateDaySeasonDisplay()
    {
        if (dayNightCycle != null && dayText != null)
            dayText.text = $"Gün {dayNightCycle.CurrentDay}";

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

            seasonText.color = seasonManager.CurrentSeason switch
            {
                SeasonManager.Season.Summer => new Color(1f, 0.9f, 0.3f),
                SeasonManager.Season.Autumn => new Color(1f, 0.5f, 0.1f),
                SeasonManager.Season.Winter => new Color(0.7f, 0.9f, 1f),
                SeasonManager.Season.Spring => new Color(0.5f, 1f, 0.5f),
                _ => Color.white
            };
        }
    }
}
