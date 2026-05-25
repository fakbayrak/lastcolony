using System;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    public enum Season { Summer, Autumn, Winter, Spring }

    [SerializeField] int seasonLengthInDays = 90;

    public Season CurrentSeason { get; private set; } = Season.Summer;
    public float ConsumptionMultiplier { get; private set; } = 1f;
    public float ProductionMultiplier { get; private set; } = 1f;

    int currentSeasonDay;
    int totalDaysPassed;

    public int DayInSeason => currentSeasonDay;

    public static event Action<Season> OnSeasonChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyMultipliers(CurrentSeason);
    }

    public void OnDayPassed()
    {
        totalDaysPassed++;
        currentSeasonDay++;

        if (totalDaysPassed % 10 == 0)
            Debug.Log($"Gün {totalDaysPassed} — Mevsim: {CurrentSeason}, Tüketim: {ConsumptionMultiplier}, Üretim: {ProductionMultiplier}");

        if (currentSeasonDay >= seasonLengthInDays)
        {
            currentSeasonDay = 0;
            CurrentSeason = NextSeason(CurrentSeason);
            ApplyMultipliers(CurrentSeason);
            Debug.Log($"Mevsim değişti: {CurrentSeason}, Gün: {totalDaysPassed}");
            OnSeasonChanged?.Invoke(CurrentSeason);
        }
    }

    public void SetSeason(Season season, int dayInSeason)
    {
        CurrentSeason = season;
        currentSeasonDay = dayInSeason;
        ApplyMultipliers(CurrentSeason);
    }

    Season NextSeason(Season season) => season switch
    {
        Season.Summer => Season.Autumn,
        Season.Autumn => Season.Winter,
        Season.Winter => Season.Spring,
        Season.Spring => Season.Summer,
        _ => Season.Summer
    };

    void ApplyMultipliers(Season season)
    {
        (ConsumptionMultiplier, ProductionMultiplier) = season switch
        {
            Season.Summer => (1.0f, 1.0f),
            Season.Autumn => (1.2f, 0.8f),
            Season.Winter => (2.0f, 0.5f),
            Season.Spring => (0.8f, 1.2f),
            _ => (1.0f, 1.0f)
        };
    }
}
