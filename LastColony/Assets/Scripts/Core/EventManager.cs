using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public string Title;
    public string Description;
    public bool RequiresChoice;
    public string ChoiceAText;
    public string ChoiceBText;
    public Action OnChoiceA;
    public Action OnChoiceB;
    public Action OnAutoResolve;
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] int minDaysBetweenEvents = 4;
    [SerializeField] int maxDaysBetweenEvents = 8;

    public static event Action<GameEvent> OnEventTriggered;

    List<GameEvent> eventPool = new List<GameEvent>();
    Queue<GameEvent> eventQueue = new Queue<GameEvent>();

    bool isEventActive;
    int nextEventDay;
    GameEvent activeEvent;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildEventPool();
        nextEventDay = UnityEngine.Random.Range(minDaysBetweenEvents, maxDaysBetweenEvents);
    }

    void OnEnable()
    {
        DayNightCycle.OnDayPassed += HandleDayPassed;
    }

    void OnDestroy()
    {
        DayNightCycle.OnDayPassed -= HandleDayPassed;
    }

    void HandleDayPassed(int currentDay)
    {
        if (isEventActive || currentDay < nextEventDay) return;

        activeEvent = eventPool[UnityEngine.Random.Range(0, eventPool.Count)];
        isEventActive = true;
        nextEventDay = currentDay + UnityEngine.Random.Range(minDaysBetweenEvents, maxDaysBetweenEvents);
        OnEventTriggered?.Invoke(activeEvent);
    }

    public void ResolveChoiceA()
    {
        activeEvent?.OnChoiceA?.Invoke();
        isEventActive = false;
    }

    public void ResolveChoiceB()
    {
        activeEvent?.OnChoiceB?.Invoke();
        isEventActive = false;
    }

    public void ResolveAuto()
    {
        activeEvent?.OnAutoResolve?.Invoke();
        isEventActive = false;
    }

    void BuildEventPool()
    {
        eventPool.Add(new GameEvent
        {
            Title = "Salgın Hastalık",
            Description = "Kolonide hastalık yayılıyor. 3 kolonici yataklık.",
            RequiresChoice = true,
            ChoiceAText = "Hastaneye kaynak harca (Wood -20, acil tedavi)",
            ChoiceBText = "Karantina ilan et (üretim yavaşlar)",
            OnChoiceA = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Wood, 20);
                Debug.Log("Salgın Hastalık: Wood -20 harcandı, salgın önlendi.");
            },
            OnChoiceB = () =>
            {
                Debug.Log("Karantina: üretim yüzde 50 yavaşladı — TODO: etki eklenecek");
            }
        });

        eventPool.Add(new GameEvent
        {
            Title = "Büyük Fırtına",
            Description = "Şiddetli bir fırtına kaynakları tahrip etti.",
            RequiresChoice = false,
            OnAutoResolve = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Wood, 20);
                ResourceManager.Instance.RemoveResource(ResourceType.Stone, 10);
                Debug.Log("Büyük Fırtına: Wood -20, Stone -10 kayboldu.");
            }
        });

        eventPool.Add(new GameEvent
        {
            Title = "Depo Çöküşü",
            Description = "Eski depo çöktü, işlenmiş malzeme kayboldu.",
            RequiresChoice = false,
            OnAutoResolve = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Lumber, 15);
                ResourceManager.Instance.RemoveResource(ResourceType.ProcessedStone, 10);
                Debug.Log("Depo Çöküşü: Lumber -15, ProcessedStone -10 kayboldu.");
            }
        });

        eventPool.Add(new GameEvent
        {
            Title = "Yabancı Gezgin",
            Description = "Kapına bir gezgin geldi. Ne yapacaksın?",
            RequiresChoice = true,
            ChoiceAText = "Kabul et (yeni iş gücü kazanırsın)",
            ChoiceBText = "Reddet (Stone +15 al, güvende kal)",
            OnChoiceA = () =>
            {
                Debug.Log("Yeni kolonici katıldı — TODO: spawn eklenecek");
            },
            OnChoiceB = () =>
            {
                ResourceManager.Instance.AddResource(ResourceType.Stone, 15);
                Debug.Log("Yabancı Gezgin reddedildi: Stone +15 alındı.");
            }
        });
    }
}
