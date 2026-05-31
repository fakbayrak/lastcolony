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
        // 1. Salgın Hastalık
        eventPool.Add(new GameEvent
        {
            Title = "Salgin Hastalik",
            Description = "Kolonide hastalik yayiliyor.\n3 kolonici yataklik durumda.",
            RequiresChoice = true,
            ChoiceAText = "Hastaneye kaynak harca\n(Wood -20, salgin onlenir)",
            ChoiceBText = "Karantina ilan et\n(uretim 3 gun %50 yavaslar)",
            OnChoiceA = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Wood, 20);
                Debug.Log("[Event] Salgin onlendi: Wood -20");
            },
            OnChoiceB = () =>
            {
                SeasonManager.Instance.ApplyProductionPenalty(0.5f, 3);
                Debug.Log("[Event] Karantina: uretim 3 gun %50 yavaslar");
            }
        });

        // 2. Büyük Fırtına
        eventPool.Add(new GameEvent
        {
            Title = "Buyuk Firtina",
            Description = "Siddetli bir firtina kaynaklari tahrip etti.\nDepolarin bir kismi zarar gordu.",
            RequiresChoice = false,
            OnAutoResolve = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Wood, 20);
                ResourceManager.Instance.RemoveResource(ResourceType.Stone, 10);
                Debug.Log("[Event] Firtina: Wood -20, Stone -10");
            }
        });

        // 3. Depo Çöküşü
        eventPool.Add(new GameEvent
        {
            Title = "Depo Cokusu",
            Description = "Eski depo coktu.\nIslenmis malzeme kayboldu.",
            RequiresChoice = false,
            OnAutoResolve = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Lumber, 15);
                ResourceManager.Instance.RemoveResource(ResourceType.ProcessedStone, 10);
                Debug.Log("[Event] Depo cokusu: Lumber -15, ProcessedStone -10");
            }
        });

        // 4. Yabancı Gezgin
        eventPool.Add(new GameEvent
        {
            Title = "Yabanci Gezgin",
            Description = "Kapina bir gezgin geldi.\nNe yapacaksin?",
            RequiresChoice = true,
            ChoiceAText = "Kabul et\n(1 yeni kolonici katilir)",
            ChoiceBText = "Reddet\n(Stone +15 al, guvende kal)",
            OnChoiceA = () =>
            {
                SpawnNewNPC();
                Debug.Log("[Event] Yeni kolonici katildi!");
            },
            OnChoiceB = () =>
            {
                ResourceManager.Instance.AddResource(ResourceType.Stone, 15);
                Debug.Log("[Event] Gezgin reddedildi: Stone +15");
            }
        });

        // 5. Göçmen Kafile (YENİ)
        eventPool.Add(new GameEvent
        {
            Title = "Gocmen Kafile",
            Description = "Uzak diyarlardan bir kafile geldi.\nKolonine katilmak istiyorlar.",
            RequiresChoice = true,
            ChoiceAText = "Kabul et\n(3 yeni kolonici, Wood -30)",
            ChoiceBText = "Reddet\n(kafile devam eder)",
            OnChoiceA = () =>
            {
                bool hasWood = ResourceManager.Instance.HasEnough(ResourceType.Wood, 30);
                if (hasWood)
                {
                    ResourceManager.Instance.RemoveResource(ResourceType.Wood, 30);
                    SpawnNewNPC();
                    SpawnNewNPC();
                    SpawnNewNPC();
                    Debug.Log("[Event] Gocmen kafile kabul edildi: 3 NPC eklendi, Wood -30");
                }
                else
                {
                    SpawnNewNPC();
                    Debug.Log("[Event] Yeterli kaynak yok, sadece 1 NPC katildi");
                }
            },
            OnChoiceB = () =>
            {
                Debug.Log("[Event] Gocmen kafile reddedildi");
            }
        });

        // 6. Altın Fırsat (YENİ)
        eventPool.Add(new GameEvent
        {
            Title = "Altin Firsat",
            Description = "Gezgin bir tüccar değerli mallar\ntaşıyor. Ne alacaksın?",
            RequiresChoice = true,
            ChoiceAText = "Metal al\n(Wood -25, Metal +15)",
            ChoiceBText = "Kereste al\n(Stone -20, Lumber +20)",
            OnChoiceA = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Wood, 25);
                ResourceManager.Instance.AddResource(ResourceType.Metal, 15);
                Debug.Log("[Event] Tuccar: Wood -25, Metal +15");
            },
            OnChoiceB = () =>
            {
                ResourceManager.Instance.RemoveResource(ResourceType.Stone, 20);
                ResourceManager.Instance.AddResource(ResourceType.Lumber, 20);
                Debug.Log("[Event] Tuccar: Stone -20, Lumber +20");
            }
        });
    }

    void SpawnNewNPC()
    {
        if (NPCManager.Instance == null) return;

        // Grid merkezine yakın rastgele boş hücre bul
        for (int attempt = 0; attempt < 20; attempt++)
        {
            int x = UnityEngine.Random.Range(7, 13);
            int z = UnityEngine.Random.Range(7, 13);

            if (!GridManager.Instance.IsCellOccupied(x, z))
            {
                Vector3 spawnPos = GridManager.Instance.GridToWorld(x, z);
                spawnPos.y = 0f;

                GameObject npcPrefab = Resources.Load<GameObject>("NPC_Prefab");
                if (npcPrefab == null)
                {
                    // Prefab Resources'ta değilse mevcut NPC'lerden birini bul ve klonla
                    var existingNPCs = NPCManager.Instance.GetAllNPCs();
                    if (existingNPCs.Count > 0)
                    {
                        GameObject clone = UnityEngine.Object.Instantiate(
                            existingNPCs[0].gameObject, spawnPos, Quaternion.identity);
                        clone.name = $"NPC_Event_{System.DateTime.Now.Ticks}";

                        NPC npc = clone.GetComponent<NPC>();
                        if (npc != null) npc.SetIdle();
                        return;
                    }
                }
                else
                {
                    GameObject npcObj = UnityEngine.Object.Instantiate(npcPrefab, spawnPos, Quaternion.identity);
                    npcObj.AddComponent<NPCVisual>();
                    return;
                }
            }
        }
        Debug.LogWarning("[EventManager] SpawnNewNPC: Uygun hucre bulunamadi");
    }
}
