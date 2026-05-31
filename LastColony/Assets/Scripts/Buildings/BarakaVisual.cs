using UnityEngine;

public class BarakaVisual : MonoBehaviour
{
    private void Awake()
    {
        BuildBaraka();
    }

    private void BuildBaraka()
    {
        Color bodyColor   = HexColor("#8B6340"); // kahverengi gövde
        Color gableColor  = HexColor("#7A5530"); // alın duvarı
        Color roofColor   = HexColor("#5C3D1E"); // çatı plakaları
        Color doorColor   = HexColor("#3B2507"); // kapı
        Color windowColor = HexColor("#C8E8FF"); // pencere camı

        // ── Ana gövde ────────────────────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.3f, 0f),
            new Vector3(0.82f, 0.6f, 0.82f),
            bodyColor, Quaternion.identity);

        // ── Alın duvarları (üçgen çatı duvarı) ───────────────────────────
        CreateBox("GableFront", transform,
            new Vector3(0f, 0.81f, 0.38f),
            new Vector3(0.82f, 0.42f, 0.06f),
            gableColor, Quaternion.identity);

        CreateBox("GableBack", transform,
            new Vector3(0f, 0.81f, -0.38f),
            new Vector3(0.82f, 0.42f, 0.06f),
            gableColor, Quaternion.identity);

        // ── Çatı plakaları ───────────────────────────────────────────────
        CreateBox("RoofLeft", transform,
            new Vector3(-0.3f, 0.88f, 0f),
            new Vector3(0.06f, 0.52f, 0.92f),
            roofColor, Quaternion.Euler(0f, 0f, -38f));

        CreateBox("RoofRight", transform,
            new Vector3(0.3f, 0.88f, 0f),
            new Vector3(0.06f, 0.52f, 0.92f),
            roofColor, Quaternion.Euler(0f, 0f, 38f));

        // ── Kapı ─────────────────────────────────────────────────────────
        CreateBox("Door", transform,
            new Vector3(0f, 0.12f, 0.42f),
            new Vector3(0.16f, 0.24f, 0.05f),
            doorColor, Quaternion.identity);

        // ── Pencereler ───────────────────────────────────────────────────
        CreateBox("WindowL", transform,
            new Vector3(-0.22f, 0.38f, 0.42f),
            new Vector3(0.16f, 0.14f, 0.05f),
            windowColor, Quaternion.identity);

        CreateBox("WindowR", transform,
            new Vector3(0.22f, 0.38f, 0.42f),
            new Vector3(0.16f, 0.14f, 0.05f),
            windowColor, Quaternion.identity);
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
