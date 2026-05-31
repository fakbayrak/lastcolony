using System.Collections;
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

    [Header("Yükseltme")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text upgradeButtonText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text maxLevelText;

    private BuildingData currentData;
    private Vector2Int currentGridPos;
    private bool hasGridPos;
    private Color upgradeButtonColor = new Color(0.953f, 0.612f, 0.071f); // #F39C12
    private Coroutine flashRoutine;

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
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
            Image img = upgradeButton.GetComponent<Image>();
            if (img != null) img.color = upgradeButtonColor;
        }
    }

    private void Update()
    {
        if (!panel.activeSelf) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            HidePanel();
    }

    public void ShowPanel(BuildingData data)
    {
        currentData = data;
        hasGridPos = false;
        buildingNameText.text = data.buildingNameTR;
        descriptionText.text  = data.description;
        detailText.text       = BuildDetailText(data);
        RefreshUpgradeUI();
        panel.SetActive(true);
    }

    public void ShowPanel(BuildingData data, Vector2Int gridPos)
    {
        currentGridPos = gridPos;
        hasGridPos = true;
        currentData = data;
        buildingNameText.text = data.buildingNameTR;
        descriptionText.text  = data.description;
        detailText.text       = BuildDetailText(data);
        RefreshUpgradeUI();
        panel.SetActive(true);
    }

    public void HidePanel() => panel.SetActive(false);
    public bool IsVisible() => panel != null && panel.activeSelf;

    private void RefreshUpgradeUI()
    {
        var mgr = BuildingUpgradeManager.Instance;
        if (!hasGridPos || mgr == null)
        {
            if (upgradeButton != null) upgradeButton.gameObject.SetActive(false);
            if (costText != null)      costText.gameObject.SetActive(false);
            if (maxLevelText != null)  maxLevelText.gameObject.SetActive(false);
            return;
        }

        int tier = mgr.GetTier(currentGridPos);

        if (mgr.CanUpgrade(currentGridPos))
        {
            var cost = mgr.GetUpgradeCost(currentGridPos);

            if (upgradeButton != null) upgradeButton.gameObject.SetActive(true);
            if (upgradeButtonText != null)
                upgradeButtonText.text = $"YÜKSELT (Tier {tier}→{tier + 1})";
            if (costText != null)
            {
                costText.gameObject.SetActive(true);
                costText.text = $"Maliyet: {cost.lumber} Kereste, " +
                                $"{cost.processedStone} İşlenmiş Taş, {cost.metal} Metal";
            }
            if (maxLevelText != null) maxLevelText.gameObject.SetActive(false);
        }
        else
        {
            if (upgradeButton != null) upgradeButton.gameObject.SetActive(false);
            if (costText != null)      costText.gameObject.SetActive(false);
            if (maxLevelText != null)
            {
                maxLevelText.gameObject.SetActive(true);
                maxLevelText.text = "Maksimum Seviye";
            }
        }
    }

    private void OnUpgradeClicked()
    {
        var mgr = BuildingUpgradeManager.Instance;
        if (!hasGridPos || mgr == null) return;

        if (mgr.TryUpgrade(currentGridPos))
        {
            int newTier = mgr.GetTier(currentGridPos);
            GameObject building = GridManager.Instance.GetBuildingAt(currentGridPos);
            if (building != null)
            {
                IBuildingVisual visual = building.GetComponent<IBuildingVisual>();
                if (visual != null) visual.UpgradeTo(newTier);
            }
            RefreshUpgradeUI();
        }
        else
        {
            // Yetersiz kaynak — butonu kısa süre kırmızı yap
            if (upgradeButton != null)
            {
                if (flashRoutine != null) StopCoroutine(flashRoutine);
                flashRoutine = StartCoroutine(FlashButtonRed());
            }
        }
    }

    private IEnumerator FlashButtonRed()
    {
        Image img = upgradeButton.GetComponent<Image>();
        if (img == null) yield break;

        img.color = new Color(0.8f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.4f);
        img.color = upgradeButtonColor;
    }

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
