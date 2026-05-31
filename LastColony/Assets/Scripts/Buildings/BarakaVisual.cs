using UnityEngine;

public class BarakaVisual : MonoBehaviour
{
    private Transform container;

    private void Awake()
    {
        BuildBaraka();
    }

    private void BuildBaraka()
    {
        container = new GameObject("BarakaVisual").transform;
        container.SetParent(transform, false);

        // ── Gövde ────────────────────────────────────────────────────────
        CreateBox("Body", new Vector3(0f, 0.34f, 0f),
            new Vector3(0.95f, 0.68f, 0.95f), "#A0785A", Quaternion.identity);
        CreateBox("WoodLine1", new Vector3(0f, 0.22f, 0f),
            new Vector3(0.96f, 0.04f, 0.96f), "#7A5535", Quaternion.identity);
        CreateBox("WoodLine2", new Vector3(0f, 0.46f, 0f),
            new Vector3(0.96f, 0.04f, 0.96f), "#7A5535", Quaternion.identity);

        // ── Çatı (saman) ─────────────────────────────────────────────────
        CreateBox("RoofLeft", new Vector3(-0.32f, 0.98f, 0f),
            new Vector3(0.07f, 0.62f, 1.02f), "#D4A843", Quaternion.Euler(0f, 0f, -40f));
        CreateBox("RoofRight", new Vector3(0.32f, 0.98f, 0f),
            new Vector3(0.07f, 0.62f, 1.02f), "#D4A843", Quaternion.Euler(0f, 0f, 40f));
        CreateBox("RoofRidge", new Vector3(0f, 1.22f, 0f),
            new Vector3(0.12f, 0.1f, 1.04f), "#B8902E", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 0.87f, 0.48f),
            new Vector3(0.95f, 0.38f, 0.07f), "#C49A35", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 0.87f, -0.48f),
            new Vector3(0.95f, 0.38f, 0.07f), "#C49A35", Quaternion.identity);
        CreateBox("EaveFront", new Vector3(0f, 0.7f, 0.5f),
            new Vector3(1.02f, 0.06f, 0.08f), "#B8902E", Quaternion.identity);
        CreateBox("EaveBack", new Vector3(0f, 0.7f, -0.5f),
            new Vector3(1.02f, 0.06f, 0.08f), "#B8902E", Quaternion.identity);

        // ── Kapı ─────────────────────────────────────────────────────────
        CreateBox("DoorFrame", new Vector3(0f, 0.18f, 0.49f),
            new Vector3(0.24f, 0.36f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("DoorInner", new Vector3(0f, 0.17f, 0.51f),
            new Vector3(0.18f, 0.3f, 0.05f), "#3B2507", Quaternion.identity);
        CreateBox("DoorHandle", new Vector3(0.07f, 0.17f, 0.53f),
            new Vector3(0.03f, 0.03f, 0.04f), "#C8A030", Quaternion.identity);

        // ── Pencereler ───────────────────────────────────────────────────
        CreateBox("WindowLFrame", new Vector3(-0.28f, 0.42f, 0.49f),
            new Vector3(0.2f, 0.18f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("WindowLGlass", new Vector3(-0.28f, 0.42f, 0.51f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);
        CreateBox("WindowRFrame", new Vector3(0.28f, 0.42f, 0.49f),
            new Vector3(0.2f, 0.18f, 0.06f), "#5C3515", Quaternion.identity);
        CreateBox("WindowRGlass", new Vector3(0.28f, 0.42f, 0.51f),
            new Vector3(0.14f, 0.12f, 0.04f), "#A8D8EA", Quaternion.identity);
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

        Renderer rend = box.GetComponent<Renderer>();
        Material mat  = new Material(Shader.Find("Standard"));
        mat.color     = HexColor(hex);
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
