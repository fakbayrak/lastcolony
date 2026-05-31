using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private static readonly Color[] skinColors =
    {
        new Color(0.22f, 0.38f, 0.12f), // ork yeşili
        new Color(0.18f, 0.30f, 0.10f), // koyu yeşil
        new Color(0.28f, 0.42f, 0.15f), // açık yeşil
    };

    private void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        // Düşmanı NPC'den büyük göster
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);

        Color skin = skinColors[Random.Range(0, skinColors.Length)];
        BuildFigure(skin);
        AddGroundAura();
    }

    private void BuildFigure(Color skinColor)
    {
        Color armorColor  = new Color(0.55f, 0.05f, 0.05f); // kırmızı zırh
        Color darkArmor   = new Color(0.15f, 0.12f, 0.10f); // koyu metal
        Color eyeColor    = new Color(1.00f, 0.15f, 0.00f); // parlak kırmızı göz
        Color teethColor  = new Color(0.95f, 0.90f, 0.70f); // sarı diş
        Color hornColor   = new Color(0.10f, 0.08f, 0.06f); // siyah boynuz
        Color spikeColor  = new Color(0.65f, 0.08f, 0.08f); // kırmızı sivri

        // ── Kafa ──────────────────────────────────────────────────────────
        GameObject head = CreateBox("Head", transform,
            new Vector3(0f, 0.80f, 0f),
            new Vector3(0.38f, 0.34f, 0.34f),
            skinColor);

        // Parlak kırmızı gözler
        CreateBox("EyeL", head.transform,
            new Vector3(-0.10f, 0.04f, 0.17f),
            new Vector3(0.09f, 0.09f, 0.02f),
            eyeColor);
        CreateBox("EyeR", head.transform,
            new Vector3(0.10f, 0.04f, 0.17f),
            new Vector3(0.09f, 0.09f, 0.02f),
            eyeColor);

        // Dişler
        CreateBox("ToothL", head.transform,
            new Vector3(-0.07f, -0.13f, 0.17f),
            new Vector3(0.06f, 0.08f, 0.02f),
            teethColor);
        CreateBox("ToothR", head.transform,
            new Vector3(0.07f, -0.13f, 0.17f),
            new Vector3(0.06f, 0.08f, 0.02f),
            teethColor);

        // 3 boynuz — ortada büyük, yanlarda küçük
        CreateBox("HornMid", head.transform,
            new Vector3(0f, 0.24f, 0f),
            new Vector3(0.08f, 0.22f, 0.08f),
            hornColor);
        CreateBox("HornL", head.transform,
            new Vector3(-0.14f, 0.20f, 0f),
            new Vector3(0.07f, 0.16f, 0.07f),
            hornColor);
        CreateBox("HornR", head.transform,
            new Vector3(0.14f, 0.20f, 0f),
            new Vector3(0.07f, 0.16f, 0.07f),
            hornColor);

        // ── Gövde — kırmızı zırhlı ───────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.44f, 0f),
            new Vector3(0.44f, 0.30f, 0.22f),
            darkArmor);

        // Kırmızı göğüs zırhı
        CreateBox("ChestArmor", transform,
            new Vector3(0f, 0.47f, 0.09f),
            new Vector3(0.36f, 0.24f, 0.06f),
            armorColor);

        // Omuz zırhları
        CreateBox("ShoulderL", transform,
            new Vector3(-0.32f, 0.57f, 0f),
            new Vector3(0.20f, 0.12f, 0.24f),
            armorColor);
        CreateBox("ShoulderR", transform,
            new Vector3(0.32f, 0.57f, 0f),
            new Vector3(0.20f, 0.12f, 0.24f),
            armorColor);

        // Omuz sivri çıkıntılar
        CreateBox("SpikeL", transform,
            new Vector3(-0.32f, 0.68f, 0f),
            new Vector3(0.08f, 0.18f, 0.08f),
            spikeColor);
        CreateBox("SpikeR", transform,
            new Vector3(0.32f, 0.68f, 0f),
            new Vector3(0.08f, 0.18f, 0.08f),
            spikeColor);

        // ── Bacaklar ─────────────────────────────────────────────────────
        CreateBox("LegL", transform,
            new Vector3(-0.12f, 0.15f, 0f),
            new Vector3(0.17f, 0.26f, 0.18f),
            darkArmor);
        CreateBox("LegR", transform,
            new Vector3(0.12f, 0.15f, 0f),
            new Vector3(0.17f, 0.26f, 0.18f),
            darkArmor);

        // Ayaklar
        CreateBox("FootL", transform,
            new Vector3(-0.12f, 0.03f, 0.03f),
            new Vector3(0.17f, 0.07f, 0.22f),
            new Color(0.10f, 0.08f, 0.06f));
        CreateBox("FootR", transform,
            new Vector3(0.12f, 0.03f, 0.03f),
            new Vector3(0.17f, 0.07f, 0.22f),
            new Color(0.10f, 0.08f, 0.06f));

        // ── Kollar ───────────────────────────────────────────────────────
        CreateBox("ArmL", transform,
            new Vector3(-0.33f, 0.42f, 0f),
            new Vector3(0.15f, 0.26f, 0.18f),
            skinColor);
        CreateBox("ArmR", transform,
            new Vector3(0.33f, 0.42f, 0f),
            new Vector3(0.15f, 0.26f, 0.18f),
            skinColor);

        // Yumruklar
        CreateBox("FistL", transform,
            new Vector3(-0.33f, 0.26f, 0f),
            new Vector3(0.15f, 0.14f, 0.15f),
            skinColor);
        CreateBox("FistR", transform,
            new Vector3(0.33f, 0.26f, 0f),
            new Vector3(0.15f, 0.14f, 0.15f),
            skinColor);

        // ── Silah: büyük balta ───────────────────────────────────────────
        // Sap
        CreateBox("AxeHandle", transform,
            new Vector3(0.38f, 0.20f, 0f),
            new Vector3(0.07f, 0.45f, 0.07f),
            new Color(0.30f, 0.20f, 0.08f));
        // Balta başı
        CreateBox("AxeHead", transform,
            new Vector3(0.50f, 0.38f, 0f),
            new Vector3(0.22f, 0.28f, 0.06f),
            new Color(0.55f, 0.52f, 0.48f));
        // Balta keskin kenar
        CreateBox("AxeBlade", transform,
            new Vector3(0.58f, 0.36f, 0f),
            new Vector3(0.08f, 0.22f, 0.04f),
            new Color(0.85f, 0.82f, 0.78f));
    }

    private void AddGroundAura()
    {
        // Düşmanın altına kırmızı ışıma dairesi
        GameObject aura = GameObject.CreatePrimitive(PrimitiveType.Quad);
        aura.name = "Aura";
        aura.transform.SetParent(transform);
        aura.transform.localPosition = new Vector3(0f, 0.01f / 1.4f, 0f);
        aura.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        aura.transform.localScale    = new Vector3(0.9f, 0.9f, 0.9f);
        Destroy(aura.GetComponent<Collider>());

        Material mat = new Material(Shader.Find("Standard"));
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = 3000;
        mat.color = new Color(0.9f, 0.05f, 0.05f, 0.35f);
        aura.GetComponent<Renderer>().material = mat;
    }

    private GameObject CreateBox(string objName, Transform parent, Vector3 localPos, Vector3 size, Color color)
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
        mat.SetFloat("_Glossiness", 0.15f);
        mat.SetFloat("_Metallic",   0.1f);
        rend.material = mat;

        return box;
    }
}
