using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class RaidWarningUICreator
{
    [MenuItem("LastColony/Create Raid Warning UI")]
    public static void CreateRaidWarningUI()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[RaidWarningUICreator] Canvas bulunamadı!");
            return;
        }

        // Eski paneli temizle
        Transform old = canvas.transform.Find("RaidWarningPanel");
        if (old != null) Object.DestroyImmediate(old.gameObject);
        Transform oldUI = canvas.transform.Find("RaidWarningUI");
        if (oldUI != null) Object.DestroyImmediate(oldUI.gameObject);

        // Panel — ekranın üst ortasında
        GameObject panel = new GameObject("RaidWarningPanel");
        panel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.pivot     = new Vector2(0.5f, 1f);
        panelRect.sizeDelta = new Vector2(500f, 80f);
        panelRect.anchoredPosition = new Vector2(0f, -10f);

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.6f, 0.05f, 0.05f, 0.85f);

        // Kırmızı çerçeve efekti için outline
        Outline outline = panel.AddComponent<Outline>();
        outline.effectColor    = new Color(1f, 0f, 0f, 0.8f);
        outline.effectDistance = new Vector2(3f, 3f);

        // Uyarı metni
        GameObject textObj = new GameObject("WarningText");
        textObj.transform.SetParent(panel.transform, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.sizeDelta        = new Vector2(480f, 70f);
        textRect.anchoredPosition = Vector2.zero;

        TMP_Text text = textObj.AddComponent<TextMeshProUGUI>();
        text.text      = "!! BASKIN !!";
        text.fontSize  = 36;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color     = new Color(1f, 0.9f, 0.1f);

        // RaidWarningUI script
        GameObject uiObj = new GameObject("RaidWarningUI");
        uiObj.transform.SetParent(canvas.transform, false);
        RaidWarningUI raidUI = uiObj.AddComponent<RaidWarningUI>();

        SerializedObject so = new SerializedObject(raidUI);
        so.FindProperty("warningPanel").objectReferenceValue = panel;
        so.FindProperty("warningText").objectReferenceValue  = text;
        so.ApplyModifiedProperties();

        panel.SetActive(false);

        EditorUtility.SetDirty(canvas.gameObject);
        Debug.Log("[RaidWarningUICreator] Raid Warning UI kurulumu tamamlandı!");
    }
}
