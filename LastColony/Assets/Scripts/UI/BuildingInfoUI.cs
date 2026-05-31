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
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
            Image img = upgradeButton.GetComponent<Image>();
            if (img != null) img.color = upgradeButtonColor;
        }

        // BuildingUpgradeManager sahnede yoksa kod ile oluştur (Awake'te Singleton kurar)
        if (BuildingUpgradeManager.Instance == null)
        {
            GameObject go = new GameObject("BuildingUpgradeManager");
            go.AddComponent<BuildingUpgradeManager>();
            Debug.Log("[BuildingInfoUI] BuildingUpgradeManager sahnede yoktu, kod ile oluşturuldu.");
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
        hasGridPos = false;
        ApplyPanel(data);
    }

    public void ShowPanel(BuildingData data, Vector2Int gridPos)
    {
        currentGridPos = gridPos;
        hasGridPos = true;
        ApplyPanel(data);
    }

    private void ApplyPanel(BuildingData data)
    {
        // 5. Geçici debug logları
        Debug.Log($"ShowPanel called. upgradeButton={upgradeButton}, costText={costText}, maxLevelText={maxLevelText}");
        Debug.Log($"BuildingUpgradeManager.Instance={BuildingUpgradeManager.Instance}");

        currentData = data;

        // a. Panel aktif
        panel.SetActive(true);

        // b. Temel metinler
        buildingNameText.text = data.buildingNameTR;
        descriptionText.text  = data.description;
        detailText.text       = BuildDetailText(data);

        // c. Yükseltme UI
        RefreshUpgradeUI();
    }

    public void HidePanel() => panel.SetActive(false);
    public bool IsVisible() => panel != null && panel.activeSelf;

    private void RefreshUpgradeUI()
    {
        if (upgradeButton == null)
        {
            Debug.LogWarning("[BuildingInfoUI] upgradeButton referansı bağlı değil (null).");
            return;
        }

        var mgr = BuildingUpgradeManager.Instance;

        if (mgr == null)
        {
            Debug.LogWarning("BuildingUpgradeManager.Instance is null");
            // Sistem yüklenmese bile butonu göster, ama devre dışı bırak
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = false;
            if (costText != null)
            {
                costText.gameObject.SetActive(true);
                costText.text = "Yükseltme sistemi yüklenmedi";
            }
            if (maxLevelText != null) maxLevelText.gameObject.SetActive(false);
            return;
        }

        if (!hasGridPos)
        {
            Debug.LogWarning("[BuildingInfoUI] gridPos iletilmedi, yükseltme bilgisi gösterilemiyor.");
            upgradeButton.gameObject.SetActive(false);
            if (costText != null)     costText.gameObject.SetActive(false);
            if (maxLevelText != null) maxLevelText.gameObject.SetActive(false);
            return;
        }

        int tier = mgr.GetTier(currentGridPos);
        Debug.Log($"[BuildingInfoUI] gridPos={currentGridPos}, tier={tier}, canUpgrade={mgr.CanUpgrade(currentGridPos)}");

        if (mgr.CanUpgrade(currentGridPos))
        {
            var cost = mgr.GetUpgradeCost(currentGridPos);

            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = true;
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
            // Tier 3 (maksimum) veya kayıtsız bina
            upgradeButton.gameObject.SetActive(false);
            if (costText != null) costText.gameObject.SetActive(false);
            if (maxLevelText != null)
            {
                maxLevelText.gameObject.SetActive(true);
                maxLevelText.text = "✓ MAKSİMUM SEVİYE";
            }
        }
    }

    private void OnUpgradeClicked()
    {
        Debug.Log("Upgrade button clicked!");
        Debug.Log($"currentGridPos={currentGridPos}");
        Debug.Log($"BuildingUpgradeManager.Instance={BuildingUpgradeManager.Instance}");

        var mgr = BuildingUpgradeManager.Instance;
        if (!hasGridPos || mgr == null)
        {
            Debug.LogWarning($"[BuildingInfoUI] Yükseltme yapılamıyor. hasGridPos={hasGridPos}, mgr={mgr}");
            return;
        }

        var cost = mgr.GetUpgradeCost(currentGridPos);
        Debug.Log($"Upgrade cost: lumber={cost.lumber}, stone={cost.processedStone}, metal={cost.metal}");
        Debug.Log($"Current lumber={ResourceManager.Instance.GetResource("Lumber")}, " +
                  $"stone={ResourceManager.Instance.GetResource("ProcessedStone")}, " +
                  $"metal={ResourceManager.Instance.GetResource("Metal")}");

        bool success = mgr.TryUpgrade(currentGridPos);
        Debug.Log($"TryUpgrade result={success}");

        if (success)
        {
            int newTier = mgr.GetTier(currentGridPos);
            Debug.Log($"New tier={newTier}");

            GameObject building = GridManager.Instance.GetBuildingAt(currentGridPos);
            Debug.Log($"Building found={building}");

            if (building != null)
            {
                IBuildingVisual visual = building.GetComponent<IBuildingVisual>();
                Debug.Log($"Visual found={visual}");
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
