using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCTaskUI : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private TaskManager taskManager;

    [Header("Panel")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text totalNPCText;
    [SerializeField] private TMP_Text assignedText;

    [Header("Spinner Değerleri")]
    [SerializeField] private TMP_Text woodCountText;
    [SerializeField] private TMP_Text stoneCountText;
    [SerializeField] private TMP_Text metalCountText;
    [SerializeField] private TMP_Text restCountText;

    [Header("Butonlar")]
    [SerializeField] private Button applyButton;
    [SerializeField] private Button toggleButton;

    private int woodCount = 0;
    private int stoneCount = 0;
    private int metalCount = 0;
    private int restCount = 0;
    private bool isPanelOpen = true;

    private void Start()
    {
        UpdateUI();
        applyButton.onClick.AddListener(ApplyTasks);
        toggleButton.onClick.AddListener(TogglePanel);
    }

    private void Update()
    {
        int total = npcManager.GetAllNPCs().Count;
        if (totalNPCText != null)
            totalNPCText.text = $"NPC: {total}";

        int assigned = woodCount + stoneCount + metalCount + restCount;
        if (assignedText != null)
        {
            assignedText.text = $"Atanan: {assigned}/{total}";
            assignedText.color = assigned > total
                ? Color.red
                : Color.white;
        }
    }

    // Wood spinner
    public void AddWood()  { woodCount++;  ClampAll(); UpdateUI(); }
    public void SubWood()  { woodCount = Mathf.Max(0, woodCount - 1); UpdateUI(); }

    // Stone spinner
    public void AddStone() { stoneCount++; ClampAll(); UpdateUI(); }
    public void SubStone() { stoneCount = Mathf.Max(0, stoneCount - 1); UpdateUI(); }

    // Metal spinner
    public void AddMetal() { metalCount++; ClampAll(); UpdateUI(); }
    public void SubMetal() { metalCount = Mathf.Max(0, metalCount - 1); UpdateUI(); }

    // Rest spinner
    public void AddRest()  { restCount++;  ClampAll(); UpdateUI(); }
    public void SubRest()  { restCount = Mathf.Max(0, restCount - 1); UpdateUI(); }

    private void ClampAll()
    {
        int total = npcManager.GetAllNPCs().Count;
        int assigned = woodCount + stoneCount + metalCount + restCount;
        if (assigned > total)
        {
            // Son arttırılanı geri al
            int excess = assigned - total;
            if (woodCount > 0 && excess > 0)  { woodCount  -= excess; excess = 0; }
            if (stoneCount > 0 && excess > 0) { stoneCount -= excess; excess = 0; }
            if (metalCount > 0 && excess > 0) { metalCount -= excess; excess = 0; }
            if (restCount > 0 && excess > 0)  { restCount  -= excess; }
        }
    }

    private void UpdateUI()
    {
        if (woodCountText  != null) woodCountText.text  = woodCount.ToString();
        if (stoneCountText != null) stoneCountText.text = stoneCount.ToString();
        if (metalCountText != null) metalCountText.text = metalCount.ToString();
        if (restCountText  != null) restCountText.text  = restCount.ToString();
    }

    private void ApplyTasks()
    {
        var npcs = npcManager.GetAllNPCs();
        int idx = 0;

        // Odun toplayıcılar
        for (int i = 0; i < woodCount && idx < npcs.Count; i++, idx++)
            npcManager.AssignGatherTask(npcs[idx], "Wood");

        // Taş toplayıcılar
        for (int i = 0; i < stoneCount && idx < npcs.Count; i++, idx++)
            npcManager.AssignGatherTask(npcs[idx], "Stone");

        // Metal toplayıcılar
        for (int i = 0; i < metalCount && idx < npcs.Count; i++, idx++)
            npcManager.AssignGatherTask(npcs[idx], "MetalOre");

        // Dinlenenler
        for (int i = 0; i < restCount && idx < npcs.Count; i++, idx++)
            npcs[idx].SetIdle();

        // Kalan NPC'ler Idle
        while (idx < npcs.Count)
        {
            npcs[idx].SetIdle();
            idx++;
        }

        Debug.Log($"[NPCTaskUI] Görevler atandı — Odun:{woodCount} Taş:{stoneCount} Metal:{metalCount} Dinlenme:{restCount}");
    }

    private void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        // Toggle button hariç her şeyi gizle/göster
        Transform[] children = panelRoot.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in children)
        {
            if (t.gameObject != toggleButton.gameObject &&
                t.gameObject != panelRoot)
            {
                t.gameObject.SetActive(isPanelOpen);
            }
        }
    }
}
