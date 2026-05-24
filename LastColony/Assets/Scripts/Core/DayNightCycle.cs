using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    [SerializeField] float dayDurationSeconds = 60f;
    [SerializeField] Light directionalLight;

    public float CurrentDayProgress { get; private set; }
    public int CurrentDay { get; private set; } = 1;
    public bool IsNight => CurrentDayProgress >= 0.65f;

    public static event Action<int> OnDayPassed;
    public static event Action OnNightBegin;

    bool nightEventFired;
    bool isPaused;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        TimeController.OnPauseStateChanged += OnPauseChanged;
    }

    void OnDestroy()
    {
        TimeController.OnPauseStateChanged -= OnPauseChanged;
    }

    void OnPauseChanged(bool paused) => isPaused = paused;

    void Update()
    {
        if (isPaused) return;

        CurrentDayProgress += Time.deltaTime / dayDurationSeconds;

        UpdateLight();
        CheckNightBegin();

        if (CurrentDayProgress >= 1f)
        {
            CurrentDayProgress = 0f;
            nightEventFired = false;
            CurrentDay++;
            SeasonManager.Instance?.OnDayPassed();
            OnDayPassed?.Invoke(CurrentDay);
        }
    }

    void UpdateLight()
    {
        if (directionalLight == null) return;

        float t = CurrentDayProgress;

        if (t < 0.5f)
        {
            directionalLight.intensity = 1f;
            directionalLight.color = new Color(1f, 1f, 0.9f);
        }
        else if (t < 0.65f)
        {
            float blend = (t - 0.5f) / 0.15f;
            directionalLight.intensity = Mathf.Lerp(1f, 0.05f, blend);
            directionalLight.color = Color.Lerp(
                new Color(1f, 1f, 0.9f),
                new Color(0.1f, 0.1f, 0.3f),
                blend
            );
        }
        else
        {
            directionalLight.intensity = 0.05f;
            directionalLight.color = new Color(0.1f, 0.1f, 0.3f);
        }
    }

    void CheckNightBegin()
    {
        if (!nightEventFired && CurrentDayProgress >= 0.65f)
        {
            nightEventFired = true;
            OnNightBegin?.Invoke();
        }
    }
}
