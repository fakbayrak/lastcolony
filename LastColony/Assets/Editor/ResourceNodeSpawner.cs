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

        // Tüm işlemi tek bir Undo grubunda topla (Ctrl+Z ile geri alınabilir)
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Spawn Resource Nodes");
        int undoGroup = Undo.GetCurrentGroup();

        // Eski node'ları temizle
        ResourceNode[] existing = GameObject.FindObjectsByType<ResourceNode>(FindObjectsSortMode.None);
        foreach (ResourceNode n in existing)
            Undo.DestroyObjectImmediate(n.gameObject);

        // Wood node'ları — sol taraf
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(3, 5),  50);
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(3, 7),  50);
        SpawnNode(grid, ResourceType.Wood,     new Vector2Int(4, 9),  50);

        // Stone node'ları — sağ taraf
        SpawnNode(grid, ResourceType.Stone,    new Vector2Int(15, 5), 40);
        SpawnNode(grid, ResourceType.Stone,    new Vector2Int(15, 7), 40);

        // MetalOre node'ları — üst taraf
        SpawnNode(grid, ResourceType.MetalOre, new Vector2Int(10, 3), 30);
        SpawnNode(grid, ResourceType.MetalOre, new Vector2Int(12, 3), 30);

        Undo.CollapseUndoOperations(undoGroup);

        // Sahneyi "dirty" işaretle ki oluşturulan node'lar kaydedilebilsin
        EditorSceneManager.MarkSceneDirty(grid.gameObject.scene);

        Debug.Log("[ResourceNodeSpawner] 7 ResourceNode oluşturuldu.");
    }

    private static void SpawnNode(GridManager grid, ResourceType type, Vector2Int gridPos, int amount)
    {
        Vector3 worldPos = grid.GridToWorld(gridPos);
        worldPos.y = 0.5f;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = $"ResourceNode_{type}_{gridPos.x}_{gridPos.y}";
        go.transform.position = worldPos;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Undo.RegisterCreatedObjectUndo(go, "Spawn Resource Node");

        // Renge göre ayırt et
        Renderer rend = go.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = type == ResourceType.Wood  ? new Color(0.4f, 0.25f, 0.1f) :
                    type == ResourceType.Stone ? new Color(0.6f, 0.6f, 0.6f) :
                                                 new Color(0.3f, 0.5f, 0.3f);
        rend.sharedMaterial = mat;

        ResourceNode node = go.AddComponent<ResourceNode>();

        // ResourceNode'un private alanlarını SerializedObject ile set et.
        // NOT: resourceType bir enum (string değil), bu yüzden enumValueIndex kullanılıyor.
        SerializedObject so = new SerializedObject(node);

        SerializedProperty typeProp = so.FindProperty("resourceType");
        if (typeProp != null) typeProp.enumValueIndex = (int)type;

        SerializedProperty amountProp = so.FindProperty("totalAmount");
        if (amountProp != null) amountProp.intValue = amount;

        so.ApplyModifiedProperties();
    }
}
