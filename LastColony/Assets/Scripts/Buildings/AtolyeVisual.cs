using UnityEngine;

public class AtolyeVisual : MonoBehaviour, IBuildingVisual
{
    private Transform container;
    private int currentTier = 1;

    private void Awake()
    {
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

    // ── Tier 1: atölye ───────────────────────────────────────────────────
    private void BuildTier1()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.36f, 0f),
            new Vector3(0.92f, 0.72f, 0.92f), "#8A8070", Quaternion.identity);
        CreateBox("Roof", new Vector3(0f, 0.77f, 0f),
            new Vector3(0.96f, 0.1f, 0.96f), "#5A5045", Quaternion.identity);
        CreateCylinder("Chimney", new Vector3(0.3f, 1.05f, 0.3f),
            new Vector3(0.18f, 0.5f, 0.18f), "#3C3530");
        CreateBox("WindowL", new Vector3(-0.47f, 0.4f, 0f),
            new Vector3(0.15f, 0.15f, 0.04f), "#C8B87A", Quaternion.Euler(0f, 90f, 0f));
        CreateBox("WindowR", new Vector3(0.47f, 0.4f, 0f),
            new Vector3(0.15f, 0.15f, 0.04f), "#C8B87A", Quaternion.Euler(0f, 90f, 0f));
    }

    // ── Tier 2: fabrika ──────────────────────────────────────────────────
    private void BuildTier2()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.41f, 0f),
            new Vector3(1.0f, 0.82f, 1.0f), "#8A8070", Quaternion.identity);
        CreateBox("Roof", new Vector3(0f, 0.87f, 0f),
            new Vector3(1.02f, 0.1f, 1.02f), "#5A5045", Quaternion.identity);

        CreateCylinder("ChimneyA", new Vector3(0.28f, 1.12f, 0.28f),
            new Vector3(0.16f, 0.52f, 0.16f), "#3C3530");
        CreateCylinder("ChimneyB", new Vector3(-0.28f, 1.12f, -0.28f),
            new Vector3(0.16f, 0.52f, 0.16f), "#3C3530");

        float[] winX = { -0.28f, 0f, 0.28f };
        foreach (float x in winX)
            CreateBox("Window", new Vector3(x, 0.45f, 0.51f),
                new Vector3(0.16f, 0.18f, 0.04f), "#C8B87A", Quaternion.identity);

        CreateBox("Door", new Vector3(0f, 0.19f, 0.51f),
            new Vector3(0.28f, 0.38f, 0.05f), "#4A4540", Quaternion.identity);
    }

    // ── Tier 3: büyük fabrika ────────────────────────────────────────────
    private void BuildTier3()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.5f, 0f),
            new Vector3(1.08f, 1.0f, 1.08f), "#706860", Quaternion.identity);
        CreateBox("Roof", new Vector3(0f, 1.06f, 0f),
            new Vector3(1.1f, 0.12f, 1.1f), "#4A4540", Quaternion.identity);

        Vector3[] chimneyPos = {
            new Vector3(0.32f, 1.45f, 0.32f),
            new Vector3(-0.32f, 1.45f, 0.32f),
            new Vector3(0.32f, 1.45f, -0.32f),
        };
        foreach (var p in chimneyPos)
        {
            CreateCylinder("Chimney", p,
                new Vector3(0.18f, 0.65f, 0.18f), "#2C2825");
            // Baca üst kırmızı bant
            CreateCylinder("ChimneyRing", new Vector3(p.x, p.y + 0.34f, p.z),
                new Vector3(0.22f, 0.04f, 0.22f), "#C0392B");
        }

        // Ön pencereler (4)
        float[] frontX = { -0.36f, -0.12f, 0.12f, 0.36f };
        foreach (float x in frontX)
            CreateBox("WindowFront", new Vector3(x, 0.6f, 0.55f),
                new Vector3(0.16f, 0.2f, 0.04f), "#C8B87A", Quaternion.identity);

        // Yan pencereler (2)
        CreateBox("WindowSideL", new Vector3(-0.55f, 0.6f, 0f),
            new Vector3(0.04f, 0.2f, 0.18f), "#C8B87A", Quaternion.identity);
        CreateBox("WindowSideR", new Vector3(0.55f, 0.6f, 0f),
            new Vector3(0.04f, 0.2f, 0.18f), "#C8B87A", Quaternion.identity);

        CreateBox("MetalDoor", new Vector3(0f, 0.24f, 0.55f),
            new Vector3(0.34f, 0.48f, 0.05f), "#4A5568", Quaternion.identity);
    }

    private Transform NewContainer()
    {
        Transform c = new GameObject("AtolyeVisual").transform;
        c.SetParent(transform, false);
        return c;
    }

    private GameObject CreateBox(string objName, Vector3 localPos,
        Vector3 size, string hex, Quaternion rotation)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = objName;
        box.transform.SetParent(container);
        box.transform.localPosition = localPos;
        box.transform.localScale    = size;
        box.transform.localRotation = rotation;
        Destroy(box.GetComponent<Collider>());
        ApplyMaterial(box, hex);
        return box;
    }

    private GameObject CreateCylinder(string objName, Vector3 localPos,
        Vector3 size, string hex)
    {
        GameObject cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cyl.name = objName;
        cyl.transform.SetParent(container);
        cyl.transform.localPosition = localPos;
        cyl.transform.localScale    = size;
        cyl.transform.localRotation = Quaternion.identity;
        Destroy(cyl.GetComponent<Collider>());
        ApplyMaterial(cyl, hex);
        return cyl;
    }

    private void ApplyMaterial(GameObject obj, string hex)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        Material mat  = new Material(Shader.Find("Standard"));
        mat.color     = HexColor(hex);
        mat.SetFloat("_Glossiness", 0.2f);
        mat.SetFloat("_Metallic",   0.0f);
        rend.material = mat;
    }

    private Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
