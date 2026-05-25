using UnityEngine;
using UnityEditor;

public class BuildingPrefabCreator
{
    [MenuItem("LastColony/Create Building Prefabs")]
    public static void CreateAllBuildingPrefabs()
    {
        CreateBuildingPrefab(
            "Baraka_Prefab",
            "Assets/Sprites/Buildings/baraka_transparent.png",
            new Vector3(3f, 3f, 1f)
        );
        CreateBuildingPrefab(
            "Depo_Prefab",
            "Assets/Sprites/Buildings/wooden_storage_barn_transparent.png",
            new Vector3(3f, 3f, 1f)
        );
        CreateBuildingPrefab(
            "Atolye_Prefab",
            "Assets/Sprites/Buildings/workshop_transparent.png",
            new Vector3(3f, 3f, 1f)
        );
        CreateBuildingPrefab(
            "Kule_Prefab",
            "Assets/Sprites/Buildings/defense_tower_transparent.png",
            new Vector3(2f, 4f, 1f)
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[BuildingPrefabCreator] 4 bina prefabı oluşturuldu.");
    }

    private static void CreateBuildingPrefab(string prefabName, string spritePath, Vector3 scale)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath);
        if (texture == null)
            Debug.LogWarning($"[BuildingPrefabCreator] Texture bulunamadı: {spritePath}");

        GameObject root = new GameObject(prefabName);

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = "Visual";
        quad.transform.SetParent(root.transform);
        quad.transform.localPosition = Vector3.zero;
        quad.transform.localRotation = Quaternion.Euler(90f, 45f, 0f);
        quad.transform.localScale = scale;

        Object.DestroyImmediate(quad.GetComponent<MeshCollider>());

        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = Color.white;
        if (texture != null)
            mat.mainTexture = texture;

        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");

        string matPath = $"Assets/Materials/{prefabName}_Mat.mat";
        AssetDatabase.CreateAsset(mat, matPath);

        quad.GetComponent<Renderer>().material = mat;

        BoxCollider col = root.AddComponent<BoxCollider>();
        col.size = new Vector3(1f, 0.1f, 1f);
        col.center = Vector3.zero;

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Buildings"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Buildings");

        string prefabPath = $"Assets/Prefabs/Buildings/{prefabName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);

        Debug.Log($"[BuildingPrefabCreator] {prefabName} oluşturuldu → {prefabPath}");
    }
}
