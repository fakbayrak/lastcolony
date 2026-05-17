using TMPro;
using UnityEngine;

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

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;

        UpdateText(ResourceType.Wood,          ResourceManager.Instance.GetResource(ResourceType.Wood));
        UpdateText(ResourceType.Stone,         ResourceManager.Instance.GetResource(ResourceType.Stone));
        UpdateText(ResourceType.MetalOre,      ResourceManager.Instance.GetResource(ResourceType.MetalOre));
        UpdateText(ResourceType.Lumber,        ResourceManager.Instance.GetResource(ResourceType.Lumber));
        UpdateText(ResourceType.ProcessedStone, ResourceManager.Instance.GetResource(ResourceType.ProcessedStone));
        UpdateText(ResourceType.Metal,         ResourceManager.Instance.GetResource(ResourceType.Metal));
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }

    private void HandleResourceChanged(ResourceType type, int newAmount)
    {
        UpdateText(type, newAmount);
    }

    private void UpdateText(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:           if (woodText          != null) woodText.text          = $"Odun: {amount}";          break;
            case ResourceType.Stone:          if (stoneText         != null) stoneText.text         = $"Taş: {amount}";            break;
            case ResourceType.MetalOre:       if (metalOreText      != null) metalOreText.text      = $"Maden: {amount}";          break;
            case ResourceType.Lumber:         if (lumberText        != null) lumberText.text        = $"Kereste: {amount}";        break;
            case ResourceType.ProcessedStone: if (processedStoneText != null) processedStoneText.text = $"İşlenmiş Taş: {amount}"; break;
            case ResourceType.Metal:          if (metalText         != null) metalText.text         = $"Metal: {amount}";          break;
        }
    }
}
