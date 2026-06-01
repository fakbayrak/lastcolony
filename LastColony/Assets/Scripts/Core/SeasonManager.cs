using System;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    public enum Season { Summer, Autumn, Winter, Spring }

    [SerializeField] int seasonLengthInDays = 90;

    public Season CurrentSeason { get; private set; } = Season.Summer;
    public float ConsumptionMultiplier { get; private set; } = 1f;

    private float baseProductionMultiplier = 1f;
    public float ProductionMultiplier => baseProductionMultiplier * productionPenalty;

    private float productionPenalty = 1f;
    private int penaltyDaysRemaining = 0;

    int currentSeasonDay;
    int totalDaysPassed;

    public int DayInSeason => currentSeasonDay;

    public static event Action<Season> OnSeasonChanged;
    public static event System.Action OnYearCompleted;

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

    private void OnEnable()  { DayNightCycle.OnDayPassed += OnDayPassedPenalty; }
    private void OnDisable() { DayNightCycle.OnDayPassed -= OnDayPassedPenalty; }

    private void OnDayPassedPenalty(int day)
    {
        if (penaltyDaysRemaining > 0)
        {
            penaltyDaysRemaining--;
            if (penaltyDaysRemaining <= 0)
            {
                productionPenalty = 1f;
                Debug.Log("[SeasonManager] Karantina sona erdi, uretim normale dondu.");
            }
        }
    }

    public void ApplyProductionPenalty(float multiplier, int days)
    {
        productionPenalty = multiplier;
        penaltyDaysRemaining = days;
        Debug.Log($"[SeasonManager] Uretim cezasi: x{multiplier}, {days} gun");
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

            if (CurrentSeason == Season.Summer && totalDaysPassed > seasonLengthInDays)
            {
                OnYearCompleted?.Invoke();
            }
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
        (ConsumptionMultiplier, baseProductionMultiplier) = season switch
        {
            Season.Summer => (1.0f, 1.0f),
            Season.Autumn => (1.2f, 0.8f),
            Season.Winter => (2.0f, 0.5f),
            Season.Spring => (0.8f, 1.2f),
            _ => (1.0f, 1.0f)
        };
    }
}
