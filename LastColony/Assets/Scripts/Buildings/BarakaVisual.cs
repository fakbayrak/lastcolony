using UnityEngine;

public class BarakaVisual : MonoBehaviour
{
    private void Awake()
    {
        BuildBaraka();
    }

    private void BuildBaraka()
    {
        Color bodyColor = HexColor("#8B6340"); // kahverengi gövde
        Color roofColor = HexColor("#5C3D1E"); // koyu kahve çatı
        Color doorColor = HexColor("#3B2507"); // çok koyu kahve kapı

        // ── Gövde ────────────────────────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.325f, 0f),
            new Vector3(0.85f, 0.65f, 0.85f),
            bodyColor, Quaternion.identity);

        // ── Çatı — orta düz + 2 yan eğik plaka ───────────────────────────
        CreateBox("RoofMid", transform,
            new Vector3(0f, 0.8f, 0f),
            new Vector3(0.5f, 0.45f, 0.88f),
            roofColor, Quaternion.identity);

        CreateBox("RoofLeft", transform,
            new Vector3(-0.3f, 0.8f, 0f),
            new Vector3(0.35f, 0.45f, 0.88f),
            roofColor, Quaternion.Euler(0f, 28f, 0f));

        CreateBox("RoofRight", transform,
            new Vector3(0.3f, 0.8f, 0f),
            new Vector3(0.35f, 0.45f, 0.88f),
            roofColor, Quaternion.Euler(0f, -28f, 0f));

        // ── Kapı (ön yüz) ────────────────────────────────────────────────
        CreateBox("Door", transform,
            new Vector3(0f, 0.18f, 0.43f),
            new Vector3(0.18f, 0.28f, 0.05f),
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
