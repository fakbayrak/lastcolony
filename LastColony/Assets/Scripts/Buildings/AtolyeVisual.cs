using UnityEngine;

public class AtolyeVisual : MonoBehaviour
{
    private void Awake()
    {
        BuildAtolye();
    }

    private void BuildAtolye()
    {
        Color bodyColor   = HexColor("#8A8070"); // gri-bej tuğla gövde
        Color roofColor   = HexColor("#5A5045"); // koyu gri düz çatı
        Color chimneyColor = HexColor("#3C3530"); // antrasit baca
        Color windowColor = HexColor("#C8B87A"); // sarımsı pencere ışığı

        // ── Gövde ────────────────────────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.36f, 0f),
            new Vector3(0.92f, 0.72f, 0.92f),
            bodyColor, Quaternion.identity);

        // ── Düz çatı ─────────────────────────────────────────────────────
        CreateBox("Roof", transform,
            new Vector3(0f, 0.77f, 0f),
            new Vector3(0.96f, 0.1f, 0.96f),
            roofColor, Quaternion.identity);

        // ── Baca (çatı üstü sağ köşe) ────────────────────────────────────
        CreateCylinder("Chimney", transform,
            new Vector3(0.3f, 1.05f, 0.3f),
            new Vector3(0.18f, 0.5f, 0.18f),
            chimneyColor);

        // ── Pencereler (yan duvarlar) ────────────────────────────────────
        CreateBox("WindowL", transform,
            new Vector3(-0.47f, 0.4f, 0f),
            new Vector3(0.15f, 0.15f, 0.04f),
            windowColor, Quaternion.Euler(0f, 90f, 0f));

        CreateBox("WindowR", transform,
            new Vector3(0.47f, 0.4f, 0f),
            new Vector3(0.15f, 0.15f, 0.04f),
            windowColor, Quaternion.Euler(0f, 90f, 0f));
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
        ApplyMaterial(box, color);
        return box;
    }

    private GameObject CreateCylinder(string objName, Transform parent,
        Vector3 localPos, Vector3 size, Color color)
    {
        GameObject cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cyl.name = objName;
        cyl.transform.SetParent(parent);
        cyl.transform.localPosition = localPos;
        cyl.transform.localScale    = size;
        cyl.transform.localRotation = Quaternion.identity;
        Destroy(cyl.GetComponent<Collider>());
        ApplyMaterial(cyl, color);
        return cyl;
    }

    private void ApplyMaterial(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        Material mat  = new Material(Shader.Find("Standard"));
        mat.color     = color;
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
