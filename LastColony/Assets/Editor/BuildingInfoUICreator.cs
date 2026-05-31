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
        panelRect.sizeDelta = new Vector2(320, 260);
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
        descText.enableWordWrapping = true;

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
        detailText.enableWordWrapping = true;

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
