using UnityEngine;

public class DepoVisual : MonoBehaviour
{
    private void Awake()
    {
        BuildDepo();
    }

    private void BuildDepo()
    {
        Color bodyColor   = HexColor("#7A5230"); // orta kahve gövde
        Color roofColor   = HexColor("#4A2E0E"); // çatı plakaları
        Color ridgeColor  = HexColor("#3A1E00"); // mahya
        Color gableColor  = HexColor("#6B4520"); // alın duvarı
        Color doorColor   = HexColor("#2C1A06"); // büyük kapı
        Color windowColor = HexColor("#C8E8FF"); // pencere camı

        // ── Ana gövde ────────────────────────────────────────────────────
        CreateBox("Body", transform,
            new Vector3(0f, 0.26f, 0f),
            new Vector3(1.1f, 0.52f, 0.92f),
            bodyColor, Quaternion.identity);

        // ── Çatı plakaları (eğimli) ──────────────────────────────────────
        CreateBox("RoofLeft", transform,
            new Vector3(-0.28f, 0.62f, 0f),
            new Vector3(0.62f, 0.08f, 0.96f),
            roofColor, Quaternion.Euler(0f, 0f, -32f));

        CreateBox("RoofRight", transform,
            new Vector3(0.28f, 0.62f, 0f),
            new Vector3(0.62f, 0.08f, 0.96f),
            roofColor, Quaternion.Euler(0f, 0f, 32f));

        // ── Mahya (çatı tepe) ────────────────────────────────────────────
        CreateBox("RoofRidge", transform,
            new Vector3(0f, 0.8f, 0f),
            new Vector3(0.1f, 0.08f, 0.96f),
            ridgeColor, Quaternion.identity);

        // ── Alın duvarları ───────────────────────────────────────────────
        CreateBox("GableFront", transform,
            new Vector3(0f, 0.62f, 0.46f),
            new Vector3(1.1f, 0.35f, 0.06f),
            gableColor, Quaternion.identity);

        CreateBox("GableBack", transform,
            new Vector3(0f, 0.62f, -0.46f),
            new Vector3(1.1f, 0.35f, 0.06f),
            gableColor, Quaternion.identity);

        // ── Büyük kapı ───────────────────────────────────────────────────
        CreateBox("Door", transform,
            new Vector3(0f, 0.17f, 0.49f),
            new Vector3(0.32f, 0.34f, 0.05f),
            doorColor, Quaternion.identity);

        // ── Küçük pencereler ─────────────────────────────────────────────
        CreateBox("WindowL", transform,
            new Vector3(-0.3f, 0.38f, 0.49f),
            new Vector3(0.14f, 0.12f, 0.05f),
            windowColor, Quaternion.identity);

        CreateBox("WindowR", transform,
            new Vector3(0.3f, 0.38f, 0.49f),
            new Vector3(0.14f, 0.12f, 0.05f),
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
