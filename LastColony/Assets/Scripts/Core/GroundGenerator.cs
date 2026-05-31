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
    [SerializeField] private int borderSize        = 26;
    [SerializeField] private float treeNoiseCutoff = 0.40f;
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

        int riverLeft  = -7;
        int riverRight = -2;
        int zStart = -borderSize;
        int zEnd   = gridHeight + borderSize;

        for (int z = zStart; z < zEnd; z++)
        {
            // Zemin tabanı (koyu mavi)
            for (int x = riverLeft - 1; x <= riverRight + 1; x++)
            {
                CreateQuad($"RiverBase_{x}_{z}", x + 0.5f, z + 0.5f,
                    1.02f, -0.03f, new Color(0.10f, 0.25f, 0.50f), riverParent.transform);
            }

            // Su yüzeyi
            for (int x = riverLeft; x <= riverRight; x++)
            {
                float wave = Mathf.Sin(z * 0.8f) * 0.01f;
                bool isEdge = (x == riverLeft || x == riverRight);
                Color c = isEdge
                    ? new Color(0.25f, 0.52f, 0.68f)
                    : new Color(0.18f, 0.44f, 0.72f);
                CreateQuad($"River_{x}_{z}", x + 0.5f, z + 0.5f,
                    1.0f, 0.01f + wave, c, riverParent.transform);
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

                // Dere alanını atla
                if (x >= -9 && x <= -1)
                    continue;

                float rand = Mathf.PerlinNoise(x * 7.3f, z * 3.7f);

                // Grid'e yakın kenarlarda (1-3 birim) seyrek ağaç
                bool nearGridEdge =
                    (x >= -3 && x < 0) ||
                    (x >= gridWidth && x < gridWidth + 3) ||
                    (z >= -3 && z < 0) ||
                    (z >= gridHeight && z < gridHeight + 3);

                if (nearGridEdge)
                {
                    // Kenarda sadece %40 doluluk
                    if (rand < 0.60f) continue;
                }
                else
                {
                    // Uzak alanlarda %85 doluluk — neredeyse tam dolu
                    float noise = Mathf.PerlinNoise((x + 300f) / treeNoiseScale, (z + 300f) / treeNoiseScale);
                    if (noise < 0.25f && rand < 0.40f) continue;
                }

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
