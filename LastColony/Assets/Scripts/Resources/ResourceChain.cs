using System.Collections.Generic;
using UnityEngine;

public class ResourceChain : MonoBehaviour
{
    public static ResourceChain Instance { get; private set; }

    private struct Recipe
    {
        public ResourceType output;
        public int inputCost;
        public int outputAmount;
    }

    private Dictionary<ResourceType, Recipe> recipes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        recipes = new Dictionary<ResourceType, Recipe>
        {
            { ResourceType.Wood,     new Recipe { output = ResourceType.Lumber,        inputCost = 10, outputAmount = 5 } },
            { ResourceType.Stone,    new Recipe { output = ResourceType.ProcessedStone, inputCost = 10, outputAmount = 5 } },
            { ResourceType.MetalOre, new Recipe { output = ResourceType.Metal,          inputCost = 5,  outputAmount = 3 } },
        };
    }

    public bool CanProcess(ResourceType rawType)
    {
        if (!recipes.TryGetValue(rawType, out Recipe recipe))
            return false;

        return ResourceManager.Instance.HasEnough(rawType, recipe.inputCost);
    }

    public bool Process(ResourceType rawType)
    {
        if (!recipes.TryGetValue(rawType, out Recipe recipe))
            return false;

        if (!ResourceManager.Instance.HasEnough(rawType, recipe.inputCost))
            return false;

        ResourceManager.Instance.RemoveResource(rawType, recipe.inputCost);
        ResourceManager.Instance.AddResource(recipe.output, recipe.outputAmount);

        Debug.Log($"[ResourceChain] {recipe.inputCost}x {rawType} → {recipe.outputAmount}x {recipe.output}");

        return true;
    }
}
