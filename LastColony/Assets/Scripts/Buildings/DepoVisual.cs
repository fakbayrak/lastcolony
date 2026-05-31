using UnityEngine;

public class DepoVisual : MonoBehaviour
{
    private Transform container;

    private void Awake()
    {
        BuildDepo();
    }

    private void BuildDepo()
    {
        container = new GameObject("DepoVisual").transform;
        container.SetParent(transform, false);

        // ── Temel ve gövde ───────────────────────────────────────────────
        CreateBox("StoneFoundation", new Vector3(0f, 0.05f, 0f),
            new Vector3(1.18f, 0.1f, 1.0f), "#8A8070", Quaternion.identity);
        CreateBox("BodyLower", new Vector3(0f, 0.35f, 0f),
            new Vector3(1.12f, 0.5f, 0.96f), "#8B5E35", Quaternion.identity);
        CreateBox("BodyUpper", new Vector3(0f, 0.76f, 0f),
            new Vector3(1.12f, 0.32f, 0.96f), "#7A5230", Quaternion.identity);
        CreateBox("FloorStrip", new Vector3(0f, 0.615f, 0f),
            new Vector3(1.14f, 0.05f, 0.98f), "#5A3515", Quaternion.identity);
        CreateBox("BeamLeft", new Vector3(-0.5f, 0.51f, 0f),
            new Vector3(0.05f, 0.82f, 0.97f), "#5A3515", Quaternion.identity);
        CreateBox("BeamRight", new Vector3(0.5f, 0.51f, 0f),
            new Vector3(0.05f, 0.82f, 0.97f), "#5A3515", Quaternion.identity);
        CreateBox("BeamMid", new Vector3(0f, 0.51f, 0f),
            new Vector3(0.05f, 0.82f, 0.97f), "#5A3515", Quaternion.identity);

        // ── Çatı (kırma çatı) ────────────────────────────────────────────
        CreateBox("RoofLowerLeft", new Vector3(-0.38f, 0.97f, 0f),
            new Vector3(0.48f, 0.08f, 1.0f), "#5C3D1E", Quaternion.Euler(0f, 0f, -28f));
        CreateBox("RoofLowerRight", new Vector3(0.38f, 0.97f, 0f),
            new Vector3(0.48f, 0.08f, 1.0f), "#5C3D1E", Quaternion.Euler(0f, 0f, 28f));
        CreateBox("RoofUpperLeft", new Vector3(-0.18f, 1.2f, 0f),
            new Vector3(0.35f, 0.08f, 1.0f), "#4A2E0E", Quaternion.Euler(0f, 0f, -55f));
        CreateBox("RoofUpperRight", new Vector3(0.18f, 1.2f, 0f),
            new Vector3(0.35f, 0.08f, 1.0f), "#4A2E0E", Quaternion.Euler(0f, 0f, 55f));
        CreateBox("RoofRidge", new Vector3(0f, 1.38f, 0f),
            new Vector3(0.1f, 0.08f, 1.0f), "#3A1E00", Quaternion.identity);
        CreateBox("GableFront", new Vector3(0f, 1.05f, 0.49f),
            new Vector3(1.12f, 0.5f, 0.07f), "#6B4520", Quaternion.identity);
        CreateBox("GableBack", new Vector3(0f, 1.05f, -0.49f),
            new Vector3(1.12f, 0.5f, 0.07f), "#6B4520", Quaternion.identity);

        // ── Büyük kapı (çift kanat) ──────────────────────────────────────
        CreateBox("DoorLeft", new Vector3(-0.11f, 0.31f, 0.52f),
            new Vector3(0.2f, 0.42f, 0.06f), "#3B2007", Quaternion.identity);
        CreateBox("DoorRight", new Vector3(0.11f, 0.31f, 0.52f),
            new Vector3(0.2f, 0.42f, 0.06f), "#3B2007", Quaternion.identity);
        CreateBox("DoorArch", new Vector3(0f, 0.54f, 0.52f),
            new Vector3(0.44f, 0.08f, 0.06f), "#2C1500", Quaternion.identity);
        CreateBox("DoorHandleLeft", new Vector3(-0.02f, 0.31f, 0.54f),
            new Vector3(0.03f, 0.03f, 0.05f), "#C8A030", Quaternion.identity);
        CreateBox("DoorHandleRight", new Vector3(0.02f, 0.31f, 0.54f),
            new Vector3(0.03f, 0.03f, 0.05f), "#C8A030", Quaternion.identity);

        // ── Pencereler (yan duvarlar) ────────────────────────────────────
        CreateBox("WindowLeft", new Vector3(-0.57f, 0.7f, 0.2f),
            new Vector3(0.06f, 0.18f, 0.22f), "#A8D8EA", Quaternion.identity);
        CreateBox("WindowRight", new Vector3(0.57f, 0.7f, 0.2f),
            new Vector3(0.06f, 0.18f, 0.22f), "#A8D8EA", Quaternion.identity);
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
