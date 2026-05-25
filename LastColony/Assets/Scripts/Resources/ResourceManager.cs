using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    // Ham maddeler
    Wood, Stone, MetalOre,
    // İşlenmiş maddeler
    Lumber, ProcessedStone, Metal
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event Action<ResourceType, int> OnResourceChanged;

    private Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        inventory[ResourceType.Wood]          = 50;
        inventory[ResourceType.Stone]         = 30;
        inventory[ResourceType.MetalOre]      = 10;
        inventory[ResourceType.Lumber]        = 0;
        inventory[ResourceType.ProcessedStone] = 0;
        inventory[ResourceType.Metal]         = 0;
    }

    public void AddResource(ResourceType type, int amount)
    {
        inventory[type] += amount;
        OnResourceChanged?.Invoke(type, inventory[type]);
    }

    public bool RemoveResource(ResourceType type, int amount)
    {
        if (inventory[type] < amount)
            return false;

        inventory[type] -= amount;
        OnResourceChanged?.Invoke(type, inventory[type]);
        return true;
    }

    public int GetResource(ResourceType type)
    {
        return inventory[type];
    }

    public bool HasEnough(ResourceType type, int amount)
    {
        return inventory[type] >= amount;
    }

    public Dictionary<string, int> GetInventory()
    {
        var result = new Dictionary<string, int>();
        foreach (var kvp in inventory)
            result[kvp.Key.ToString()] = kvp.Value;
        return result;
    }

    public void SetResource(string key, int amount)
    {
        if (System.Enum.TryParse<ResourceType>(key, out ResourceType type))
        {
            inventory[type] = amount;
            OnResourceChanged?.Invoke(type, amount);
        }
    }
}
