using UnityEngine;
using System.Collections.Generic;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Material groundMaterial;

    [Header("Zemin Renkleri")]
    [SerializeField] private Color grassColor      = new Color(0.35f, 0.55f, 0.25f);
    [SerializeField] private Color dirtColor       = new Color(0.55f, 0.42f, 0.28f);
    [SerializeField] private Color forestGrassColor = new Color(0.20f, 0.40f, 0.15f);
    [SerializeField] private Color riverColor      = new Color(0.20f, 0.45f, 0.70f);
    [SerializeField] private Color riverEdgeColor  = new Color(0.25f, 0.50f, 0.60f);
    [SerializeField] private float noiseScale      = 3f;

    [Header("Çevre Ayarları")]
    [SerializeField] private int borderSize        = 8;
    [SerializeField] private float treeNoiseCutoff = 0.55f;
    [SerializeField] private float treeNoiseScale  = 2.5f;

    private int gridWidth;
    private int gridHeight;

    // Grid dünya sınırları
    private float gridMinX;
    private float gridMaxX;
    private float gridMinZ;
    private float gridMaxZ;

    private void Start()
    {
        gridWidth  = gridManager.Width;
        gridHeight = gridManager.Height;

        // GridManager koordinat sistemi: GridToWorld(x,y) = (x+0.5, 0, y+0.5)
        gridMinX = 0f;
        gridMaxX = gridWidth;
        gridMinZ = 0f;
        gridMaxZ = gridHeight;

        GenerateGround();
        GenerateBorder();
        GenerateRiver();
        GenerateTrees();
    }

    // ─── Grid zemini ───────────────────────────────────────────────────────────
    private void GenerateGround()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 worldPos = gridManager.GridToWorld(new Vector2Int(x, z));
                CreateQuad($"Tile_{x}_{z}", worldPos.x, worldPos.z, 0.98f, -0.01f,
                    GetGridTileColor(x, z), transform);
            }
        }
    }

    private Color GetGridTileColor(int x, int z)
    {
        float noise = Mathf.PerlinNoise((x + 100f) / noiseScale, (z + 100f) / noiseScale);
        return Color.Lerp(dirtColor, grassColor, noise);
    }

    // ─── Grid dışı zemin kuşağı ────────────────────────────────────────────────
    private void GenerateBorder()
    {
        GameObject borderParent = new GameObject("BorderGround");
        borderParent.transform.SetParent(transform);

        int startX = -borderSize;
        int endX   = gridWidth  + borderSize;
        int startZ = -borderSize;
        int endZ   = gridHeight + borderSize;

        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                // Grid içini atla
                if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
                    continue;

                float wx = x + 0.5f;
                float wz = z + 0.5f;

                float noise = Mathf.PerlinNoise((x + 200f) / noiseScale, (z + 200f) / noiseScale);
                Color c = Color.Lerp(forestGrassColor, new Color(0.15f, 0.32f, 0.10f), noise);

                CreateQuad($"Border_{x}_{z}", wx, wz, 1.0f, -0.01f, c, borderParent.transform);
            }
        }
    }

    // ─── Dere (grid sol kenarının dışında, x = -1 ile -6 arası) ───────────────
    private void GenerateRiver()
    {
        GameObject riverParent = new GameObject("River");
        riverParent.transform.SetParent(transform);

        // Dere: x = -6 ile -1 arasında, tüm z boyunca kıvrımlı
        int riverStartX = -6;
        int riverEndX   = -1;

        for (int z = -borderSize; z < gridHeight + borderSize; z++)
        {
            // Perlin noise ile kıvrım: her z satırında genişlik ve offset değişir
            float offset = Mathf.PerlinNoise(0f, (z + 50f) / 4f) * 1.5f;
            int localStart = riverStartX + Mathf.RoundToInt(offset);
            int localEnd   = riverEndX   + Mathf.RoundToInt(offset * 0.5f);

            for (int x = localStart; x <= localEnd; x++)
            {
                float wx = x + 0.5f;
                float wz = z + 0.5f;

                // Kenar mı orta mı?
                bool isEdge = (x == localStart || x == localEnd);
                Color c = isEdge ? riverEdgeColor : riverColor;

                // Hafif dalgalanma için y yüksekliği
                float wave = Mathf.Sin(wz * 1.5f + wx * 0.5f) * 0.02f;

                CreateQuad($"River_{x}_{z}", wx, wz, 1.0f, -0.005f + wave, c, riverParent.transform);
            }
        }
    }

    // ─── Ağaçlar (grid dışı) ──────────────────────────────────────────────────
    private void GenerateTrees()
    {
        GameObject treeParent = new GameObject("Trees");
        treeParent.transform.SetParent(transform);

        int startX = -borderSize;
        int endX   = gridWidth  + borderSize;
        int startZ = -borderSize;
        int endZ   = gridHeight + borderSize;

        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                // Grid içini atla
                if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
                    continue;

                // Dere alanını atla (x = -8 ile -1)
                if (x >= -8 && x <= -1)
                    continue;

                float noise = Mathf.PerlinNoise((x + 300f) / treeNoiseScale, (z + 300f) / treeNoiseScale);
                if (noise < treeNoiseCutoff) continue;

                // Rastgele varyasyon için seed
                float rand = Mathf.PerlinNoise(x * 7.3f, z * 3.7f);
                if (rand < 0.35f) continue; // Seyrekleştir

                float wx = x + 0.5f + (rand - 0.5f) * 0.4f;
                float wz = z + 0.5f + (Mathf.PerlinNoise(x * 5f, z * 5f) - 0.5f) * 0.4f;

                CreateTree(wx, wz, rand, treeParent.transform);
            }
        }
    }

    private void CreateTree(float wx, float wz, float rand, Transform parent)
    {
        GameObject treeRoot = new GameObject("Tree");
        treeRoot.transform.SetParent(parent);
        treeRoot.transform.position = new Vector3(wx, 0f, wz);

        float heightMult = 0.7f + rand * 0.8f;

        // Gövde (Cylinder)
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.name = "Trunk";
        trunk.transform.SetParent(treeRoot.transform);
        trunk.transform.localPosition = new Vector3(0f, 0.4f * heightMult, 0f);
        trunk.transform.localScale    = new Vector3(0.18f, 0.4f * heightMult, 0.18f);
        Destroy(trunk.GetComponent<Collider>());

        float trunkBrown = 0.3f + rand * 0.2f;
        SetMaterialColor(trunk, new Color(trunkBrown, trunkBrown * 0.6f, 0.1f));

        // Yapraklar — 2 katman Sphere
        float leafSize1 = 0.55f + rand * 0.35f;
        float leafSize2 = leafSize1 * 0.65f;

        Color leafColor1 = new Color(0.10f + rand * 0.12f, 0.38f + rand * 0.18f, 0.08f);
        Color leafColor2 = new Color(0.08f + rand * 0.10f, 0.45f + rand * 0.15f, 0.10f);

        GameObject leaves1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaves1.name = "Leaves1";
        leaves1.transform.SetParent(treeRoot.transform);
        leaves1.transform.localPosition = new Vector3(0f, 0.9f * heightMult, 0f);
        leaves1.transform.localScale    = new Vector3(leafSize1, leafSize1 * 1.1f, leafSize1);
        Destroy(leaves1.GetComponent<Collider>());
        SetMaterialColor(leaves1, leafColor1);

        GameObject leaves2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaves2.name = "Leaves2";
        leaves2.transform.SetParent(treeRoot.transform);
        leaves2.transform.localPosition = new Vector3(0f, 1.25f * heightMult, 0f);
        leaves2.transform.localScale    = new Vector3(leafSize2, leafSize2, leafSize2);
        Destroy(leaves2.GetComponent<Collider>());
        SetMaterialColor(leaves2, leafColor2);
    }

    // ─── Yardımcı metodlar ────────────────────────────────────────────────────
    private void CreateQuad(string name, float wx, float wz, float scale, float y, Color color, Transform parent)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = name;
        quad.transform.SetParent(parent);
        quad.transform.position = new Vector3(wx, y, wz);
        quad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        quad.transform.localScale = Vector3.one * scale;
        Destroy(quad.GetComponent<MeshCollider>());
        SetMaterialColor(quad, color);
    }

    private void SetMaterialColor(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) return;

        if (groundMaterial != null)
        {
            Material mat = new Material(groundMaterial);
            mat.color = color;
            rend.material = mat;
        }
        else
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            rend.material = mat;
        }
    }
}
