using UnityEngine;

public class TarlaVisual : MonoBehaviour, IBuildingVisual
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

    // ── Tier 1: basit toprak tarla ───────────────────────────────────────
    private void BuildTier1()
    {
        container = NewContainer();

        BuildGround(0.95f);

        // 4 ekin sırası (Z boyunca uzanan) + altlarında toprak şerit
        float[] rowsX = { -0.3f, -0.1f, 0.1f, 0.3f };
        foreach (float x in rowsX)
        {
            CreateBox("SoilStrip", new Vector3(x, 0.09f, 0f),
                new Vector3(0.14f, 0.06f, 0.8f), "#7A4E2D", Quaternion.identity);
            CreateBox("CropRow", new Vector3(x, 0.18f, 0f),
                new Vector3(0.1f, 0.18f, 0.78f), "#8BC34A", Quaternion.identity);
        }

        BuildFence(0.48f, 0.05f, 0.05f, false);
    }

    // ── Tier 2: sulama kanallı tarla ─────────────────────────────────────
    private void BuildTier2()
    {
        container = NewContainer();

        BuildGround(1.0f);

        // 6 ekin sırası (X boyunca uzanan), orta bant kanala bırakıldı
        float[] rowsZ = { -0.42f, -0.28f, -0.14f, 0.14f, 0.28f, 0.42f };
        foreach (float z in rowsZ)
        {
            CreateBox("SoilStrip", new Vector3(0f, 0.09f, z),
                new Vector3(0.9f, 0.06f, 0.14f), "#7A4E2D", Quaternion.identity);
            CreateBox("CropRow", new Vector3(0f, 0.2f, z),
                new Vector3(0.86f, 0.22f, 0.1f), "#6DB33F", Quaternion.identity);
        }

        // Ortada yatay sulama kanalı
        CreateBox("Canal", new Vector3(0f, 0.07f, 0f),
            new Vector3(0.92f, 0.05f, 0.12f), "#4A90D9", Quaternion.identity);

        // Kanalın yanında küçük ahşap su çarkı (dikey göbek + yatay diskler)
        CreateCylinder("WheelHub", new Vector3(0.52f, 0.2f, 0f),
            new Vector3(0.06f, 0.18f, 0.06f), "#8B5E3C");
        CreateCylinder("WheelDiskLow", new Vector3(0.52f, 0.12f, 0f),
            new Vector3(0.22f, 0.02f, 0.22f), "#A0522D");
        CreateCylinder("WheelDiskHigh", new Vector3(0.52f, 0.28f, 0f),
            new Vector3(0.22f, 0.02f, 0.22f), "#A0522D");

        // Çit biraz daha güçlü görünümlü
        BuildFence(0.5f, 0.07f, 0.07f, false);
    }

    // ── Tier 3: sera / gelişmiş tarla ────────────────────────────────────
    private void BuildTier3()
    {
        container = NewContainer();

        BuildGround(1.0f);

        // Tier 2 ekinleri + kanal
        float[] rowsZ = { -0.42f, -0.28f, -0.14f, 0.14f, 0.28f, 0.42f };
        foreach (float z in rowsZ)
        {
            CreateBox("SoilStrip", new Vector3(0f, 0.09f, z),
                new Vector3(0.9f, 0.06f, 0.14f), "#7A4E2D", Quaternion.identity);
            CreateBox("CropRow", new Vector3(0f, 0.2f, z),
                new Vector3(0.86f, 0.22f, 0.1f), "#6DB33F", Quaternion.identity);
        }
        CreateBox("Canal", new Vector3(0f, 0.07f, 0f),
            new Vector3(0.92f, 0.05f, 0.12f), "#4A90D9", Quaternion.identity);

        // 4 köşede taş/ahşap direk
        Vector3[] corners = {
            new Vector3(-0.48f, 0.32f, -0.48f),
            new Vector3(0.48f, 0.32f, -0.48f),
            new Vector3(-0.48f, 0.32f, 0.48f),
            new Vector3(0.48f, 0.32f, 0.48f),
        };
        foreach (var c in corners)
            CreateBox("GreenhousePost", c,
                new Vector3(0.07f, 0.64f, 0.07f), "#8A7060", Quaternion.identity);

        // Çatı kirişleri (çerçeve)
        CreateBox("BeamFront", new Vector3(0f, 0.64f, 0.5f),
            new Vector3(1.04f, 0.05f, 0.06f), "#7A6050", Quaternion.identity);
        CreateBox("BeamBack", new Vector3(0f, 0.64f, -0.5f),
            new Vector3(1.04f, 0.05f, 0.06f), "#7A6050", Quaternion.identity);
        CreateBox("BeamLeft", new Vector3(-0.5f, 0.64f, 0f),
            new Vector3(0.06f, 0.05f, 1.04f), "#7A6050", Quaternion.identity);
        CreateBox("BeamRight", new Vector3(0.5f, 0.64f, 0f),
            new Vector3(0.06f, 0.05f, 1.04f), "#7A6050", Quaternion.identity);
        CreateBox("BeamRidge", new Vector3(0f, 0.66f, 0f),
            new Vector3(1.04f, 0.05f, 0.06f), "#7A6050", Quaternion.identity);

        // Yarı şeffaf cam çatı panelleri
        float[] panelZ = { -0.34f, 0f, 0.34f };
        foreach (float z in panelZ)
            CreateBox("GlassPanel", new Vector3(0f, 0.66f, z),
                new Vector3(1.0f, 0.04f, 0.34f), "#C8E8F0", Quaternion.identity);

        // Ön yüzde kapı açıklığı (çerçeve)
        CreateBox("DoorPostL", new Vector3(-0.14f, 0.25f, 0.5f),
            new Vector3(0.05f, 0.5f, 0.05f), "#7A6050", Quaternion.identity);
        CreateBox("DoorPostR", new Vector3(0.14f, 0.25f, 0.5f),
            new Vector3(0.05f, 0.5f, 0.05f), "#7A6050", Quaternion.identity);
        CreateBox("DoorLintel", new Vector3(0f, 0.5f, 0.5f),
            new Vector3(0.4f, 0.05f, 0.05f), "#7A6050", Quaternion.identity);
    }

    // ── Ortak yapı taşları ───────────────────────────────────────────────
    private void BuildGround(float size)
    {
        CreateBox("Ground", new Vector3(0f, 0.04f, 0f),
            new Vector3(size, 0.08f, size), "#5C3A1E", Quaternion.identity);
    }

    // Dört kenar boyunca ahşap çit; leaveFrontGap true ise ön (+Z) kenar açık bırakılır
    private void BuildFence(float half, float postW, float railThk, bool leaveFrontGap)
    {
        float postY = 0.17f;
        float postH = 0.3f;
        float railY = 0.22f;
        Vector3 postSize = new Vector3(postW, postH, postW);

        // Köşe direkleri
        CreateBox("FencePost", new Vector3(-half, postY, -half), postSize, "#8B5E3C", Quaternion.identity);
        CreateBox("FencePost", new Vector3(half, postY, -half), postSize, "#8B5E3C", Quaternion.identity);
        CreateBox("FencePost", new Vector3(-half, postY, half), postSize, "#8B5E3C", Quaternion.identity);
        CreateBox("FencePost", new Vector3(half, postY, half), postSize, "#8B5E3C", Quaternion.identity);

        // Kenar orta direkleri
        CreateBox("FencePost", new Vector3(-half, postY, 0f), postSize, "#8B5E3C", Quaternion.identity);
        CreateBox("FencePost", new Vector3(half, postY, 0f), postSize, "#8B5E3C", Quaternion.identity);
        CreateBox("FencePost", new Vector3(0f, postY, -half), postSize, "#8B5E3C", Quaternion.identity);
        if (!leaveFrontGap)
            CreateBox("FencePost", new Vector3(0f, postY, half), postSize, "#8B5E3C", Quaternion.identity);

        // Yatay kirişler
        float span = half * 2f;
        CreateBox("FenceRail", new Vector3(0f, railY, -half),
            new Vector3(span, railThk, railThk), "#A0522D", Quaternion.identity);
        CreateBox("FenceRail", new Vector3(-half, railY, 0f),
            new Vector3(railThk, railThk, span), "#A0522D", Quaternion.identity);
        CreateBox("FenceRail", new Vector3(half, railY, 0f),
            new Vector3(railThk, railThk, span), "#A0522D", Quaternion.identity);
        if (!leaveFrontGap)
            CreateBox("FenceRail", new Vector3(0f, railY, half),
                new Vector3(span, railThk, railThk), "#A0522D", Quaternion.identity);
    }

    // ── BarakaVisual ile birebir aynı yardımcılar ────────────────────────
    private Transform NewContainer()
    {
        Transform c = new GameObject("TarlaVisual").transform;
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
