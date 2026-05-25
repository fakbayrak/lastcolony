using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Color iconColor = Color.white;
    [SerializeField] private string resourceKey;
    [SerializeField] private string resourceDisplayName;

    private TooltipUI tooltip;

    public string ResourceKey => resourceKey;

    public void Initialize(string key, string displayName, Color color, TooltipUI tooltipRef)
    {
        resourceKey = key;
        resourceDisplayName = displayName;
        iconColor = color;
        tooltip = tooltipRef;

        if (iconImage != null)
            iconImage.color = iconColor;
    }

    public void UpdateAmount(int amount)
    {
        if (amountText != null)
            amountText.text = amount.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Show(resourceDisplayName, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Hide();
    }
}
