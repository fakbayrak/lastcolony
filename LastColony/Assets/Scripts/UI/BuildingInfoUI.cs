using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class BuildingInfoUI : MonoBehaviour
{
    public static BuildingInfoUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("İçerik")]
    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text detailText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        panel.SetActive(false);
        if (closeButton != null)
            closeButton.onClick.AddListener(HidePanel);
    }

    private void Update()
    {
        if (!panel.activeSelf) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            HidePanel();
    }

    public void ShowPanel(BuildingData data)
    {
        buildingNameText.text = data.buildingNameTR;
        descriptionText.text  = data.description;
        detailText.text       = BuildDetailText(data);
        panel.SetActive(true);
    }

    public void HidePanel() => panel.SetActive(false);
    public bool IsVisible() => panel != null && panel.activeSelf;

    private string BuildDetailText(BuildingData data)
    {
        if (data.buildingName == "Atolye")
            return "Günlük Üretim:\n" +
                   "10 Odun → 5 Kereste\n" +
                   "10 Taş → 5 İşlenmiş Taş\n" +
                   "5 Maden → 3 Metal";

        string cost = "İnşa Maliyeti:\n";
        if (data.costLumber > 0)         cost += $"Kereste: {data.costLumber}\n";
        if (data.costProcessedStone > 0) cost += $"İşlenmiş Taş: {data.costProcessedStone}\n";
        if (data.costMetal > 0)          cost += $"Metal: {data.costMetal}";
        return cost.TrimEnd('\n');
    }
}
