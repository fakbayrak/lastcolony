using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColonyManager : MonoBehaviour
{
    public static ColonyManager Instance { get; private set; }

    [SerializeField] private NPCManager npcManager;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private TimeController timeController;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;

    private bool colonyCollapsed = false;
    private float hungerDamageInterval = 5f;
    private float hungerDamageTimer = 0f;
    private float hungerDamageAmount = 10f;
    private float starvationThreshold = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (colonyCollapsed) return;
        if (timeController != null && timeController.IsPaused) return;

        hungerDamageTimer += Time.deltaTime;
        if (hungerDamageTimer >= hungerDamageInterval)
        {
            hungerDamageTimer = 0f;
            ApplyStarvationDamage();
        }

        CheckColonyStatus();
    }

    private void ApplyStarvationDamage()
    {
        var allNPCs = npcManager.GetAllNPCs();
        foreach (NPC npc in allNPCs)
        {
            if (npc == null) continue;
            if (npc.Hunger < starvationThreshold)
            {
                npc.TakeDamage(hungerDamageAmount);
                Debug.Log($"[ColonyManager] NPC aç kalıyor, hasar aldı. Sağlık: {npc.Health}");
            }
        }
    }

    private void CheckColonyStatus()
    {
        var allNPCs = npcManager.GetAllNPCs();
        if (allNPCs == null || allNPCs.Count == 0) return;

        int aliveCount = 0;
        foreach (NPC npc in allNPCs)
        {
            if (npc != null && npc.Health > 0f)
                aliveCount++;
        }

        if (aliveCount == 0 && !colonyCollapsed)
            TriggerColonyCollapse();
    }

    private void TriggerColonyCollapse()
    {
        colonyCollapsed = true;
        Debug.Log("[ColonyManager] Koloni çöktü.");

        if (timeController != null)
            timeController.Pause();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "KOLONİ ÇÖKTÜ\n\nTüm koloniciler hayatını kaybetti.";
    }

    public bool IsCollapsed => colonyCollapsed;
    public void ResetCollapse() { colonyCollapsed = false; }
}
