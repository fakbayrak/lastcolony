using UnityEngine;

public class DepoVisual : MonoBehaviour, IBuildingVisual
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

    // ── Tier 1: ahşap ambar ──────────────────────────────────────────────
    private void BuildTier1()
    {
        container = NewContainer();

        CreateBox("StoneFoundation", new Vector3(0f, 0.05f, 0f),
            new Vector3(1.18f, 0.1f, 1.0f), "#8A8070", Quaternion.identity);
        CreateBox("BodyLower", new Vector3(0f, 0.35f, 0f),
            new Vector3(1.12f, 0.5f, 0.96f), "#8B5E35", Quaternion.identity);
        CreateBox("BodyUpper", new Vector3(0f, 0.76f, 0f),
            new Vector3(1.12f, 0.32f, 0.96f), "#7A5230", Quaternion.identity);
        CreateBox("FloorStrip", new Vector3(0f, 0.615f, 0f),
            new Vector3(1.14f, 0.05f, 0.98f), "#5A3515", Quaternion.identity);
        CreateBox("BeamLeft", new Vector3(-0.5f, 0.51f, 0f),
            new Vector3(0.05f, 0.82f, 0.97f), "#5A3515", Quaternion.identity);
        CreateBox("BeamRight", new Vector3(0.5f, 0.51f, 0f),
            new Vector3(0.05f, 0.82f, 0.97f), "#5A3515", Quaternion.identity);
        CreateBox("BeamMid", new Vector3(0f, 0.51f, 0f),
            new Vector3(0.05f, 0.82f, 0.97f), "#5A3515", Quaternion.identity);

        CreateBox("RoofLowerLeft", new Vector3(-0.38f, 0.97f, 0f),
            new Vector3(0.48f, 0.08f, 1.0f), "#5C3D1E", Quaternion.Euler(0f, 0f, -28f));
        CreateBox("RoofLowerRight", new Vector3(0.38f, 0.97f, 0f),
            new Vector3(0.48f, 0.08f, 1.0f), "#5C3D1E", Quaternion.Euler(0f, 0f, 28f));
        CreateBox("RoofUpperLeft", new Vector3(-0.18f, 1.2f, 0f),
            new Vector3(0.35f, 0.08f, 1.0f), "#4A2E0E", Quaternion.Euler(0f, 0f, -55f));
        CreateBox("RoofUpperRight", new Vector3(0.18f, 1.2f, 0f),
            new Vector3(0.35f, 0.08f, 1.0f), "#4A2E0E", Quaternion.Euler(0f, 0f, 55f));
        CreateBox("RoofRidge", new Vector3(0f, 1.38f, 0f),
            new Vector3(0.1f, 0.08f, 1.0f), "#3A1E00", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 1.05f, 0.49f),
            new Vector3(1.12f, 0.5f, 0.07f), "#6B4520", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 1.05f, -0.49f),
            new Vector3(1.12f, 0.5f, 0.07f), "#6B4520", Quaternion.identity);

        CreateBox("DoorLeft", new Vector3(-0.11f, 0.31f, 0.52f),
            new Vector3(0.2f, 0.42f, 0.06f), "#3B2007", Quaternion.identity);
        CreateBox("DoorRight", new Vector3(0.11f, 0.31f, 0.52f),
            new Vector3(0.2f, 0.42f, 0.06f), "#3B2007", Quaternion.identity);
        CreateBox("DoorArch", new Vector3(0f, 0.54f, 0.52f),
            new Vector3(0.44f, 0.08f, 0.06f), "#2C1500", Quaternion.identity);
        CreateBox("DoorHandleLeft", new Vector3(-0.02f, 0.31f, 0.54f),
            new Vector3(0.03f, 0.03f, 0.05f), "#C8A030", Quaternion.identity);
        CreateBox("DoorHandleRight", new Vector3(0.02f, 0.31f, 0.54f),
            new Vector3(0.03f, 0.03f, 0.05f), "#C8A030", Quaternion.identity);

        CreateBox("WindowLeft", new Vector3(-0.57f, 0.7f, 0.2f),
            new Vector3(0.06f, 0.18f, 0.22f), "#A8D8EA", Quaternion.identity);
        CreateBox("WindowRight", new Vector3(0.57f, 0.7f, 0.2f),
            new Vector3(0.06f, 0.18f, 0.22f), "#A8D8EA", Quaternion.identity);
    }

    // ── Tier 2: daha büyük ahşap ambar ───────────────────────────────────
    private void BuildTier2()
    {
        container = NewContainer();

        CreateBox("Body", new Vector3(0f, 0.325f, 0f),
            new Vector3(1.3f, 0.65f, 1.08f), "#8B5E35", Quaternion.identity);

        float[] beamX = { -0.6f, -0.2f, 0.2f, 0.6f };
        foreach (float x in beamX)
            CreateBox("Beam", new Vector3(x, 0.325f, 0f),
                new Vector3(0.05f, 0.66f, 1.09f), "#5A3515", Quaternion.identity);

        CreateBox("FloorStrip1", new Vector3(0f, 0.25f, 0f),
            new Vector3(1.32f, 0.05f, 1.09f), "#5A3515", Quaternion.identity);
        CreateBox("FloorStrip2", new Vector3(0f, 0.5f, 0f),
            new Vector3(1.32f, 0.05f, 1.09f), "#5A3515", Quaternion.identity);

        CreateBox("RoofLowerLeft", new Vector3(-0.42f, 0.78f, 0f),
            new Vector3(0.52f, 0.08f, 1.12f), "#4A2E0E", Quaternion.Euler(0f, 0f, -28f));
        CreateBox("RoofLowerRight", new Vector3(0.42f, 0.78f, 0f),
            new Vector3(0.52f, 0.08f, 1.12f), "#4A2E0E", Quaternion.Euler(0f, 0f, 28f));
        CreateBox("RoofUpperLeft", new Vector3(-0.2f, 1.04f, 0f),
            new Vector3(0.4f, 0.08f, 1.12f), "#4A2E0E", Quaternion.Euler(0f, 0f, -55f));
        CreateBox("RoofUpperRight", new Vector3(0.2f, 1.04f, 0f),
            new Vector3(0.4f, 0.08f, 1.12f), "#4A2E0E", Quaternion.Euler(0f, 0f, 55f));
        CreateBox("RoofRidge", new Vector3(0f, 1.24f, 0f),
            new Vector3(0.14f, 0.1f, 1.12f), "#3A1E00", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 0.9f, 0.55f),
            new Vector3(1.3f, 0.55f, 0.07f), "#6B4520", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 0.9f, -0.55f),
            new Vector3(1.3f, 0.55f, 0.07f), "#6B4520", Quaternion.identity);
        CreateBox("Vent", new Vector3(0f, 1.0f, 0.56f),
            new Vector3(0.18f, 0.12f, 0.06f), "#2C1500", Quaternion.identity);

        CreateBox("DoorLeft", new Vector3(-0.13f, 0.34f, 0.56f),
            new Vector3(0.24f, 0.48f, 0.06f), "#3B2007", Quaternion.identity);
        CreateBox("DoorRight", new Vector3(0.13f, 0.34f, 0.56f),
            new Vector3(0.24f, 0.48f, 0.06f), "#3B2007", Quaternion.identity);
        CreateBox("DoorArch", new Vector3(0f, 0.6f, 0.56f),
            new Vector3(0.52f, 0.08f, 0.06f), "#2C1500", Quaternion.identity);

        float[] winZ = { -0.25f, 0.25f };
        foreach (float z in winZ)
        {
            CreateBox("WindowLeft", new Vector3(-0.66f, 0.4f, z),
                new Vector3(0.06f, 0.18f, 0.22f), "#A8D8EA", Quaternion.identity);
            CreateBox("WindowRight", new Vector3(0.66f, 0.4f, z),
                new Vector3(0.06f, 0.18f, 0.22f), "#A8D8EA", Quaternion.identity);
        }
    }

    // ── Tier 3: taş ambar, metal kapı ────────────────────────────────────
    private void BuildTier3()
    {
        container = NewContainer();

        CreateBox("StoneFoundation", new Vector3(0f, 0.07f, 0f),
            new Vector3(1.38f, 0.14f, 1.1f), "#6A6A62", Quaternion.identity);
        CreateBox("BodyLowerStone", new Vector3(0f, 0.4f, 0f),
            new Vector3(1.32f, 0.52f, 1.06f), "#8A8075", Quaternion.identity);
        CreateBox("BodyUpperWood", new Vector3(0f, 0.84f, 0f),
            new Vector3(1.32f, 0.36f, 1.06f), "#7A5230", Quaternion.identity);

        Vector3[] buttress = {
            new Vector3(-0.62f, 0.4f, -0.5f),
            new Vector3(0.62f, 0.4f, -0.5f),
            new Vector3(-0.62f, 0.4f, 0.5f),
            new Vector3(0.62f, 0.4f, 0.5f),
        };
        foreach (var b in buttress)
            CreateBox("Buttress", b,
                new Vector3(0.12f, 0.68f, 0.12f), "#6A6A62", Quaternion.identity);

        CreateBox("RoofLowerLeft", new Vector3(-0.46f, 1.12f, 0f),
            new Vector3(0.52f, 0.08f, 1.14f), "#3A2010", Quaternion.Euler(0f, 0f, -28f));
        CreateBox("RoofLowerRight", new Vector3(0.46f, 1.12f, 0f),
            new Vector3(0.52f, 0.08f, 1.14f), "#3A2010", Quaternion.Euler(0f, 0f, 28f));
        CreateBox("RoofUpperLeft", new Vector3(-0.22f, 1.38f, 0f),
            new Vector3(0.4f, 0.08f, 1.14f), "#3A2010", Quaternion.Euler(0f, 0f, -55f));
        CreateBox("RoofUpperRight", new Vector3(0.22f, 1.38f, 0f),
            new Vector3(0.4f, 0.08f, 1.14f), "#3A2010", Quaternion.Euler(0f, 0f, 55f));
        CreateBox("RoofRidge", new Vector3(0f, 1.58f, 0f),
            new Vector3(0.14f, 0.1f, 1.14f), "#2A1408", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 1.22f, 0.56f),
            new Vector3(1.32f, 0.55f, 0.07f), "#7A5230", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 1.22f, -0.56f),
            new Vector3(1.32f, 0.55f, 0.07f), "#7A5230", Quaternion.identity);

        CreateBox("MetalDoor", new Vector3(0f, 0.32f, 0.57f),
            new Vector3(0.5f, 0.52f, 0.06f), "#4A5568", Quaternion.identity);
        CreateBox("DoorHandle", new Vector3(0.1f, 0.32f, 0.6f),
            new Vector3(0.03f, 0.1f, 0.04f), "#C8A030", Quaternion.identity);

        float[] winZ = { -0.32f, 0f, 0.32f };
        foreach (float z in winZ)
        {
            CreateBox("WindowLeft", new Vector3(-0.67f, 0.55f, z),
                new Vector3(0.06f, 0.2f, 0.22f), "#A8D8EA", Quaternion.identity);
            CreateBox("WindowRight", new Vector3(0.67f, 0.55f, z),
                new Vector3(0.06f, 0.2f, 0.22f), "#A8D8EA", Quaternion.identity);
        }
    }

    private Transform NewContainer()
    {
        Transform c = new GameObject("DepoVisual").transform;
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

        Renderer rend = box.GetComponent<Renderer>();
        Material mat  = new Material(Shader.Find("Standard"));
        mat.color     = HexColor(hex);
        mat.SetFloat("_Glossiness", 0.2f);
        mat.SetFloat("_Metallic",   0.0f);
        rend.material = mat;
        return box;
    }

    private Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
