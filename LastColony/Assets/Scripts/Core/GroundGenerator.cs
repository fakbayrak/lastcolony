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
        GenerateMountains();
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

                // Dere alanını atla (x = -8 ile -1)
                if (x >= -9 && x <= -1)
                    continue;

                float noise = Mathf.PerlinNoise((x + 300f) / treeNoiseScale, (z + 300f) / treeNoiseScale);
                if (noise < treeNoiseCutoff) continue;

                // Rastgele varyasyon için seed
                float rand = Mathf.PerlinNoise(x * 7.3f, z * 3.7f);
                if (rand < 0.20f) continue; // Seyrekleştir

                float wx = x + 0.5f + (rand - 0.5f) * 0.4f;
                float wz = z + 0.5f + (Mathf.PerlinNoise(x * 5f, z * 5f) - 0.5f) * 0.4f;

                CreateTree(wx, wz, rand, treeParent.transform);
            }
        }
    }

    private void GenerateMountains()
    {
        GameObject mountainParent = new GameObject("Mountains");
        mountainParent.transform.SetParent(transform);

        float baseZ = gridHeight + 12f;

        float[,] peaks = new float[,]
        {
            // wx,    wz,           height, radius
            { -4f,   baseZ + 0f,   6.0f,   4.0f },
            {  3f,   baseZ + 3f,   9.0f,   5.5f },
            { 10f,   baseZ + 5f,  12.0f,   7.0f },
            { 18f,   baseZ + 4f,  10.0f,   6.0f },
            { 25f,   baseZ + 2f,   8.0f,   5.0f },
            { 31f,   baseZ + 0f,   6.0f,   4.0f },
            // Ön küçük tepeler
            {  0f,   baseZ - 5f,   4.0f,   3.0f },
            { 14f,   baseZ - 3f,   5.0f,   3.5f },
            { 27f,   baseZ - 4f,   4.5f,   3.0f },
        };

        for (int i = 0; i < peaks.GetLength(0); i++)
            CreateMountain(peaks[i,0], peaks[i,1], peaks[i,2], peaks[i,3], mountainParent.transform);
    }

    private void CreateMountain(float wx, float wz, float height, float radius, Transform parent)
    {
        GameObject mountain = new GameObject("Mountain");
        mountain.transform.SetParent(parent);
        mountain.transform.position = new Vector3(wx, 0f, wz);

        MeshFilter mf = mountain.AddComponent<MeshFilter>();
        MeshRenderer mr = mountain.AddComponent<MeshRenderer>();

        // Dağ mesh'i: tabanda çember, tepede nokta (koni benzeri ama yumuşak)
        int segments = 16;
        int rings    = 6;

        // Vertex sayısı: her ring * segments + tepe noktası + taban merkezi
        int vertCount = (rings + 1) * (segments + 1) + 2;
        Vector3[] verts  = new Vector3[vertCount];
        Color[]   colors = new Color[vertCount];
        int[]     tris   = new int[segments * rings * 6 + segments * 3 + segments * 3];

        int vi = 0;

        // Taban merkezi
        verts[vi]  = new Vector3(0f, 0f, 0f);
        colors[vi] = new Color(0.28f, 0.35f, 0.20f); // yeşil-kahve taban
        int baseCenterIdx = vi++;

        // Ring'ler: tabandan tepeye
        for (int r = 0; r <= rings; r++)
        {
            float t = (float)r / rings; // 0 = taban, 1 = tepe

            // Dağ profili: kare kök eğrisi — tabanda geniş, yukarı doğru hızla daralır
            float radiusAtRing = radius * Mathf.Pow(1f - t, 0.6f);
            float heightAtRing = height * Mathf.Pow(t, 0.8f);

            // Renk: tabanda yeşil-kahve, ortada gri-kahve, tepede açık gri/beyaz
            Color ringColor;
            if (t < 0.3f)
                ringColor = Color.Lerp(new Color(0.30f, 0.38f, 0.22f), new Color(0.45f, 0.42f, 0.38f), t / 0.3f);
            else if (t < 0.75f)
                ringColor = Color.Lerp(new Color(0.45f, 0.42f, 0.38f), new Color(0.58f, 0.56f, 0.54f), (t - 0.3f) / 0.45f);
            else
                ringColor = Color.Lerp(new Color(0.58f, 0.56f, 0.54f), new Color(0.90f, 0.92f, 0.95f), (t - 0.75f) / 0.25f);

            for (int s = 0; s <= segments; s++)
            {
                float angle = (float)s / segments * Mathf.PI * 2f;
                // Hafif gürültü ile doğallık
                float noiseR = 1f + Mathf.PerlinNoise(wx * 0.3f + s * 0.4f, wz * 0.3f + r * 0.7f) * 0.25f - 0.125f;
                float noiseH = 1f + Mathf.PerlinNoise(wx * 0.5f + r * 0.3f, wz * 0.5f + s * 0.2f) * 0.15f - 0.075f;

                float rx = Mathf.Cos(angle) * radiusAtRing * noiseR;
                float rz = Mathf.Sin(angle) * radiusAtRing * noiseR;
                float ry = heightAtRing * noiseH;

                verts[vi]  = new Vector3(rx, ry, rz);
                colors[vi] = ringColor;
                vi++;
            }
        }

        // Tepe noktası
        int peakIdx = vi;
        verts[vi]  = new Vector3(0f, height, 0f);
        colors[vi] = new Color(0.92f, 0.94f, 0.97f);
        vi++;

        // Triangle'lar
        int ti = 0;

        // Taban disk
        for (int s = 0; s < segments; s++)
        {
            tris[ti++] = baseCenterIdx;
            tris[ti++] = 1 + s;
            tris[ti++] = 1 + (s + 1) % segments;
        }

        // Ring'ler arası yan yüzeyler
        for (int r = 0; r < rings; r++)
        {
            int ringStart     = 1 + r       * (segments + 1);
            int nextRingStart = 1 + (r + 1) * (segments + 1);

            for (int s = 0; s < segments; s++)
            {
                int curr     = ringStart     + s;
                int next     = ringStart     + s + 1;
                int currTop  = nextRingStart + s;
                int nextTop  = nextRingStart + s + 1;

                tris[ti++] = curr;
                tris[ti++] = currTop;
                tris[ti++] = next;

                tris[ti++] = next;
                tris[ti++] = currTop;
                tris[ti++] = nextTop;
            }
        }

        // Tepe üçgenleri
        int lastRingStart = 1 + rings * (segments + 1);
        for (int s = 0; s < segments; s++)
        {
            tris[ti++] = lastRingStart + s;
            tris[ti++] = peakIdx;
            tris[ti++] = lastRingStart + s + 1;
        }

        Mesh mesh = new Mesh();
        mesh.name = "MountainMesh";
        mesh.vertices  = verts;
        mesh.colors    = colors;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        mf.mesh = mesh;

        // Vertex renklerini destekleyen shader
        Material mat = new Material(Shader.Find("Standard"));
        mat.enableInstancing = true;
        mr.material = mat;

        // Vertex renklerini aktif et
        mr.material.SetFloat("_Glossiness", 0.1f);
        mr.material.SetFloat("_Metallic", 0.0f);
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
