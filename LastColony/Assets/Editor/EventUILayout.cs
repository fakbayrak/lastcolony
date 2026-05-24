using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class EventUILayout
{
    [MenuItem("LastColony/Setup Event UI Layout")]
    static void SetupLayout()
    {
        SetupPanel();
        SetupTitleText();
        SetupDescriptionText();
        SetupChoiceAButton();
        SetupChoiceBButton();
        SetupConfirmButton();

        Debug.Log("EventUI layout applied.");
    }

    static void SetupPanel()
    {
        var panel = GameObject.Find("EventPanel");
        if (panel == null) { Debug.LogError("EventUILayout: 'EventPanel' bulunamadı."); return; }

        var rt = panel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(500, 350);
        rt.anchoredPosition = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);

        var image = panel.GetComponent<Image>();
        if (image != null)
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);
    }

    static void SetupTitleText()
    {
        var obj = GameObject.Find("TitleText");
        if (obj == null) { Debug.LogError("EventUILayout: 'TitleText' bulunamadı."); return; }

        var rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(460, 50);
        rt.anchoredPosition = new Vector2(0, 130);

        var tmp = obj.GetComponent<TextMeshProUGUI>();
        if (tmp == null) return;
        tmp.fontSize = 24;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    static void SetupDescriptionText()
    {
        var obj = GameObject.Find("DescriptionText");
        if (obj == null) { Debug.LogError("EventUILayout: 'DescriptionText' bulunamadı."); return; }

        var rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(460, 80);
        rt.anchoredPosition = new Vector2(0, 50);

        var tmp = obj.GetComponent<TextMeshProUGUI>();
        if (tmp == null) return;
        tmp.fontSize = 16;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;
    }

    static void SetupChoiceAButton()
    {
        var obj = GameObject.Find("ChoiceAButton");
        if (obj == null) { Debug.LogError("EventUILayout: 'ChoiceAButton' bulunamadı."); return; }

        var rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 50);
        rt.anchoredPosition = new Vector2(-110, -100);

        var image = obj.GetComponent<Image>();
        if (image != null)
            image.color = new Color(0.2f, 0.5f, 0.2f, 1f);
    }

    static void SetupChoiceBButton()
    {
        var obj = GameObject.Find("ChoiceBButton");
        if (obj == null) { Debug.LogError("EventUILayout: 'ChoiceBButton' bulunamadı."); return; }

        var rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 50);
        rt.anchoredPosition = new Vector2(110, -100);

        var image = obj.GetComponent<Image>();
        if (image != null)
            image.color = new Color(0.5f, 0.2f, 0.2f, 1f);
    }

    static void SetupConfirmButton()
    {
        var obj = GameObject.Find("ConfirmButton");
        if (obj == null) { Debug.LogError("EventUILayout: 'ConfirmButton' bulunamadı."); return; }

        var rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 50);
        rt.anchoredPosition = new Vector2(0, -100);

        var image = obj.GetComponent<Image>();
        if (image != null)
            image.color = new Color(0.2f, 0.3f, 0.5f, 1f);
    }
}
