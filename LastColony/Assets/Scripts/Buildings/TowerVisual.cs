using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    private void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        BuildTower();
    }

    private void BuildTower()
    {
        Color stoneColor     = new Color(0.55f, 0.52f, 0.48f);
        Color stoneDark      = new Color(0.40f, 0.38f, 0.34f);
        Color stoneLight     = new Color(0.68f, 0.65f, 0.60f);
        Color woodColor      = new Color(0.40f, 0.28f, 0.14f);
        Color flagColor      = new Color(0.80f, 0.10f, 0.10f);
        Color arrowSlitColor = new Color(0.20f, 0.18f, 0.15f);

        // ── Taban platformu ──────────────────────────────────────────────
        CreateBox("Base", transform,
            new Vector3(0f, 0.15f, 0f),
            new Vector3(1.10f, 0.30f, 1.10f),
            stoneDark);

        // Taban kenar taşları (4 köşe)
        Vector3[] cornerOffsets = {
            new Vector3( 0.45f, 0.20f,  0.45f),
            new Vector3(-0.45f, 0.20f,  0.45f),
            new Vector3( 0.45f, 0.20f, -0.45f),
            new Vector3(-0.45f, 0.20f, -0.45f),
        };
        foreach (var offset in cornerOffsets)
            CreateBox("CornerStone", transform, offset,
                new Vector3(0.22f, 0.40f, 0.22f), stoneLight);

        // ── Ana kule gövdesi ─────────────────────────────────────────────
        CreateBox("TowerBody", transform,
            new Vector3(0f, 0.85f, 0f),
            new Vector3(0.80f, 1.10f, 0.80f),
            stoneColor);

        // Gövde yatay çizgi detayları (taş arası)
        for (int i = 0; i < 3; i++)
        {
            float y = 0.45f + i * 0.30f;
            CreateBox($"StoneRow_{i}", transform,
                new Vector3(0f, y, 0.41f),
                new Vector3(0.82f, 0.04f, 0.02f),
                stoneDark);
        }

        // ── Ok mazgalları (4 yön) ────────────────────────────────────────
        Vector3[] slitDirs = {
            new Vector3(0f,    0.85f,  0.41f),
            new Vector3(0f,    0.85f, -0.41f),
            new Vector3( 0.41f, 0.85f, 0f),
            new Vector3(-0.41f, 0.85f, 0f),
        };
        bool[] rotated = { false, false, true, true };
        for (int i = 0; i < slitDirs.Length; i++)
        {
            GameObject slit = CreateBox($"ArrowSlit_{i}", transform,
                slitDirs[i],
                rotated[i]
                    ? new Vector3(0.06f, 0.22f, 0.04f)
                    : new Vector3(0.22f, 0.06f, 0.04f),
                arrowSlitColor);
        }

        // ── Üst platform ────────────────────────────────────────────────
        CreateBox("TopPlatform", transform,
            new Vector3(0f, 1.50f, 0f),
            new Vector3(0.92f, 0.14f, 0.92f),
            stoneDark);

        // ── Mazgal dişleri (battlements) — 8 diş ───────────────────────
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
            CreateBox("Battlement", transform, bPos,
                new Vector3(0.18f, 0.22f, 0.18f), stoneLight);

        // ── Bayrak direği ────────────────────────────────────────────────
        CreateBox("FlagPole", transform,
            new Vector3(0f, 2.05f, 0f),
            new Vector3(0.06f, 0.60f, 0.06f),
            woodColor);

        // Bayrak
        CreateBox("Flag", transform,
            new Vector3(0.18f, 2.28f, 0f),
            new Vector3(0.32f, 0.16f, 0.04f),
            flagColor);

        // ── Kapı ─────────────────────────────────────────────────────────
        CreateBox("Door", transform,
            new Vector3(0f, 0.38f, 0.41f),
            new Vector3(0.28f, 0.38f, 0.04f),
            new Color(0.28f, 0.18f, 0.08f));

        // Kapı kemerini temsilen üst taş
        CreateBox("DoorArch", transform,
            new Vector3(0f, 0.58f, 0.41f),
            new Vector3(0.32f, 0.10f, 0.04f),
            stoneDark);
    }

    private GameObject CreateBox(string objName, Transform parent,
        Vector3 localPos, Vector3 size, Color color)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = objName;
        box.transform.SetParent(parent);
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
