using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingToolbar : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private ResourceManager resourceManager;

    [Header("Bina Verileri")]
    [SerializeField] private BuildingData[] buildings;

    [Header("UI")]
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;

    private Button[] buttons;

    private void Start()
    {
        BuildButtons();
    }

    private void BuildButtons()
    {
        if (buttonContainer == null || buttonPrefab == null) return;

        buttons = new Button[buildings.Length];

        for (int i = 0; i < buildings.Length; i++)
        {
            BuildingData data = buildings[i];

            GameObject go = Instantiate(buttonPrefab, buttonContainer);
            go.name = $"Btn_{data.buildingNameTR}";

            Image[] images = go.GetComponentsInChildren<Image>(true);
            if (images.Length > 1 && data.toolbarIcon != null)
                images[1].sprite = data.toolbarIcon;

            TMP_Text[] texts = go.GetComponentsInChildren<TMP_Text>(true);
            if (texts.Length > 0) texts[0].text = data.buildingNameTR;
            if (texts.Length > 1) texts[1].text = BuildCostText(data);

            Debug.Log($"[Toolbar] {data.buildingNameTR} — Image sayısı: {images.Length}, Text sayısı: {texts.Length}");

            Button btn = go.GetComponent<Button>();
            if (btn != null)
            {
                int index = i;
                btn.onClick.AddListener(() => OnBuildingButtonClicked(index));
                buttons[i] = btn;
            }
        }
    }

    private string BuildCostText(BuildingData data)
    {
        string cost = "";
        if (data.costLumber > 0)         cost += $"Kereste: {data.costLumber}\n";
        if (data.costProcessedStone > 0) cost += $"Taş: {data.costProcessedStone}\n";
        if (data.costMetal > 0)          cost += $"Metal: {data.costMetal}";
        return cost.TrimEnd('\n');
    }

    private void OnBuildingButtonClicked(int index)
    {
        BuildingData data = buildings[index];

        var inventory = resourceManager.GetInventory();
        bool canAfford =
            (inventory.ContainsKey("Lumber")         ? inventory["Lumber"]         : 0) >= data.costLumber &&
            (inventory.ContainsKey("ProcessedStone")  ? inventory["ProcessedStone"]  : 0) >= data.costProcessedStone &&
            (inventory.ContainsKey("Metal")          ? inventory["Metal"]          : 0) >= data.costMetal;

        if (!canAfford)
        {
            Debug.Log($"[BuildingToolbar] Yeterli kaynak yok: {data.buildingNameTR}");
            return;
        }

        buildingPlacement.SetActivePrefab(data.prefab);
        buildingPlacement.SetBuildingCost(data.costLumber, data.costProcessedStone, data.costMetal);
        buildingPlacement.EnterPlacementMode();

        Debug.Log($"[BuildingToolbar] Placement modu: {data.buildingNameTR}");
    }

    private void Update()
    {
        if (buildings == null || buttons == null) return;
        var inventory = resourceManager.GetInventory();

        for (int i = 0; i < buildings.Length; i++)
        {
            if (buttons[i] == null) continue;
            BuildingData data = buildings[i];

            bool canAfford =
                (inventory.ContainsKey("Lumber")         ? inventory["Lumber"]         : 0) >= data.costLumber &&
                (inventory.ContainsKey("ProcessedStone")  ? inventory["ProcessedStone"]  : 0) >= data.costProcessedStone &&
                (inventory.ContainsKey("Metal")          ? inventory["Metal"]          : 0) >= data.costMetal;

            CanvasGroup cg = buttons[i].GetComponent<CanvasGroup>();
            if (cg != null)
                cg.alpha = canAfford ? 1f : 0.4f;
        }
    }
}
