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
            new Vector3(0f, 0.25f, 0f),
            new Vector3(0.8f, 0.5f, 0.8f),
            bodyColor, Quaternion.identity);

        // ── Çatı — üçgen prism efekti için 2 eğik plaka ──────────────────
        CreateBox("RoofA", transform,
            new Vector3(0f, 0.62f, 0f),
            new Vector3(0.9f, 0.3f, 0.5f),
            roofColor, Quaternion.Euler(0f, 25f, 0f));

        CreateBox("RoofB", transform,
            new Vector3(0f, 0.62f, 0f),
            new Vector3(0.9f, 0.3f, 0.5f),
            roofColor, Quaternion.Euler(0f, -25f, 0f));

        // ── Kapı (ön yüz) ────────────────────────────────────────────────
        CreateBox("Door", transform,
            new Vector3(0f, 0.15f, 0.41f),
            new Vector3(0.15f, 0.2f, 0.05f),
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
