using TMPro;
using UnityEngine;

public class ResourceHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text metalText;

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;

        UpdateText(ResourceType.Wood,  ResourceManager.Instance.GetResource(ResourceType.Wood));
        UpdateText(ResourceType.Stone, ResourceManager.Instance.GetResource(ResourceType.Stone));
        UpdateText(ResourceType.Metal, ResourceManager.Instance.GetResource(ResourceType.Metal));
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
            case ResourceType.Wood:  woodText.text  = $"Odun: {amount}";  break;
            case ResourceType.Stone: stoneText.text = $"Taş: {amount}";   break;
            case ResourceType.Metal: metalText.text = $"Metal: {amount}"; break;
        }
    }
}
