using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private static readonly Color[] skinColors =
    {
        new Color(0.25f, 0.42f, 0.18f), // ork yeşili
        new Color(0.20f, 0.35f, 0.15f), // koyu yeşil
        new Color(0.30f, 0.48f, 0.20f), // açık yeşil
    };

    private void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        Color skin = skinColors[Random.Range(0, skinColors.Length)];
        BuildFigure(skin);
    }

    private void BuildFigure(Color skinColor)
    {
        Color armorColor = new Color(0.20f, 0.18f, 0.15f); // koyu metal zırh
        Color eyeColor   = new Color(0.90f, 0.10f, 0.05f); // kırmızı göz
        Color teethColor = new Color(0.90f, 0.88f, 0.75f); // sarımtırak diş
        Color hornColor  = new Color(0.15f, 0.12f, 0.08f); // siyah boynuz

        // ── Kafa — NPC'den daha büyük ve kare ──────────────────────────────
        GameObject head = CreateBox("Head", transform,
            new Vector3(0f, 0.80f, 0f),
            new Vector3(0.36f, 0.32f, 0.32f),
            skinColor);

        // Kırmızı gözler
        CreateBox("EyeL", head.transform,
            new Vector3(-0.09f, 0.04f, 0.16f),
            new Vector3(0.08f, 0.08f, 0.02f),
            eyeColor);
        CreateBox("EyeR", head.transform,
            new Vector3(0.09f, 0.04f, 0.16f),
            new Vector3(0.08f, 0.08f, 0.02f),
            eyeColor);

        // Dişler (alt çene)
        CreateBox("ToothL", head.transform,
            new Vector3(-0.06f, -0.12f, 0.16f),
            new Vector3(0.05f, 0.07f, 0.02f),
            teethColor);
        CreateBox("ToothR", head.transform,
            new Vector3(0.06f, -0.12f, 0.16f),
            new Vector3(0.05f, 0.07f, 0.02f),
            teethColor);

        // Boynuzlar
        CreateBox("HornL", head.transform,
            new Vector3(-0.12f, 0.20f, 0f),
            new Vector3(0.07f, 0.18f, 0.07f),
            hornColor);
        CreateBox("HornR", head.transform,
            new Vector3(0.12f, 0.20f, 0f),
            new Vector3(0.07f, 0.18f, 0.07f),
            hornColor);

        // ── Gövde — daha geniş ve kısa ─────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.44f, 0f),
            new Vector3(0.42f, 0.30f, 0.22f),
            armorColor);

        // Zırh plakası (göğüs)
        CreateBox("Chest", transform,
            new Vector3(0f, 0.46f, 0.08f),
            new Vector3(0.32f, 0.22f, 0.06f),
            new Color(0.28f, 0.25f, 0.20f));

        // ── Bacaklar — kısa bodur ───────────────────────────────────────────
        CreateBox("LegL", transform,
            new Vector3(-0.11f, 0.15f, 0f),
            new Vector3(0.16f, 0.26f, 0.18f),
            new Color(0.18f, 0.16f, 0.12f));
        CreateBox("LegR", transform,
            new Vector3(0.11f, 0.15f, 0f),
            new Vector3(0.16f, 0.26f, 0.18f),
            new Color(0.18f, 0.16f, 0.12f));

        // Ayaklar
        CreateBox("FootL", transform,
            new Vector3(-0.11f, 0.03f, 0.03f),
            new Vector3(0.16f, 0.07f, 0.20f),
            new Color(0.12f, 0.10f, 0.08f));
        CreateBox("FootR", transform,
            new Vector3(0.11f, 0.03f, 0.03f),
            new Vector3(0.16f, 0.07f, 0.20f),
            new Color(0.12f, 0.10f, 0.08f));

        // ── Kollar — kalın ──────────────────────────────────────────────────
        CreateBox("ArmL", transform,
            new Vector3(-0.30f, 0.42f, 0f),
            new Vector3(0.14f, 0.26f, 0.18f),
            skinColor);
        CreateBox("ArmR", transform,
            new Vector3(0.30f, 0.42f, 0f),
            new Vector3(0.14f, 0.26f, 0.18f),
            skinColor);

        // Omuz zırhı L
        CreateBox("ShoulderL", transform,
            new Vector3(-0.30f, 0.57f, 0f),
            new Vector3(0.18f, 0.10f, 0.22f),
            armorColor);
        // Omuz zırhı R
        CreateBox("ShoulderR", transform,
            new Vector3(0.30f, 0.57f, 0f),
            new Vector3(0.18f, 0.10f, 0.22f),
            armorColor);

        // Omuz sivri çıkıntı L
        CreateBox("SpikeL", transform,
            new Vector3(-0.30f, 0.66f, 0f),
            new Vector3(0.07f, 0.14f, 0.07f),
            hornColor);
        // Omuz sivri çıkıntı R
        CreateBox("SpikeR", transform,
            new Vector3(0.30f, 0.66f, 0f),
            new Vector3(0.07f, 0.14f, 0.07f),
            hornColor);

        // El L (yumruk)
        CreateBox("FistL", transform,
            new Vector3(-0.30f, 0.26f, 0f),
            new Vector3(0.14f, 0.13f, 0.14f),
            skinColor);
        // El R (silah tutan)
        CreateBox("FistR", transform,
            new Vector3(0.30f, 0.26f, 0f),
            new Vector3(0.14f, 0.13f, 0.14f),
            skinColor);

        // Silah — sopa/gürz
        CreateBox("WeaponHandle", transform,
            new Vector3(0.30f, 0.10f, 0f),
            new Vector3(0.06f, 0.28f, 0.06f),
            new Color(0.35f, 0.22f, 0.10f));
        CreateBox("WeaponHead", transform,
            new Vector3(0.30f, -0.04f, 0f),
            new Vector3(0.14f, 0.10f, 0.14f),
            new Color(0.40f, 0.38f, 0.35f));
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
        mat.SetFloat("_Glossiness", 0.05f);
        mat.SetFloat("_Metallic",   0.0f);
        rend.material = mat;

        return box;
    }
}
