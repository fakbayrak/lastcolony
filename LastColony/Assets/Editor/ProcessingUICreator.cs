using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ProcessingUICreator
{
    [MenuItem("LastColony/Create Processing UI")]
    public static void CreateProcessingUI()
    {
        // Canvas bul
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[ProcessingUICreator] Sahnede Canvas bulunamadı!");
            return;
        }

        // Eski paneli temizle
        Transform old = canvas.transform.Find("ProcessingPanel");
        if (old != null) Object.DestroyImmediate(old.gameObject);

        Transform oldUI = canvas.transform.Find("ProcessingUI");
        if (oldUI != null) Object.DestroyImmediate(oldUI.gameObject);

        // ProcessingPanel oluştur
        GameObject panel = new GameObject("ProcessingPanel");
        panel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(400, 320);
        panelRect.anchoredPosition = new Vector2(0, 0);
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.92f);

        // Başlık
        GameObject title = new GameObject("TitleText");
        title.transform.SetParent(panel.transform, false);
        RectTransform titleRect = title.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(380, 40);
        titleRect.anchoredPosition = new Vector2(0, 130);
        TMP_Text titleText = title.AddComponent<TextMeshProUGUI>();
        titleText.text = "⚙ İşleme Atölyesi";
        titleText.fontSize = 18;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.white;

        // 3 satır: her biri Info + Button
        string[] labels    = { "WoodRow",     "StoneRow",     "MetalOreRow"  };
        string[] infoNames = { "WoodInfo",    "StoneInfo",    "MetalOreInfo" };
        string[] btnNames  = { "WoodButton",  "StoneButton",  "MetalOreButton" };
        float[]  yPos      = { 70f, 0f, -70f };

        GameObject[] infoObjects   = new GameObject[3];
        GameObject[] buttonObjects = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            // Satır container
            GameObject row = new GameObject(labels[i]);
            row.transform.SetParent(panel.transform, false);
            RectTransform rowRect = row.AddComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(380, 55);
            rowRect.anchoredPosition = new Vector2(0, yPos[i]);

            // Info yazısı
            GameObject info = new GameObject(infoNames[i]);
            info.transform.SetParent(row.transform, false);
            RectTransform infoRect = info.AddComponent<RectTransform>();
            infoRect.sizeDelta = new Vector2(270, 50);
            infoRect.anchoredPosition = new Vector2(-50, 0);
            TMP_Text infoText = info.AddComponent<TextMeshProUGUI>();
            infoText.fontSize = 13;
            infoText.color = new Color(0.9f, 0.9f, 0.9f);
            infoText.alignment = TextAlignmentOptions.MidlineLeft;
            infoText.text = "...";
            infoObjects[i] = info;

            // Buton
            GameObject btn = new GameObject(btnNames[i]);
            btn.transform.SetParent(row.transform, false);
            RectTransform btnRect = btn.AddComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(80, 40);
            btnRect.anchoredPosition = new Vector2(155, 0);
            Image btnImage = btn.AddComponent<Image>();
            btnImage.color = new Color(0.2f, 0.6f, 0.2f);
            Button btnComp = btn.AddComponent<Button>();

            GameObject btnText = new GameObject("Text");
            btnText.transform.SetParent(btn.transform, false);
            RectTransform btnTextRect = btnText.AddComponent<RectTransform>();
            btnTextRect.sizeDelta = new Vector2(80, 40);
            btnTextRect.anchoredPosition = Vector2.zero;
            TMP_Text btnTMP = btnText.AddComponent<TextMeshProUGUI>();
            btnTMP.text = "İşle";
            btnTMP.fontSize = 14;
            btnTMP.fontStyle = FontStyles.Bold;
            btnTMP.alignment = TextAlignmentOptions.Center;
            btnTMP.color = Color.white;

            buttonObjects[i] = btn;
        }

        // Kapat butonu
        GameObject closeBtn = new GameObject("CloseButton");
        closeBtn.transform.SetParent(panel.transform, false);
        RectTransform closeRect = closeBtn.AddComponent<RectTransform>();
        closeRect.sizeDelta = new Vector2(80, 30);
        closeRect.anchoredPosition = new Vector2(0, -130);
        Image closeImage = closeBtn.AddComponent<Image>();
        closeImage.color = new Color(0.6f, 0.15f, 0.15f);
        Button closeComp = closeBtn.AddComponent<Button>();

        GameObject closeTxt = new GameObject("Text");
        closeTxt.transform.SetParent(closeBtn.transform, false);
        RectTransform closeTxtRect = closeTxt.AddComponent<RectTransform>();
        closeTxtRect.sizeDelta = new Vector2(80, 30);
        closeTxtRect.anchoredPosition = Vector2.zero;
        TMP_Text closeTMP = closeTxt.AddComponent<TextMeshProUGUI>();
        closeTMP.text = "Kapat";
        closeTMP.fontSize = 13;
        closeTMP.alignment = TextAlignmentOptions.Center;
        closeTMP.color = Color.white;

        // ProcessingUI GameObject
        GameObject uiObj = new GameObject("ProcessingUI");
        uiObj.transform.SetParent(canvas.transform, false);
        ProcessingUI processingUI = uiObj.AddComponent<ProcessingUI>();

        // Referansları bağla (SerializedObject ile)
        SerializedObject so = new SerializedObject(processingUI);
        so.FindProperty("panel").objectReferenceValue           = panel;
        so.FindProperty("woodButton").objectReferenceValue      = buttonObjects[0].GetComponent<Button>();
        so.FindProperty("stoneButton").objectReferenceValue     = buttonObjects[1].GetComponent<Button>();
        so.FindProperty("metalOreButton").objectReferenceValue  = buttonObjects[2].GetComponent<Button>();
        so.FindProperty("woodInfo").objectReferenceValue        = infoObjects[0].GetComponent<TMP_Text>();
        so.FindProperty("stoneInfo").objectReferenceValue       = infoObjects[1].GetComponent<TMP_Text>();
        so.FindProperty("metalOreInfo").objectReferenceValue    = infoObjects[2].GetComponent<TMP_Text>();
        so.ApplyModifiedProperties();

        // Kapat butonuna listener
        closeComp.onClick.AddListener(() => processingUI.HidePanel());

        // Panel başta kapalı
        panel.SetActive(false);

        // Atölye prefabına AtolyeClickHandler ekle
        string prefabPath = "Assets/Prefabs/Buildings/Atolye_Prefab.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null)
        {
            AtolyeClickHandler existing = prefab.GetComponent<AtolyeClickHandler>();
            if (existing == null)
            {
                using (var scope = new PrefabUtility.EditPrefabContentsScope(prefabPath))
                {
                    scope.prefabContentsRoot.AddComponent<AtolyeClickHandler>();
                }
                Debug.Log("[ProcessingUICreator] AtolyeClickHandler prefaba eklendi.");
            }
            else
            {
                Debug.Log("[ProcessingUICreator] AtolyeClickHandler zaten mevcut.");
            }
        }
        else
        {
            Debug.LogWarning($"[ProcessingUICreator] Prefab bulunamadı: {prefabPath}");
        }

        EditorUtility.SetDirty(canvas.gameObject);
        Debug.Log("[ProcessingUICreator] ProcessingUI kurulumu tamamlandı!");
    }
}
