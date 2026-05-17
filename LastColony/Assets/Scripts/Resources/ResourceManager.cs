using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Metal
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

        inventory[ResourceType.Wood]  = 50;
        inventory[ResourceType.Stone] = 30;
        inventory[ResourceType.Metal] = 10;
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
}
