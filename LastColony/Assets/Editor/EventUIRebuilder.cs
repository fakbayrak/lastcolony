using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class EventUIRebuilder
{
    [MenuItem("LastColony/Rebuild Event UI")]
    public static void RebuildEventUI()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null) { Debug.LogError("Canvas bulunamadi!"); return; }

        // Eski paneli temizle
        Transform old = canvas.transform.Find("EventPanel");
        if (old != null) Object.DestroyImmediate(old.gameObject);

        // Ana panel
        GameObject panel = new GameObject("EventPanel");
        panel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot     = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(520f, 420f);
        panelRect.anchoredPosition = Vector2.zero;

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.08f, 0.08f, 0.10f, 0.96f);

        // Başlık
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta        = new Vector2(490f, 55f);
        titleRect.anchoredPosition = new Vector2(0f, 170f);
        TMP_Text titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.fontSize  = 26;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color     = new Color(1f, 0.85f, 0.2f);
        titleText.text      = "Baslik";

        // Açıklama
        GameObject descObj = new GameObject("DescriptionText");
        descObj.transform.SetParent(panel.transform, false);
        RectTransform descRect = descObj.AddComponent<RectTransform>();
        descRect.sizeDelta        = new Vector2(480f, 80f);
        descRect.anchoredPosition = new Vector2(0f, 90f);
        TMP_Text descText = descObj.AddComponent<TextMeshProUGUI>();
        descText.fontSize           = 16;
        descText.alignment          = TextAlignmentOptions.Center;
        descText.color              = new Color(0.88f, 0.88f, 0.88f);
        descText.enableWordWrapping = true;
        descText.text               = "Aciklama";

        // Ayraç
        GameObject line = new GameObject("Divider");
        line.transform.SetParent(panel.transform, false);
        RectTransform lineRect = line.AddComponent<RectTransform>();
        lineRect.sizeDelta        = new Vector2(480f, 2f);
        lineRect.anchoredPosition = new Vector2(0f, 42f);
        Image lineImg = line.AddComponent<Image>();
        lineImg.color = new Color(0.4f, 0.4f, 0.4f);

        // Seçenek A butonu
        GameObject btnA = CreateButton("ChoiceAButton", panel.transform,
            new Vector2(-130f, -30f), new Vector2(230f, 110f),
            new Color(0.15f, 0.50f, 0.15f));

        // Seçenek B butonu
        GameObject btnB = CreateButton("ChoiceBButton", panel.transform,
            new Vector2(130f, -30f), new Vector2(230f, 110f),
            new Color(0.55f, 0.15f, 0.10f));

        // Onayla butonu (tek seçenek)
        GameObject btnConfirm = CreateButton("ConfirmButton", panel.transform,
            new Vector2(0f, -30f), new Vector2(300f, 80f),
            new Color(0.20f, 0.40f, 0.65f));
        btnConfirm.GetComponentInChildren<TMP_Text>().text = "Tamam";

        // EventUI script referanslarını güncelle
        EventUI eventUI = Object.FindFirstObjectByType<EventUI>();
        if (eventUI != null)
        {
            SerializedObject so = new SerializedObject(eventUI);
            so.FindProperty("eventPanel").objectReferenceValue       = panel;
            so.FindProperty("titleText").objectReferenceValue        = titleText;
            so.FindProperty("descriptionText").objectReferenceValue  = descText;
            so.FindProperty("choiceAButton").objectReferenceValue    = btnA.GetComponent<Button>();
            so.FindProperty("choiceBButton").objectReferenceValue    = btnB.GetComponent<Button>();
            so.FindProperty("choiceAText").objectReferenceValue      = btnA.GetComponentInChildren<TMP_Text>();
            so.FindProperty("choiceBText").objectReferenceValue      = btnB.GetComponentInChildren<TMP_Text>();
            so.FindProperty("confirmButton").objectReferenceValue    = btnConfirm.GetComponent<Button>();
            so.ApplyModifiedProperties();
        }

        panel.SetActive(false);
        EditorUtility.SetDirty(canvas.gameObject);
        Debug.Log("[EventUIRebuilder] Event UI yeniden kuruldu!");
    }

    static GameObject CreateButton(string name, Transform parent, Vector2 pos, Vector2 size, Color color)
    {
        GameObject btn = new GameObject(name);
        btn.transform.SetParent(parent, false);

        RectTransform rect = btn.AddComponent<RectTransform>();
        rect.sizeDelta        = size;
        rect.anchoredPosition = pos;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot     = new Vector2(0.5f, 0.5f);

        Image img = btn.AddComponent<Image>();
        img.color = color;
        btn.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btn.transform, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.sizeDelta        = new Vector2(size.x - 10f, size.y - 10f);
        textRect.anchoredPosition = Vector2.zero;

        TMP_Text tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.fontSize           = 14;
        tmp.fontStyle          = FontStyles.Bold;
        tmp.alignment          = TextAlignmentOptions.Center;
        tmp.color              = Color.white;
        tmp.enableWordWrapping = true;
        tmp.text               = "Secenek";

        return btn;
    }
}
