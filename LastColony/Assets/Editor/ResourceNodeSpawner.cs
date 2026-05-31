using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ResourceNodeSpawner
{
    [MenuItem("LastColony/Spawn Resource Nodes")]
    public static void SpawnNodes()
    {
        GridManager grid = GameObject.FindFirstObjectByType<GridManager>();
        if (grid == null) { Debug.LogError("[ResourceNodeSpawner] GridManager bulunamadı!"); return; }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Spawn Resource Nodes");
        int undoGroup = Undo.GetCurrentGroup();

        // Eski node'ları temizle
        ResourceNode[] existing = GameObject.FindObjectsByType<ResourceNode>(FindObjectsSortMode.None);
        foreach (ResourceNode n in existing)
            Undo.DestroyObjectImmediate(n.gameObject);

        // Wood node'ları — sol üst, sol orta, üst orta (5 node)
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(2,  15), 100);
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(4,  12), 100);
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(2,   8), 100);
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(7,  17), 100);
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(5,   5), 100);

        // Stone node'ları — sağ kenar ve sağ orta (4 node)
        SpawnNode(grid, ResourceType.Stone,    new Vector2Int(17, 15), 80);
        SpawnNode(grid, ResourceType.Stone,    new Vector2Int(16, 10), 80);
        SpawnNode(grid, ResourceType.Stone,    new Vector2Int(17,  5), 80);
        SpawnNode(grid, ResourceType.Stone,    new Vector2Int(13, 17), 80);

        // MetalOre node'ları — alt orta (2 node, nadir)
        SpawnNode(grid, ResourceType.MetalOre, new Vector2Int(8,   3), 50);
        SpawnNode(grid, ResourceType.MetalOre, new Vector2Int(13,  3), 50);

        Undo.CollapseUndoOperations(undoGroup);
        EditorSceneManager.MarkSceneDirty(grid.gameObject.scene);
        Debug.Log("[ResourceNodeSpawner] 11 ResourceNode oluşturuldu.");
    }

    private static void SpawnNode(GridManager grid, ResourceType type, Vector2Int gridPos, int amount)
    {
        Vector3 worldPos = grid.GridToWorld(gridPos);
        worldPos.y = 0f;

        GameObject root = new GameObject($"ResourceNode_{type}_{gridPos.x}_{gridPos.y}");
        root.transform.position = worldPos;
        Undo.RegisterCreatedObjectUndo(root, "Spawn Resource Node");

        // Görsel oluştur
        switch (type)
        {
            case ResourceType.Wood:     CreateWoodVisual(root.transform);     break;
            case ResourceType.Stone:    CreateStoneVisual(root.transform);    break;
            case ResourceType.MetalOre: CreateMetalOreVisual(root.transform); break;
        }

        // ResourceNode bileşeni ekle
        ResourceNode node = root.AddComponent<ResourceNode>();
        SerializedObject so = new SerializedObject(node);
        SerializedProperty typeProp   = so.FindProperty("resourceType");
        SerializedProperty amountProp = so.FindProperty("totalAmount");
        if (typeProp   != null) typeProp.enumValueIndex = (int)type;
        if (amountProp != null) amountProp.intValue = amount;
        so.ApplyModifiedProperties();
    }

    // ─── Ağaç grubu (Wood) ────────────────────────────────────────────────────
    private static void CreateWoodVisual(Transform parent)
    {
        // 3 ağaç grubu halinde yerleştirilmiş
        Vector3[] offsets = {
            new Vector3(0f,    0f, 0f),
            new Vector3(0.55f, 0f, 0.3f),
            new Vector3(-0.4f, 0f, 0.5f),
        };
        float[] sizes   = { 1.0f, 0.85f, 0.90f };
        float[] browns  = { 0.28f, 0.32f, 0.25f };

        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject tree = new GameObject($"Tree_{i}");
            tree.transform.SetParent(parent);
            tree.transform.localPosition = offsets[i];

            float h = sizes[i];

            // Gövde
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.transform.SetParent(tree.transform);
            trunk.transform.localPosition = new Vector3(0f, 0.3f * h, 0f);
            trunk.transform.localScale    = new Vector3(0.15f, 0.3f * h, 0.15f);
            Object.DestroyImmediate(trunk.GetComponent<Collider>());
            SetColor(trunk, new Color(browns[i], browns[i] * 0.6f, 0.08f));

            // Alt yaprak
            GameObject leavesBot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leavesBot.transform.SetParent(tree.transform);
            leavesBot.transform.localPosition = new Vector3(0f, 0.75f * h, 0f);
            leavesBot.transform.localScale    = new Vector3(0.5f * h, 0.45f * h, 0.5f * h);
            Object.DestroyImmediate(leavesBot.GetComponent<Collider>());
            SetColor(leavesBot, new Color(0.12f + i*0.03f, 0.42f + i*0.04f, 0.10f));

            // Üst yaprak
            GameObject leavesTop = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leavesTop.transform.SetParent(tree.transform);
            leavesTop.transform.localPosition = new Vector3(0f, 1.05f * h, 0f);
            leavesTop.transform.localScale    = new Vector3(0.32f * h, 0.32f * h, 0.32f * h);
            Object.DestroyImmediate(leavesTop.GetComponent<Collider>());
            SetColor(leavesTop, new Color(0.15f + i*0.02f, 0.48f + i*0.03f, 0.12f));
        }
    }

    // ─── Taş kümesi (Stone) ───────────────────────────────────────────────────
    private static void CreateStoneVisual(Transform parent)
    {
        Vector3[] offsets = {
            new Vector3(0f,    0f,    0f),
            new Vector3(0.45f, 0f,    0.2f),
            new Vector3(-0.3f, 0f,    0.4f),
            new Vector3(0.15f, 0f,   -0.35f),
        };
        float[] scales = { 0.55f, 0.40f, 0.35f, 0.30f };
        float[] grays  = { 0.55f, 0.60f, 0.50f, 0.58f };

        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rock.name = $"Rock_{i}";
            rock.transform.SetParent(parent);
            rock.transform.localPosition = offsets[i] + new Vector3(0f, scales[i] * 0.35f, 0f);
            rock.transform.localScale = new Vector3(
                scales[i] * 1.2f,
                scales[i] * 0.7f,
                scales[i] * 1.0f);
            Object.DestroyImmediate(rock.GetComponent<Collider>());
            float g = grays[i];
            SetColor(rock, new Color(g, g, g - 0.05f));
        }
    }

    // ─── Maden ocağı (MetalOre) ───────────────────────────────────────────────
    private static void CreateMetalOreVisual(Transform parent)
    {
        // Zemin kaya — düz büyük kaya
        GameObject baseRock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        baseRock.name = "BaseRock";
        baseRock.transform.SetParent(parent);
        baseRock.transform.localPosition = new Vector3(0f, 0.15f, 0f);
        baseRock.transform.localScale = new Vector3(0.9f, 0.3f, 0.9f);
        Object.DestroyImmediate(baseRock.GetComponent<Collider>());
        SetColor(baseRock, new Color(0.30f, 0.28f, 0.25f));

        // Maden kristalleri — parlak pas/turuncu parçalar
        Vector3[] crystalOffsets = {
            new Vector3(0f,    0.25f,  0f),
            new Vector3(0.25f, 0.20f,  0.15f),
            new Vector3(-0.2f, 0.18f,  0.2f),
        };
        float[] crystalSizes = { 0.28f, 0.20f, 0.18f };
        Color[] crystalColors = {
            new Color(0.65f, 0.35f, 0.10f), // pas turuncu
            new Color(0.55f, 0.30f, 0.12f),
            new Color(0.70f, 0.40f, 0.08f),
        };

        for (int i = 0; i < crystalOffsets.Length; i++)
        {
            GameObject crystal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crystal.name = $"Crystal_{i}";
            crystal.transform.SetParent(parent);
            crystal.transform.localPosition = crystalOffsets[i];
            crystal.transform.localRotation = Quaternion.Euler(
                Random.Range(10f, 40f),
                Random.Range(0f,  90f),
                Random.Range(5f,  30f));
            float s = crystalSizes[i];
            crystal.transform.localScale = new Vector3(s * 0.6f, s * 1.4f, s * 0.6f);
            Object.DestroyImmediate(crystal.GetComponent<Collider>());
            SetColor(crystal, crystalColors[i]);
        }
    }

    private static void SetColor(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) return;
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        mat.SetFloat("_Glossiness", 0.1f);
        rend.sharedMaterial = mat;
    }
}
