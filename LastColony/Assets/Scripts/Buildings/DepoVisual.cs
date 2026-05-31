using UnityEngine;

public class DepoVisual : MonoBehaviour
{
    private void Awake()
    {
        BuildDepo();
    }

    private void BuildDepo()
    {
        Color bodyColor = HexColor("#7A5230"); // orta kahve gövde
        Color roofColor = HexColor("#4A2E0E"); // koyu çatı
        Color doorColor = HexColor("#2C1A06"); // büyük kapı

        // ── Gövde ────────────────────────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.275f, 0f),
            new Vector3(1.1f, 0.55f, 0.95f),
            bodyColor, Quaternion.identity);

        // ── Çatı — kemer efekti için merkez düz + 2 yan eğik ─────────────
        CreateBox("RoofCenter", transform,
            new Vector3(0f, 0.65f, 0f),
            new Vector3(1.15f, 0.3f, 0.98f),
            roofColor, Quaternion.identity);

        CreateBox("RoofLeft", transform,
            new Vector3(-0.5f, 0.62f, 0f),
            new Vector3(0.4f, 0.28f, 0.98f),
            roofColor, Quaternion.Euler(0f, 0f, 35f));

        CreateBox("RoofRight", transform,
            new Vector3(0.5f, 0.62f, 0f),
            new Vector3(0.4f, 0.28f, 0.98f),
            roofColor, Quaternion.Euler(0f, 0f, -35f));

        // ── Büyük kapı (ön yüz ortada) ───────────────────────────────────
        CreateBox("Door", transform,
            new Vector3(0f, 0.16f, 0.48f),
            new Vector3(0.3f, 0.32f, 0.05f),
            doorColor, Quaternion.identity);
    }

    private GameObject CreateBox(string objName, Transform parent,
        Vector3 localPos, Vector3 size, Color color, Quaternion rotation)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = objName;
        box.transform.SetParent(parent);
        box.transform.localPosition = localPos;
        box.transform.localScale    = size;
        box.transform.localRotation = rotation;
        Destroy(box.GetComponent<Collider>());

        Renderer rend = box.GetComponent<Renderer>();
        Material mat  = new Material(Shader.Find("Standard"));
        mat.color     = color;
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
