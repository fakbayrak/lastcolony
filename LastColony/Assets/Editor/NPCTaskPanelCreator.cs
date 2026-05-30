using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.UI;
using TMPro;

public class NPCTaskPanelCreator
{
    [MenuItem("LastColony/Create NPC Task Panel")]
    public static void CreatePanel()
    {
        // Canvas'ı bul
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null) { Debug.LogError("Canvas bulunamadı!"); return; }

        // Ana panel
        GameObject panel = CreateUIObject("NPCTaskPanel", canvas.transform);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        // Sağ kenarı tamamen kaplasın
        panelRect.anchorMin = new Vector2(1, 0);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 0.5f);
        panelRect.anchoredPosition = new Vector2(0, 0);
        panelRect.sizeDelta = new Vector2(200, 0); // height 0 = stretch

        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0.12f, 0.18f, 0.24f, 0.95f);

        VerticalLayoutGroup vlg = panel.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 4;
        vlg.padding = new RectOffset(8, 8, 8, 8);
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childAlignment = TextAnchor.UpperCenter;

        // ContentSizeFitter kaldırıldı — sabit yükseklik kullanılıyor (sizeDelta.y = 520)
        // ContentSizeFitter csf = panel.AddComponent<ContentSizeFitter>();
        // csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Başlık
        CreateLabel(panel.transform, "NPCPanelTitle", "NPC YÖNETİMİ", 16, Color.white, 30, true);
        CreateLabel(panel.transform, "TotalNPCText", "NPC: 0", 14, Color.white, 25);
        CreateLabel(panel.transform, "AssignedText", "Atanan: 0/0", 13, Color.yellow, 25);
        CreateDivider(panel.transform);

        // Kaynak satırları
        CreateResourceRow(panel.transform, "Wood",   "Odun");
        CreateResourceRow(panel.transform, "Stone",  "Taş");
        CreateResourceRow(panel.transform, "Metal",  "Metal");
        CreateResourceRow(panel.transform, "Rest",   "Dinlenme");

        CreateDivider(panel.transform);

        // Uygula butonu
        GameObject applyBtn = CreateButton(panel.transform, "ApplyButton", "UYGULA", 13);
        LayoutElement applyLE = applyBtn.AddComponent<LayoutElement>();
        applyLE.preferredHeight = 40;
        applyBtn.GetComponent<Image>().color = new Color(0.15f, 0.55f, 0.25f, 1f);

        // Toggle butonu kaldırıldı — panel sağ kenarda sabit kaldığı için gerek yok.

        // === NPCTaskUI component'ini ekle ve referansları otomatik bağla ===
        NPCTaskUI npcTaskUI = panel.AddComponent<NPCTaskUI>();

        NPCManager npcManager = Object.FindFirstObjectByType<NPCManager>();
        TaskManager taskManager = Object.FindFirstObjectByType<TaskManager>();
        if (npcManager == null)
            Debug.LogWarning("[NPCTaskPanelCreator] Sahnede NPCManager bulunamadı; referans boş bırakıldı.");
        if (taskManager == null)
            Debug.LogWarning("[NPCTaskPanelCreator] Sahnede TaskManager bulunamadı; referans boş bırakıldı.");

        // Private [SerializeField] alanlar SerializedObject ile atanır
        SerializedObject so = new SerializedObject(npcTaskUI);
        so.FindProperty("npcManager").objectReferenceValue     = npcManager;
        so.FindProperty("taskManager").objectReferenceValue    = taskManager;
        so.FindProperty("panelRoot").objectReferenceValue      = panel;
        so.FindProperty("totalNPCText").objectReferenceValue   = FindText(panel.transform, "TotalNPCText");
        so.FindProperty("assignedText").objectReferenceValue   = FindText(panel.transform, "AssignedText");
        so.FindProperty("woodCountText").objectReferenceValue  = FindText(panel.transform, "WoodCountText");
        so.FindProperty("stoneCountText").objectReferenceValue = FindText(panel.transform, "StoneCountText");
        so.FindProperty("metalCountText").objectReferenceValue = FindText(panel.transform, "MetalCountText");
        so.FindProperty("restCountText").objectReferenceValue  = FindText(panel.transform, "RestCountText");
        so.FindProperty("applyButton").objectReferenceValue    = FindButton(panel.transform, "ApplyButton");
        so.FindProperty("toggleButton").objectReferenceValue   = null; // ToggleButton kaldırıldı
        so.ApplyModifiedProperties();

        // === Spinner butonlarının onClick olaylarını bağla ===
        BindClick(panel.transform, "WoodAdd",  npcTaskUI.AddWood);
        BindClick(panel.transform, "WoodSub",  npcTaskUI.SubWood);
        BindClick(panel.transform, "StoneAdd", npcTaskUI.AddStone);
        BindClick(panel.transform, "StoneSub", npcTaskUI.SubStone);
        BindClick(panel.transform, "MetalAdd", npcTaskUI.AddMetal);
        BindClick(panel.transform, "MetalSub", npcTaskUI.SubMetal);
        BindClick(panel.transform, "RestAdd",  npcTaskUI.AddRest);
        BindClick(panel.transform, "RestSub",  npcTaskUI.SubRest);

        EditorUtility.SetDirty(npcTaskUI);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("[NPCTaskPanelCreator] Panel oluşturuldu ve referanslar bağlandı.");
        Selection.activeGameObject = panel;
    }

    private static TMP_Text FindText(Transform root, string name)
    {
        GameObject go = FindChild(root, name);
        return go != null ? go.GetComponent<TMP_Text>() : null;
    }

    private static Button FindButton(Transform root, string name)
    {
        GameObject go = FindChild(root, name);
        return go != null ? go.GetComponent<Button>() : null;
    }

    private static void BindClick(Transform root, string buttonName, UnityAction action)
    {
        GameObject go = FindChild(root, buttonName);
        if (go == null)
        {
            Debug.LogWarning($"[NPCTaskPanelCreator] Buton bulunamadı: {buttonName}");
            return;
        }
        Button btn = go.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogWarning($"[NPCTaskPanelCreator] '{buttonName}' üzerinde Button component yok.");
            return;
        }
        UnityEventTools.AddPersistentListener(btn.onClick, action);
    }

    // Parent dahil tüm alt objelerde (inaktif olanlar dahil) isme göre arama
    private static GameObject FindChild(Transform root, string name)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            if (t.name == name)
                return t.gameObject;
        return null;
    }

    private static void CreateResourceRow(Transform parent, string key, string label)
    {
        GameObject row = CreateUIObject($"Row_{key}", parent);
        HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 5;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;

        LayoutElement rowLE = row.AddComponent<LayoutElement>();
        rowLE.preferredHeight = 32;

        // Label
        GameObject lbl = CreateUIObject($"{key}Label", row.transform);
        TMP_Text lblText = lbl.AddComponent<TextMeshProUGUI>();
        lblText.text = label;
        lblText.fontSize = 13;
        lblText.color = Color.white;
        lblText.alignment = TextAlignmentOptions.MidlineLeft;
        LayoutElement lblLE = lbl.AddComponent<LayoutElement>();
        lblLE.preferredWidth = 80;
        lblLE.minWidth = 80;
        lblLE.flexibleWidth = 0;

        // Sub button
        GameObject sub = CreateButton(row.transform, $"{key}Sub", "−", 13);
        LayoutElement subLE = sub.AddComponent<LayoutElement>();
        subLE.preferredWidth = 28;
        subLE.minWidth = 28;
        sub.GetComponent<Image>().color = new Color(0.6f, 0.2f, 0.2f, 1f);

        // Count text
        GameObject count = CreateUIObject($"{key}CountText", row.transform);
        TMP_Text countText = count.AddComponent<TextMeshProUGUI>();
        countText.text = "0";
        countText.fontSize = 16;
        countText.color = Color.white;
        countText.alignment = TextAlignmentOptions.Center;
        LayoutElement countLE = count.AddComponent<LayoutElement>();
        countLE.preferredWidth = 30;
        countLE.minWidth = 30;

        // Add button
        GameObject add = CreateButton(row.transform, $"{key}Add", "+", 13);
        LayoutElement addLE = add.AddComponent<LayoutElement>();
        addLE.preferredWidth = 28;
        addLE.minWidth = 28;
        add.GetComponent<Image>().color = new Color(0.2f, 0.5f, 0.2f, 1f);
    }

    private static void CreateDivider(Transform parent)
    {
        GameObject div = CreateUIObject("Divider", parent);
        Image img = div.AddComponent<Image>();
        img.color = new Color(0.4f, 0.5f, 0.6f, 0.5f);
        LayoutElement le = div.AddComponent<LayoutElement>();
        le.preferredHeight = 2;
    }

    private static GameObject CreateLabel(Transform parent, string name,
        string text, int fontSize, Color color, float height, bool bold = false)
    {
        GameObject obj = CreateUIObject(name, parent);
        TMP_Text tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        if (bold) tmp.fontStyle = FontStyles.Bold;
        LayoutElement le = obj.AddComponent<LayoutElement>();
        le.preferredHeight = height;
        return obj;
    }

    private static GameObject CreateButton(Transform parent, string name,
        string text, int fontSize)
    {
        GameObject obj = CreateUIObject(name, parent);
        Image img = obj.AddComponent<Image>();
        img.color = new Color(0.25f, 0.35f, 0.45f, 1f);
        Button btn = obj.AddComponent<Button>();

        GameObject txtObj = CreateUIObject("Text", obj.transform);
        RectTransform txtRect = txtObj.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        TMP_Text tmp = txtObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

        return obj;
    }

    private static GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        obj.AddComponent<RectTransform>();
        return obj;
    }
}
