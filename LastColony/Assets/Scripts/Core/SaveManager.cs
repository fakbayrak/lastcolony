using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class NPCSaveData
{
    public float health;
    public float hunger;
    public float energy;
    public int gridX;
    public int gridY;
}

[System.Serializable]
public class BuildingSaveData
{
    public int gridX;
    public int gridY;
    public string buildingType;
    public int tier;
}

[System.Serializable]
public class ResourceSaveData
{
    public List<string> keys = new List<string>();
    public List<int> values = new List<int>();
}

[System.Serializable]
public class GameSaveData
{
    public int currentDay;
    public int currentSeason;
    public int dayInSeason;
    public ResourceSaveData resources = new ResourceSaveData();
    public List<NPCSaveData> npcs = new List<NPCSaveData>();
    public List<BuildingSaveData> buildings = new List<BuildingSaveData>();
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private SeasonManager seasonManager;

    private string SavePath => Path.Combine(Application.persistentDataPath, "lastcolony_save.json");

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

    private void OnEnable()
    {
        DayNightCycle.OnDayPassed += OnDayPassed;
    }

    private void OnDisable()
    {
        DayNightCycle.OnDayPassed -= OnDayPassed;
    }

    private void OnDayPassed(int day)
    {
        SaveGame();
    }

    public void SaveGame()
    {
        GameSaveData data = new GameSaveData();

        data.currentDay = dayNightCycle.CurrentDay;
        data.currentSeason = (int)seasonManager.CurrentSeason;
        data.dayInSeason = seasonManager.DayInSeason;

        Dictionary<string, int> inventory = resourceManager.GetInventory();
        foreach (var kvp in inventory)
        {
            data.resources.keys.Add(kvp.Key);
            data.resources.values.Add(kvp.Value);
        }

        foreach (NPC npc in npcManager.GetAllNPCs())
        {
            if (npc == null) continue;
            Vector2Int gridPos = GridManager.Instance.WorldToGrid(npc.transform.position);
            data.npcs.Add(new NPCSaveData
            {
                health = npc.Health,
                hunger = npc.Hunger,
                energy = npc.Energy,
                gridX = gridPos.x,
                gridY = gridPos.y
            });
        }

        foreach (PlacedBuildingRecord record in buildingPlacement.GetPlacedBuildings())
        {
            data.buildings.Add(new BuildingSaveData
            {
                gridX = record.gridX,
                gridY = record.gridY,
                buildingType = record.buildingType,
                tier = record.tier
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveManager] Oyun kaydedildi → {SavePath}");
    }

    public bool LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveManager] Kayıt dosyası bulunamadı.");
            return false;
        }

        string json = File.ReadAllText(SavePath);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

        for (int i = 0; i < data.resources.keys.Count; i++)
        {
            string key = data.resources.keys[i];
            int value = data.resources.values[i];
            resourceManager.SetResource(key, value);
        }

        dayNightCycle.SetDay(data.currentDay);
        seasonManager.SetSeason((SeasonManager.Season)data.currentSeason, data.dayInSeason);

        foreach (BuildingSaveData b in data.buildings)
        {
            buildingPlacement.LoadBuildingRecord(new PlacedBuildingRecord
            {
                gridX = b.gridX,
                gridY = b.gridY,
                buildingType = b.buildingType,
                tier = b.tier
            });
        }

        Debug.Log($"[SaveManager] Gün {data.currentDay}, Mevsim {(SeasonManager.Season)data.currentSeason} yüklendi.");
        return true;
    }

    public bool HasSaveFile() => File.Exists(SavePath);

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
