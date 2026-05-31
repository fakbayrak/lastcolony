using UnityEngine;

public class TowerVisual : MonoBehaviour, IBuildingVisual
{
    private Transform container;
    private int currentTier = 1;

    private void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        BuildTier1();
    }

    public void UpgradeTo(int tier)
    {
        if (container != null) Destroy(container.gameObject);
        currentTier = tier;
        switch (tier)
        {
            case 2:  BuildTier2(); break;
            case 3:  BuildTier3(); break;
            default: BuildTier1(); break;
        }
    }

    // ── Tier 1: temel taş kule ───────────────────────────────────────────
    private void BuildTier1()
    {
        container = NewContainer();

        Color stoneColor     = new Color(0.55f, 0.52f, 0.48f);
        Color stoneDark      = new Color(0.40f, 0.38f, 0.34f);
        Color stoneLight     = new Color(0.68f, 0.65f, 0.60f);
        Color woodColor      = new Color(0.40f, 0.28f, 0.14f);
        Color flagColor      = new Color(0.80f, 0.10f, 0.10f);
        Color arrowSlitColor = new Color(0.20f, 0.18f, 0.15f);

        CreateBox("Base", new Vector3(0f, 0.15f, 0f),
            new Vector3(1.10f, 0.30f, 1.10f), stoneDark);

        Vector3[] cornerOffsets = {
            new Vector3( 0.45f, 0.20f,  0.45f),
            new Vector3(-0.45f, 0.20f,  0.45f),
            new Vector3( 0.45f, 0.20f, -0.45f),
            new Vector3(-0.45f, 0.20f, -0.45f),
        };
        foreach (var offset in cornerOffsets)
            CreateBox("CornerStone", offset, new Vector3(0.22f, 0.40f, 0.22f), stoneLight);

        CreateBox("TowerBody", new Vector3(0f, 0.85f, 0f),
            new Vector3(0.80f, 1.10f, 0.80f), stoneColor);

        for (int i = 0; i < 3; i++)
        {
            float y = 0.45f + i * 0.30f;
            CreateBox($"StoneRow_{i}", new Vector3(0f, y, 0.41f),
                new Vector3(0.82f, 0.04f, 0.02f), stoneDark);
        }

        Vector3[] slitDirs = {
            new Vector3(0f,    0.85f,  0.41f),
            new Vector3(0f,    0.85f, -0.41f),
            new Vector3( 0.41f, 0.85f, 0f),
            new Vector3(-0.41f, 0.85f, 0f),
        };
        bool[] rotated = { false, false, true, true };
        for (int i = 0; i < slitDirs.Length; i++)
            CreateBox($"ArrowSlit_{i}", slitDirs[i],
                rotated[i] ? new Vector3(0.06f, 0.22f, 0.04f)
                           : new Vector3(0.22f, 0.06f, 0.04f),
                arrowSlitColor);

        CreateBox("TopPlatform", new Vector3(0f, 1.50f, 0f),
            new Vector3(0.92f, 0.14f, 0.92f), stoneDark);

        Vector3[] battlementPositions = {
            new Vector3( 0.32f, 1.68f,  0.38f),
            new Vector3(-0.32f, 1.68f,  0.38f),
            new Vector3( 0.32f, 1.68f, -0.38f),
            new Vector3(-0.32f, 1.68f, -0.38f),
            new Vector3( 0.38f, 1.68f,  0.32f),
            new Vector3( 0.38f, 1.68f, -0.32f),
            new Vector3(-0.38f, 1.68f,  0.32f),
            new Vector3(-0.38f, 1.68f, -0.32f),
        };
        foreach (var bPos in battlementPositions)
            CreateBox("Battlement", bPos, new Vector3(0.18f, 0.22f, 0.18f), stoneLight);

        CreateBox("FlagPole", new Vector3(0f, 2.05f, 0f),
            new Vector3(0.06f, 0.60f, 0.06f), woodColor);
        CreateBox("Flag", new Vector3(0.18f, 2.28f, 0f),
            new Vector3(0.32f, 0.16f, 0.04f), flagColor);

        CreateBox("Door", new Vector3(0f, 0.38f, 0.41f),
            new Vector3(0.28f, 0.38f, 0.04f), new Color(0.28f, 0.18f, 0.08f));
        CreateBox("DoorArch", new Vector3(0f, 0.58f, 0.41f),
            new Vector3(0.32f, 0.10f, 0.04f), stoneDark);
    }

    // ── Tier 2: güçlendirilmiş kule (~%20 daha büyük, 2 bayrak) ──────────
    private void BuildTier2()
    {
        container = NewContainer();

        Color stoneColor  = new Color(0.55f, 0.52f, 0.48f);
        Color stoneDark   = new Color(0.40f, 0.38f, 0.34f);
        Color stoneLight  = new Color(0.68f, 0.65f, 0.60f);
        Color woodColor   = new Color(0.40f, 0.28f, 0.14f);
        Color flagRed     = new Color(0.80f, 0.10f, 0.10f);
        Color flagBlue    = new Color(0.15f, 0.30f, 0.75f);
        Color slitColor   = new Color(0.20f, 0.18f, 0.15f);

        CreateBox("Base", new Vector3(0f, 0.18f, 0f),
            new Vector3(1.32f, 0.36f, 1.32f), stoneDark);

        Vector3[] cornerOffsets = {
            new Vector3( 0.54f, 0.24f,  0.54f),
            new Vector3(-0.54f, 0.24f,  0.54f),
            new Vector3( 0.54f, 0.24f, -0.54f),
            new Vector3(-0.54f, 0.24f, -0.54f),
        };
        foreach (var offset in cornerOffsets)
            CreateBox("CornerStone", offset, new Vector3(0.26f, 0.48f, 0.26f), stoneLight);

        CreateBox("TowerBody", new Vector3(0f, 1.02f, 0f),
            new Vector3(0.96f, 1.32f, 0.96f), stoneColor);

        for (int i = 0; i < 3; i++)
        {
            float y = 0.55f + i * 0.36f;
            CreateBox($"StoneRow_{i}", new Vector3(0f, y, 0.49f),
                new Vector3(0.98f, 0.05f, 0.02f), stoneDark);
        }

        Vector3[] slitDirs = {
            new Vector3(0f,    1.02f,  0.49f),
            new Vector3(0f,    1.02f, -0.49f),
            new Vector3( 0.49f, 1.02f, 0f),
            new Vector3(-0.49f, 1.02f, 0f),
        };
        bool[] rotated = { false, false, true, true };
        for (int i = 0; i < slitDirs.Length; i++)
            CreateBox($"ArrowSlit_{i}", slitDirs[i],
                rotated[i] ? new Vector3(0.07f, 0.28f, 0.05f)
                           : new Vector3(0.28f, 0.07f, 0.05f),
                slitColor);

        CreateBox("TopPlatform", new Vector3(0f, 1.80f, 0f),
            new Vector3(1.1f, 0.16f, 1.1f), stoneDark);

        Vector3[] battlementPositions = {
            new Vector3( 0.4f, 2.0f,  0.46f),
            new Vector3(-0.4f, 2.0f,  0.46f),
            new Vector3( 0.4f, 2.0f, -0.46f),
            new Vector3(-0.4f, 2.0f, -0.46f),
            new Vector3( 0.46f, 2.0f,  0.4f),
            new Vector3( 0.46f, 2.0f, -0.4f),
            new Vector3(-0.46f, 2.0f,  0.4f),
            new Vector3(-0.46f, 2.0f, -0.4f),
        };
        foreach (var bPos in battlementPositions)
            CreateBox("Battlement", bPos, new Vector3(0.24f, 0.28f, 0.24f), stoneLight);

        // İki bayrak (kırmızı + mavi)
        CreateBox("FlagPole", new Vector3(0f, 2.45f, 0f),
            new Vector3(0.07f, 0.72f, 0.07f), woodColor);
        CreateBox("FlagRed", new Vector3(0.2f, 2.72f, 0f),
            new Vector3(0.36f, 0.18f, 0.04f), flagRed);
        CreateBox("FlagBlue", new Vector3(-0.2f, 2.5f, 0f),
            new Vector3(0.3f, 0.16f, 0.04f), flagBlue);

        // Belirgin kapı kemeri
        CreateBox("Door", new Vector3(0f, 0.45f, 0.49f),
            new Vector3(0.34f, 0.46f, 0.05f), new Color(0.28f, 0.18f, 0.08f));
        CreateBox("DoorArch", new Vector3(0f, 0.72f, 0.49f),
            new Vector3(0.42f, 0.16f, 0.05f), stoneLight);
    }

    // ── Tier 3: ileri savunma kulesi (3 katman mazgal, 4 köşe kulesi) ────
    private void BuildTier3()
    {
        container = NewContainer();

        Color stoneColor  = new Color(0.55f, 0.52f, 0.48f);
        Color stoneDark   = new Color(0.40f, 0.38f, 0.34f);
        Color stoneLight  = new Color(0.68f, 0.65f, 0.60f);
        Color woodColor   = new Color(0.40f, 0.28f, 0.14f);
        Color flagRed     = new Color(0.80f, 0.10f, 0.10f);
        Color flagBlue    = new Color(0.15f, 0.30f, 0.75f);
        Color slitColor   = new Color(0.20f, 0.18f, 0.15f);

        // Genişletilmiş taban
        CreateBox("BaseWide", new Vector3(0f, 0.12f, 0f),
            new Vector3(1.7f, 0.24f, 1.7f), stoneDark);
        CreateBox("Base", new Vector3(0f, 0.34f, 0f),
            new Vector3(1.4f, 0.4f, 1.4f), stoneDark);

        CreateBox("TowerBody", new Vector3(0f, 1.2f, 0f),
            new Vector3(1.04f, 1.5f, 1.04f), stoneColor);

        for (int i = 0; i < 4; i++)
        {
            float y = 0.65f + i * 0.36f;
            CreateBox($"StoneRow_{i}", new Vector3(0f, y, 0.53f),
                new Vector3(1.06f, 0.05f, 0.02f), stoneDark);
        }

        Vector3[] slitDirs = {
            new Vector3(0f,    1.2f,  0.53f),
            new Vector3(0f,    1.2f, -0.53f),
            new Vector3( 0.53f, 1.2f, 0f),
            new Vector3(-0.53f, 1.2f, 0f),
        };
        bool[] rotated = { false, false, true, true };
        for (int i = 0; i < slitDirs.Length; i++)
            CreateBox($"ArrowSlit_{i}", slitDirs[i],
                rotated[i] ? new Vector3(0.08f, 0.32f, 0.05f)
                           : new Vector3(0.32f, 0.08f, 0.05f),
                slitColor);

        CreateBox("TopPlatform", new Vector3(0f, 2.05f, 0f),
            new Vector3(1.2f, 0.18f, 1.2f), stoneDark);

        // 3 katman mazgal
        float[] battleY = { 2.26f, 2.5f, 2.74f };
        float[] battleSize = { 0.28f, 0.24f, 0.2f };
        for (int layer = 0; layer < 3; layer++)
        {
            float off = 0.5f - layer * 0.06f;
            Vector3[] pos = {
                new Vector3( off, battleY[layer],  off),
                new Vector3(-off, battleY[layer],  off),
                new Vector3( off, battleY[layer], -off),
                new Vector3(-off, battleY[layer], -off),
            };
            foreach (var p in pos)
                CreateBox($"Battlement_L{layer}", p,
                    new Vector3(battleSize[layer], 0.24f, battleSize[layer]), stoneLight);
        }

        // 4 köşeye küçük kule çıkıntısı
        Vector3[] turretPos = {
            new Vector3( 0.5f, 1.4f,  0.5f),
            new Vector3(-0.5f, 1.4f,  0.5f),
            new Vector3( 0.5f, 1.4f, -0.5f),
            new Vector3(-0.5f, 1.4f, -0.5f),
        };
        foreach (var p in turretPos)
            CreateBox("CornerTurret", p, new Vector3(0.28f, 1.7f, 0.28f), stoneColor);

        // 2 büyük bayrak
        CreateBox("FlagPole", new Vector3(0f, 3.0f, 0f),
            new Vector3(0.08f, 0.9f, 0.08f), woodColor);
        CreateBox("FlagRed", new Vector3(0.26f, 3.32f, 0f),
            new Vector3(0.46f, 0.24f, 0.04f), flagRed);
        CreateBox("FlagBlue", new Vector3(-0.26f, 3.05f, 0f),
            new Vector3(0.4f, 0.22f, 0.04f), flagBlue);

        CreateBox("Door", new Vector3(0f, 0.55f, 0.53f),
            new Vector3(0.38f, 0.5f, 0.05f), new Color(0.28f, 0.18f, 0.08f));
        CreateBox("DoorArch", new Vector3(0f, 0.85f, 0.53f),
            new Vector3(0.46f, 0.18f, 0.05f), stoneLight);
    }

    private Transform NewContainer()
    {
        Transform c = new GameObject("TowerVisual").transform;
        c.SetParent(transform, false);
        return c;
    }

    private GameObject CreateBox(string objName, Vector3 localPos, Vector3 size, Color color)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = objName;
        box.transform.SetParent(container);
        box.transform.localPosition = localPos;
        box.transform.localScale    = size;
        box.transform.localRotation = Quaternion.identity;
        Destroy(box.GetComponent<Collider>());

        Renderer rend = box.GetComponent<Renderer>();
        Material mat  = new Material(Shader.Find("Standard"));
        mat.color     = color;
        mat.SetFloat("_Glossiness", 0.05f);
        mat.SetFloat("_Metallic",   0.0f);
        rend.material = mat;
        return box;
    }
}
