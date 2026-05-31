using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class BuildingInfoUICreator
{
    [MenuItem("LastColony/Create Building Info UI")]
    public static void CreateBuildingInfoUI()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[BuildingInfoUICreator] Sahnede Canvas bulunamadı!");
            return;
        }

        // Eski paneli temizle
        Transform old = canvas.transform.Find("BuildingInfoPanel");
        if (old != null) Object.DestroyImmediate(old.gameObject);

        Transform oldUI = canvas.transform.Find("BuildingInfoUI");
        if (oldUI != null) Object.DestroyImmediate(oldUI.gameObject);

        // Panel
        GameObject panel = new GameObject("BuildingInfoPanel");
        panel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(320, 280);
        panelRect.anchoredPosition = new Vector2(200, 0);
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.08f, 0.08f, 0.08f, 0.93f);

        // Bina adı
        GameObject nameObj = new GameObject("BuildingNameText");
        nameObj.transform.SetParent(panel.transform, false);
        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.sizeDelta = new Vector2(300, 40);
        nameRect.anchoredPosition = new Vector2(0, 100);
        TMP_Text nameText = nameObj.AddComponent<TextMeshProUGUI>();
        nameText.text = "Bina Adı";
        nameText.fontSize = 18;
        nameText.fontStyle = FontStyles.Bold;
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.color = new Color(1f, 0.85f, 0.3f);

        // Açıklama
        GameObject descObj = new GameObject("DescriptionText");
        descObj.transform.SetParent(panel.transform, false);
        RectTransform descRect = descObj.AddComponent<RectTransform>();
        descRect.sizeDelta = new Vector2(290, 60);
        descRect.anchoredPosition = new Vector2(0, 45);
        TMP_Text descText = descObj.AddComponent<TextMeshProUGUI>();
        descText.text = "Açıklama";
        descText.fontSize = 12;
        descText.color = new Color(0.8f, 0.8f, 0.8f);
        descText.alignment = TextAlignmentOptions.Center;
        descText.textWrappingMode = TextWrappingModes.Normal;

        // Ayraç çizgi
        GameObject line = new GameObject("Divider");
        line.transform.SetParent(panel.transform, false);
        RectTransform lineRect = line.AddComponent<RectTransform>();
        lineRect.sizeDelta = new Vector2(280, 2);
        lineRect.anchoredPosition = new Vector2(0, 10);
        Image lineImage = line.AddComponent<Image>();
        lineImage.color = new Color(0.4f, 0.4f, 0.4f);

        // Detay yazısı
        GameObject detailObj = new GameObject("DetailText");
        detailObj.transform.SetParent(panel.transform, false);
        RectTransform detailRect = detailObj.AddComponent<RectTransform>();
        detailRect.sizeDelta = new Vector2(290, 90);
        detailRect.anchoredPosition = new Vector2(0, -45);
        TMP_Text detailText = detailObj.AddComponent<TextMeshProUGUI>();
        detailText.text = "Detay";
        detailText.fontSize = 12;
        detailText.color = new Color(0.9f, 0.9f, 0.9f);
        detailText.alignment = TextAlignmentOptions.Center;
        detailText.textWrappingMode = TextWrappingModes.Normal;

        // Kapat butonu
        GameObject closeBtn = new GameObject("CloseButton");
        closeBtn.transform.SetParent(panel.transform, false);
        RectTransform closeRect = closeBtn.AddComponent<RectTransform>();
        closeRect.sizeDelta = new Vector2(80, 28);
        closeRect.anchoredPosition = new Vector2(0, -108);
        Image closeImage = closeBtn.AddComponent<Image>();
        closeImage.color = new Color(0.55f, 0.12f, 0.12f);
        Button closeComp = closeBtn.AddComponent<Button>();

        GameObject closeTxt = new GameObject("Text");
        closeTxt.transform.SetParent(closeBtn.transform, false);
        RectTransform closeTxtRect = closeTxt.AddComponent<RectTransform>();
        closeTxtRect.sizeDelta = new Vector2(80, 28);
        closeTxtRect.anchoredPosition = Vector2.zero;
        TMP_Text closeTMP = closeTxt.AddComponent<TextMeshProUGUI>();
        closeTMP.text = "Kapat";
        closeTMP.fontSize = 13;
        closeTMP.alignment = TextAlignmentOptions.Center;
        closeTMP.color = Color.white;

        // Yükselt butonu
        GameObject upgradeBtn = new GameObject("UpgradeButton");
        upgradeBtn.transform.SetParent(panel.transform, false);
        RectTransform upgradeRect = upgradeBtn.AddComponent<RectTransform>();
        upgradeRect.sizeDelta = new Vector2(200, 45);
        upgradeRect.anchoredPosition = new Vector2(0, -160);
        Image upgradeImage = upgradeBtn.AddComponent<Image>();
        upgradeImage.color = new Color(0.953f, 0.612f, 0.071f); // #F39C12
        Button upgradeComp = upgradeBtn.AddComponent<Button>();

        GameObject upgradeTxt = new GameObject("Text");
        upgradeTxt.transform.SetParent(upgradeBtn.transform, false);
        RectTransform upgradeTxtRect = upgradeTxt.AddComponent<RectTransform>();
        upgradeTxtRect.sizeDelta = new Vector2(200, 45);
        upgradeTxtRect.anchoredPosition = Vector2.zero;
        TMP_Text upgradeTMP = upgradeTxt.AddComponent<TextMeshProUGUI>();
        upgradeTMP.text = "YÜKSELt";
        upgradeTMP.fontSize = 16;
        upgradeTMP.fontStyle = FontStyles.Bold;
        upgradeTMP.alignment = TextAlignmentOptions.Center;
        upgradeTMP.color = Color.white;

        // Maliyet yazısı
        GameObject costObj = new GameObject("CostText");
        costObj.transform.SetParent(panel.transform, false);
        RectTransform costRect = costObj.AddComponent<RectTransform>();
        costRect.sizeDelta = new Vector2(220, 35);
        costRect.anchoredPosition = new Vector2(0, -215);
        TMP_Text costTMP = costObj.AddComponent<TextMeshProUGUI>();
        costTMP.text = "Maliyet: -";
        costTMP.fontSize = 12;
        costTMP.alignment = TextAlignmentOptions.Center;
        costTMP.color = new Color(0.925f, 0.941f, 0.945f); // #ECF0F1

        // Maksimum seviye yazısı
        GameObject maxObj = new GameObject("MaxLevelText");
        maxObj.transform.SetParent(panel.transform, false);
        RectTransform maxRect = maxObj.AddComponent<RectTransform>();
        maxRect.sizeDelta = new Vector2(200, 45);
        maxRect.anchoredPosition = new Vector2(0, -160);
        TMP_Text maxTMP = maxObj.AddComponent<TextMeshProUGUI>();
        maxTMP.text = "✓ MAKSİMUM SEVİYE";
        maxTMP.fontSize = 14;
        maxTMP.fontStyle = FontStyles.Bold;
        maxTMP.alignment = TextAlignmentOptions.Center;
        maxTMP.color = new Color(0.945f, 0.769f, 0.059f); // #F1C40F
        maxObj.SetActive(false);

        // BuildingInfoUI script
        GameObject uiObj = new GameObject("BuildingInfoUI");
        uiObj.transform.SetParent(canvas.transform, false);
        BuildingInfoUI buildingInfoUI = uiObj.AddComponent<BuildingInfoUI>();

        SerializedObject so = new SerializedObject(buildingInfoUI);
        so.FindProperty("panel").objectReferenceValue            = panel;
        so.FindProperty("buildingNameText").objectReferenceValue = nameText;
        so.FindProperty("descriptionText").objectReferenceValue  = descText;
        so.FindProperty("detailText").objectReferenceValue       = detailText;
        so.FindProperty("closeButton").objectReferenceValue      = closeComp;
        so.FindProperty("upgradeButton").objectReferenceValue    = upgradeComp;
        so.FindProperty("upgradeButtonText").objectReferenceValue = upgradeTMP;
        so.FindProperty("costText").objectReferenceValue         = costTMP;
        so.FindProperty("maxLevelText").objectReferenceValue     = maxTMP;
        so.ApplyModifiedProperties();

        closeComp.onClick.AddListener(() => buildingInfoUI.HidePanel());
        panel.SetActive(false);

        // BuildingClickDetector sahnеye ekle
        GameObject existingDetector = GameObject.Find("BuildingClickDetector");
        if (existingDetector != null) Object.DestroyImmediate(existingDetector);

        GameObject detectorObj = new GameObject("BuildingClickDetector");
        BuildingClickDetector detector = detectorObj.AddComponent<BuildingClickDetector>();

        BuildingPlacement placement = Object.FindFirstObjectByType<BuildingPlacement>();
        Camera cam = Camera.main;

        SerializedObject detectorSO = new SerializedObject(detector);
        detectorSO.FindProperty("buildingPlacement").objectReferenceValue = placement;
        detectorSO.FindProperty("mainCamera").objectReferenceValue = cam;

        // BuildingData asset'lerini bul ve ata
        string[] guids = AssetDatabase.FindAssets("t:BuildingData");
        BuildingData[] allData = new BuildingData[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            allData[i] = AssetDatabase.LoadAssetAtPath<BuildingData>(path);
        }

        SerializedProperty listProp = detectorSO.FindProperty("buildingDataList");
        listProp.arraySize = allData.Length;
        for (int i = 0; i < allData.Length; i++)
            listProp.GetArrayElementAtIndex(i).objectReferenceValue = allData[i];

        detectorSO.ApplyModifiedProperties();

        EditorUtility.SetDirty(canvas.gameObject);
        Debug.Log($"[BuildingInfoUICreator] Kurulum tamamlandı! {allData.Length} BuildingData bulundu.");
    }
}
