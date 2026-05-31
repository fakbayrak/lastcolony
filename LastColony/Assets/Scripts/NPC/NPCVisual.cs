using UnityEngine;

public class NPCVisual : MonoBehaviour
{
    [Header("Renk Varyasyonu")]
    [SerializeField] private bool randomizeColor = true;

    private static readonly Color[] shirtColors =
    {
        new Color(0.80f, 0.30f, 0.20f), // kırmızı
        new Color(0.20f, 0.45f, 0.75f), // mavi
        new Color(0.25f, 0.60f, 0.25f), // yeşil
        new Color(0.75f, 0.60f, 0.15f), // sarı
        new Color(0.55f, 0.25f, 0.65f), // mor
        new Color(0.80f, 0.50f, 0.15f), // turuncu
    };

    private void Start()
    {
        // Mevcut MeshRenderer'ı gizle (kapsül/silindir)
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        // Varsa Capsule/Mesh Collider bırak (NPC hareketi için gerekebilir)

        Color shirt = shirtColors[Random.Range(0, shirtColors.Length)];
        if (!randomizeColor) shirt = shirtColors[0];

        BuildFigure(shirt);
    }

    private void BuildFigure(Color shirtColor)
    {
        // Renk paleti
        Color skinColor  = new Color(0.90f, 0.75f, 0.60f);
        Color pantsColor = new Color(0.25f, 0.25f, 0.40f);
        Color hairColor  = new Color(0.25f, 0.18f, 0.10f);
        Color shoeColor  = new Color(0.20f, 0.15f, 0.10f);

        // ── Kafa ──────────────────────────────────────────────────────────────
        GameObject head = CreateBox("Head", transform,
            new Vector3(0f, 0.75f, 0f),
            new Vector3(0.28f, 0.28f, 0.28f),
            skinColor);

        // Saç (kafanın üstü)
        CreateBox("Hair", transform,
            new Vector3(0f, 0.92f, 0f),
            new Vector3(0.30f, 0.10f, 0.30f),
            hairColor);

        // Göz L
        CreateBox("EyeL", head.transform,
            new Vector3(-0.08f, 0.02f, 0.14f),
            new Vector3(0.06f, 0.06f, 0.02f),
            new Color(0.1f, 0.1f, 0.1f));

        // Göz R
        CreateBox("EyeR", head.transform,
            new Vector3(0.08f, 0.02f, 0.14f),
            new Vector3(0.06f, 0.06f, 0.02f),
            new Color(0.1f, 0.1f, 0.1f));

        // ── Gövde ─────────────────────────────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.40f, 0f),
            new Vector3(0.30f, 0.28f, 0.18f),
            shirtColor);

        // ── Bacaklar ──────────────────────────────────────────────────────────
        CreateBox("LegL", transform,
            new Vector3(-0.08f, 0.13f, 0f),
            new Vector3(0.12f, 0.24f, 0.14f),
            pantsColor);

        CreateBox("LegR", transform,
            new Vector3(0.08f, 0.13f, 0f),
            new Vector3(0.12f, 0.24f, 0.14f),
            pantsColor);

        // Ayakkabı L
        CreateBox("ShoeL", transform,
            new Vector3(-0.08f, 0.02f, 0.02f),
            new Vector3(0.12f, 0.06f, 0.16f),
            shoeColor);

        // Ayakkabı R
        CreateBox("ShoeR", transform,
            new Vector3(0.08f, 0.02f, 0.02f),
            new Vector3(0.12f, 0.06f, 0.16f),
            shoeColor);

        // ── Kollar ────────────────────────────────────────────────────────────
        CreateBox("ArmL", transform,
            new Vector3(-0.22f, 0.38f, 0f),
            new Vector3(0.11f, 0.24f, 0.14f),
            shirtColor);

        CreateBox("ArmR", transform,
            new Vector3(0.22f, 0.38f, 0f),
            new Vector3(0.11f, 0.24f, 0.14f),
            shirtColor);

        // El L
        CreateBox("HandL", transform,
            new Vector3(-0.22f, 0.24f, 0f),
            new Vector3(0.10f, 0.10f, 0.10f),
            skinColor);

        // El R
        CreateBox("HandR", transform,
            new Vector3(0.22f, 0.24f, 0f),
            new Vector3(0.10f, 0.10f, 0.10f),
            skinColor);
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
