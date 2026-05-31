using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ProcessingUI : MonoBehaviour
{
    public static ProcessingUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Butonlar")]
    [SerializeField] private Button woodButton;
    [SerializeField] private Button stoneButton;
    [SerializeField] private Button metalOreButton;

    [Header("Bilgi Yazıları")]
    [SerializeField] private TMP_Text woodInfo;
    [SerializeField] private TMP_Text stoneInfo;
    [SerializeField] private TMP_Text metalOreInfo;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        panel.SetActive(false);

        woodButton.onClick.AddListener(() => TryProcess(ResourceType.Wood));
        stoneButton.onClick.AddListener(() => TryProcess(ResourceType.Stone));
        metalOreButton.onClick.AddListener(() => TryProcess(ResourceType.MetalOre));
    }

    private void Update()
    {
        if (!panel.activeSelf) return;
        RefreshInfo();

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            HidePanel();
    }

    private void TryProcess(ResourceType type)
    {
        bool success = ResourceChain.Instance.Process(type);
        if (!success)
            Debug.Log($"[ProcessingUI] Yeterli kaynak yok: {type}");
    }

    private void RefreshInfo()
    {
        int wood      = ResourceManager.Instance.GetResource(ResourceType.Wood);
        int stone     = ResourceManager.Instance.GetResource(ResourceType.Stone);
        int metalOre  = ResourceManager.Instance.GetResource(ResourceType.MetalOre);
        int lumber    = ResourceManager.Instance.GetResource(ResourceType.Lumber);
        int procStone = ResourceManager.Instance.GetResource(ResourceType.ProcessedStone);
        int metal     = ResourceManager.Instance.GetResource(ResourceType.Metal);

        woodInfo.text     = $"Odun ({wood}) → Kereste ({lumber})\n10 Odun = 5 Kereste";
        stoneInfo.text    = $"Taş ({stone}) → İşlenmiş Taş ({procStone})\n10 Taş = 5 İşlenmiş Taş";
        metalOreInfo.text = $"Maden ({metalOre}) → Metal ({metal})\n5 Maden = 3 Metal";

        woodButton.interactable     = ResourceChain.Instance.CanProcess(ResourceType.Wood);
        stoneButton.interactable    = ResourceChain.Instance.CanProcess(ResourceType.Stone);
        metalOreButton.interactable = ResourceChain.Instance.CanProcess(ResourceType.MetalOre);
    }

    public void ShowPanel() => panel.SetActive(true);
    public void HidePanel() => panel.SetActive(false);
    public void TogglePanel()
    {
        if (panel.activeSelf) HidePanel();
        else ShowPanel();
    }
}
