using UnityEditor;
using UnityEngine;

public class TarlaPrefabCreator
{
    [MenuItem("LastColony/Create Tarla Prefab")]
    public static void CreateTarlaPrefab()
    {
        GameObject go = new GameObject("Tarla_Prefab");
        go.AddComponent<TarlaVisual>();
        go.AddComponent<TarlaAutoProducer>();

        string path = "Assets/Prefabs/Buildings/Tarla_Prefab.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Tarla_Prefab created at " + path);
    }
}
