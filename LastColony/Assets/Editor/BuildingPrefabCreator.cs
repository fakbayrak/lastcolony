using UnityEngine;
using UnityEditor;

public class BuildingPrefabCreator
{
    [MenuItem("LastColony/Create Building Prefabs")]
    public static void CreateAllBuildingPrefabs()
    {
        // Kule_Prefab dokunulmuyor — TowerVisual zaten mevcut.
        CreateBuildingPrefab<BarakaVisual>("Baraka_Prefab");
        CreateBuildingPrefab<DepoVisual>("Depo_Prefab");
        CreateBuildingPrefab<AtolyeVisual>("Atolye_Prefab");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[BuildingPrefabCreator] 3 bina prefabı (Baraka, Depo, Atolye) 3D görselle oluşturuldu.");
    }

    private static void CreateBuildingPrefab<T>(string prefabName) where T : Component
    {
        GameObject root = new GameObject(prefabName);

        // 3D hacimli görsel component'i — child objeleri runtime'da Awake'te oluşturur
        root.AddComponent<T>();

        // Tıklama / yerleştirme için collider
        BoxCollider col = root.AddComponent<BoxCollider>();
        col.size   = new Vector3(1f, 1f, 1f);
        col.center = new Vector3(0f, 0.5f, 0f);

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Buildings"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Buildings");

        string prefabPath = $"Assets/Prefabs/Buildings/{prefabName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);

        Debug.Log($"[BuildingPrefabCreator] {prefabName} oluşturuldu → {prefabPath}");
    }
}
