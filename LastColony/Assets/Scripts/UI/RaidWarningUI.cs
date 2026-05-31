using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaidWarningUI : MonoBehaviour
{
    public static RaidWarningUI Instance { get; private set; }

    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMP_Text warningText;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        warningPanel.SetActive(false);
        SpawnManager.OnRaidStarted += ShowWarning;
    }

    private void OnDestroy()
    {
        SpawnManager.OnRaidStarted -= ShowWarning;
    }

    private void ShowWarning()
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        warningPanel.SetActive(true);
        hideCoroutine = StartCoroutine(HideAfterDelay(4f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += Time.deltaTime;
            // Yanıp sönme efekti
            float alpha = Mathf.Abs(Mathf.Sin(elapsed * 4f));
            if (warningText != null)
                warningText.alpha = alpha;
            yield return null;
        }
        warningPanel.SetActive(false);
    }
}
