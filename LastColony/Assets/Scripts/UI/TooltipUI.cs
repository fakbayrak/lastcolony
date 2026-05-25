using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TMP_Text tooltipText;
    [SerializeField] private RectTransform canvasRect;

    private void Awake()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public void Show(string text, Vector3 worldPosition)
    {
        if (tooltipPanel == null || tooltipText == null) return;

        tooltipText.text = text;
        tooltipPanel.SetActive(true);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPosition);
        RectTransform rt = tooltipPanel.GetComponent<RectTransform>();
        if (rt != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, screenPoint, null, out localPoint);
            rt.localPosition = localPoint + new Vector2(120f, 0f);
        }
    }

    public void Hide()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }
}
