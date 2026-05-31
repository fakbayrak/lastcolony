using UnityEngine;

public class BarakaVisual : MonoBehaviour, IBuildingVisual
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

    // ── Tier 1: saman çatılı köy kulübesi ────────────────────────────────
    private void BuildTier1()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.34f, 0f),
            new Vector3(0.95f, 0.68f, 0.95f), "#A0785A", Quaternion.identity);
        CreateBox("WoodLine1", new Vector3(0f, 0.22f, 0f),
            new Vector3(0.96f, 0.04f, 0.96f), "#7A5535", Quaternion.identity);
        CreateBox("WoodLine2", new Vector3(0f, 0.46f, 0f),
            new Vector3(0.96f, 0.04f, 0.96f), "#7A5535", Quaternion.identity);

        CreateBox("RoofLeft", new Vector3(-0.32f, 0.98f, 0f),
            new Vector3(0.07f, 0.62f, 1.02f), "#D4A843", Quaternion.Euler(0f, 0f, -40f));
        CreateBox("RoofRight", new Vector3(0.32f, 0.98f, 0f),
            new Vector3(0.07f, 0.62f, 1.02f), "#D4A843", Quaternion.Euler(0f, 0f, 40f));
        CreateBox("RoofRidge", new Vector3(0f, 1.22f, 0f),
            new Vector3(0.12f, 0.1f, 1.04f), "#B8902E", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 0.87f, 0.48f),
            new Vector3(0.95f, 0.38f, 0.07f), "#C49A35", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 0.87f, -0.48f),
            new Vector3(0.95f, 0.38f, 0.07f), "#C49A35", Quaternion.identity);
        CreateBox("EaveFront", new Vector3(0f, 0.7f, 0.5f),
            new Vector3(1.02f, 0.06f, 0.08f), "#B8902E", Quaternion.identity);
        CreateBox("EaveBack", new Vector3(0f, 0.7f, -0.5f),
            new Vector3(1.02f, 0.06f, 0.08f), "#B8902E", Quaternion.identity);

        CreateBox("DoorFrame", new Vector3(0f, 0.18f, 0.49f),
            new Vector3(0.24f, 0.36f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("DoorInner", new Vector3(0f, 0.17f, 0.51f),
            new Vector3(0.18f, 0.3f, 0.05f), "#3B2507", Quaternion.identity);
        CreateBox("DoorHandle", new Vector3(0.07f, 0.17f, 0.53f),
            new Vector3(0.03f, 0.03f, 0.04f), "#C8A030", Quaternion.identity);

        CreateBox("WindowLFrame", new Vector3(-0.28f, 0.42f, 0.49f),
            new Vector3(0.2f, 0.18f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("WindowLGlass", new Vector3(-0.28f, 0.42f, 0.51f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);
        CreateBox("WindowRFrame", new Vector3(0.28f, 0.42f, 0.49f),
            new Vector3(0.2f, 0.18f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("WindowRGlass", new Vector3(0.28f, 0.42f, 0.51f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);
    }

    // ── Tier 2: kiremit çatılı ahşap ev ──────────────────────────────────
    private void BuildTier2()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.375f, 0f),
            new Vector3(1.0f, 0.75f, 1.0f), "#A0785A", Quaternion.identity);
        CreateBox("WoodLine1", new Vector3(0f, 0.2f, 0f),
            new Vector3(1.01f, 0.04f, 1.01f), "#7A5535", Quaternion.identity);
        CreateBox("WoodLine2", new Vector3(0f, 0.4f, 0f),
            new Vector3(1.01f, 0.04f, 1.01f), "#7A5535", Quaternion.identity);
        CreateBox("WoodLine3", new Vector3(0f, 0.6f, 0f),
            new Vector3(1.01f, 0.04f, 1.01f), "#7A5535", Quaternion.identity);

        CreateBox("RoofLeft", new Vector3(-0.34f, 1.05f, 0f),
            new Vector3(0.07f, 0.68f, 1.08f), "#C0392B", Quaternion.Euler(0f, 0f, -40f));
        CreateBox("RoofRight", new Vector3(0.34f, 1.05f, 0f),
            new Vector3(0.07f, 0.68f, 1.08f), "#C0392B", Quaternion.Euler(0f, 0f, 40f));
        CreateBox("RoofRidge", new Vector3(0f, 1.32f, 0f),
            new Vector3(0.14f, 0.12f, 1.1f), "#922B21", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 0.95f, 0.5f),
            new Vector3(1.0f, 0.4f, 0.07f), "#8B4513", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 0.95f, -0.5f),
            new Vector3(1.0f, 0.4f, 0.07f), "#8B4513", Quaternion.identity);

        CreateBox("DoorFrame", new Vector3(0f, 0.18f, 0.51f),
            new Vector3(0.24f, 0.36f, 0.06f), "#6B3515", Quaternion.identity);
        CreateBox("DoorInner", new Vector3(0f, 0.17f, 0.53f),
            new Vector3(0.18f, 0.3f, 0.05f), "#2C1A06", Quaternion.identity);
        CreateBox("DoorHandle", new Vector3(0.07f, 0.17f, 0.55f),
            new Vector3(0.03f, 0.03f, 0.04f), "#C8A030", Quaternion.identity);

        CreateBox("WindowLFrame", new Vector3(-0.28f, 0.42f, 0.51f),
            new Vector3(0.2f, 0.18f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("WindowLGlass", new Vector3(-0.28f, 0.42f, 0.53f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);
        CreateBox("WindowRFrame", new Vector3(0.28f, 0.42f, 0.51f),
            new Vector3(0.2f, 0.18f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("WindowRGlass", new Vector3(0.28f, 0.42f, 0.53f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);

        CreateBox("SideWindowL", new Vector3(-0.51f, 0.42f, 0.1f),
            new Vector3(0.06f, 0.16f, 0.2f), "#A8D8EA", Quaternion.identity);
        CreateBox("SideWindowR", new Vector3(0.51f, 0.42f, 0.1f),
            new Vector3(0.06f, 0.16f, 0.2f), "#A8D8EA", Quaternion.identity);

        CreateCylinder("Chimney", new Vector3(0.3f, 1.18f, 0.3f),
            new Vector3(0.1f, 0.28f, 0.1f), "#7A7060");
    }

    // ── Tier 3: taş ev, daha büyük ───────────────────────────────────────
    private void BuildTier3()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.41f, 0f),
            new Vector3(1.08f, 0.82f, 1.08f), "#8A8A80", Quaternion.identity);

        float[] stripY = { 0.18f, 0.4f, 0.6f, 0.78f };
        for (int i = 0; i < stripY.Length; i++)
            CreateBox($"StoneLine{i}", new Vector3(0f, stripY[i], 0f),
                new Vector3(1.09f, 0.04f, 1.09f), "#6A6A62", Quaternion.identity);

        Vector3[] corners = {
            new Vector3(-0.5f, 0.42f, -0.5f),
            new Vector3(0.5f, 0.42f, -0.5f),
            new Vector3(-0.5f, 0.42f, 0.5f),
            new Vector3(0.5f, 0.42f, 0.5f),
        };
        foreach (var c in corners)
            CreateBox("CornerStone", c,
                new Vector3(0.1f, 0.84f, 0.1f), "#727268", Quaternion.identity);

        CreateBox("RoofLeft", new Vector3(-0.36f, 1.12f, 0f),
            new Vector3(0.07f, 0.72f, 1.12f), "#8B4513", Quaternion.Euler(0f, 0f, -40f));
        CreateBox("RoofRight", new Vector3(0.36f, 1.12f, 0f),
            new Vector3(0.07f, 0.72f, 1.12f), "#8B4513", Quaternion.Euler(0f, 0f, 40f));
        CreateBox("RoofRidge", new Vector3(0f, 1.42f, 0f),
            new Vector3(0.16f, 0.14f, 1.14f), "#6B3210", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 1.0f, 0.54f),
            new Vector3(1.08f, 0.42f, 0.07f), "#7A5530", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 1.0f, -0.54f),
            new Vector3(1.08f, 0.42f, 0.07f), "#7A5530", Quaternion.identity);

        CreateBox("DoorFrame", new Vector3(0f, 0.2f, 0.55f),
            new Vector3(0.28f, 0.42f, 0.06f), "#606058", Quaternion.identity);
        CreateBox("DoorInner", new Vector3(0f, 0.19f, 0.57f),
            new Vector3(0.22f, 0.36f, 0.05f), "#2C1A06", Quaternion.identity);

        // Ön pencereler (2)
        CreateBox("WindowFLFrame", new Vector3(-0.32f, 0.48f, 0.55f),
            new Vector3(0.2f, 0.18f, 0.06f), "#606058", Quaternion.identity);
        CreateBox("WindowFLGlass", new Vector3(-0.32f, 0.48f, 0.57f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);
        CreateBox("WindowFRFrame", new Vector3(0.32f, 0.48f, 0.55f),
            new Vector3(0.2f, 0.18f, 0.06f), "#606058", Quaternion.identity);
        CreateBox("WindowFRGlass", new Vector3(0.32f, 0.48f, 0.57f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);

        // Yan pencereler (her duvarda 2'şer)
        float[] sideZ = { -0.25f, 0.25f };
        foreach (float z in sideZ)
        {
            CreateBox("WindowSLFrame", new Vector3(-0.55f, 0.48f, z),
                new Vector3(0.06f, 0.18f, 0.2f), "#606058", Quaternion.identity);
            CreateBox("WindowSLGlass", new Vector3(-0.57f, 0.48f, z),
                new Vector3(0.04f, 0.12f, 0.14f), "#A8D8EA", Quaternion.identity);
            CreateBox("WindowSRFrame", new Vector3(0.55f, 0.48f, z),
                new Vector3(0.06f, 0.18f, 0.2f), "#606058", Quaternion.identity);
            CreateBox("WindowSRGlass", new Vector3(0.57f, 0.48f, z),
                new Vector3(0.04f, 0.12f, 0.14f), "#A8D8EA", Quaternion.identity);
        }

        CreateCylinder("Chimney", new Vector3(0.32f, 1.3f, 0.32f),
            new Vector3(0.13f, 0.35f, 0.13f), "#5A5248");
    }

    private Transform NewContainer()
    {
        Transform c = new GameObject("BarakaVisual").transform;
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
