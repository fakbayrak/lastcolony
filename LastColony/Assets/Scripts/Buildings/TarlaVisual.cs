using UnityEngine;

public class TarlaVisual : MonoBehaviour, IBuildingVisual
{
    private int currentTier = 1;

    private void Start() => BuildTier1();

    public void UpgradeTo(int tier)
    {
        currentTier = tier;
        foreach (Transform child in transform) Destroy(child.gameObject);
        if (tier == 1) BuildTier1();
        else if (tier == 2) BuildTier2();
        else BuildTier3();
    }

    private void BuildTier1()
    {
        // Zemin — düz toprak tarla
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.SetParent(transform);
        ground.transform.localPosition = new Vector3(0, -0.4f, 0);
        ground.transform.localScale = new Vector3(0.9f, 0.05f, 0.9f);
        SetColor(ground, new Color(0.55f, 0.35f, 0.15f)); // koyu toprak

        // Sıralar — 3 ekin sırası
        for (int i = 0; i < 3; i++)
        {
            GameObject row = GameObject.CreatePrimitive(PrimitiveType.Cube);
            row.transform.SetParent(transform);
            row.transform.localPosition = new Vector3(-0.25f + i * 0.25f, -0.25f, 0);
            row.transform.localScale = new Vector3(0.08f, 0.2f, 0.7f);
            SetColor(row, new Color(0.3f, 0.7f, 0.2f)); // yeşil ekin
        }
    }

    private void BuildTier2()
    {
        BuildTier1();
        // Sulama kanalı — mavi şerit
        GameObject canal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        canal.transform.SetParent(transform);
        canal.transform.localPosition = new Vector3(0, -0.37f, 0.4f);
        canal.transform.localScale = new Vector3(0.9f, 0.04f, 0.1f);
        SetColor(canal, new Color(0.2f, 0.5f, 0.9f)); // mavi su

        // 5 ekin sırası
        for (int i = 0; i < 5; i++)
        {
            GameObject row = GameObject.CreatePrimitive(PrimitiveType.Cube);
            row.transform.SetParent(transform);
            row.transform.localPosition = new Vector3(-0.4f + i * 0.2f, -0.2f, 0);
            row.transform.localScale = new Vector3(0.07f, 0.25f, 0.7f);
            SetColor(row, new Color(0.2f, 0.8f, 0.15f));
        }
    }

    private void BuildTier3()
    {
        BuildTier2();
        // Sera çatısı — yarı şeffaf
        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.transform.SetParent(transform);
        roof.transform.localPosition = new Vector3(0, 0.2f, 0);
        roof.transform.localScale = new Vector3(0.95f, 0.05f, 0.95f);
        SetColor(roof, new Color(0.8f, 0.95f, 1f)); // açık mavi cam

        // Çerçeve direkleri
        Vector3[] posts = { new Vector3(-0.4f,0,-0.4f), new Vector3(0.4f,0,-0.4f),
                            new Vector3(-0.4f,0,0.4f),  new Vector3(0.4f,0,0.4f) };
        foreach (var pos in posts)
        {
            GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
            post.transform.SetParent(transform);
            post.transform.localPosition = pos;
            post.transform.localScale = new Vector3(0.05f, 0.6f, 0.05f);
            SetColor(post, new Color(0.7f, 0.7f, 0.7f));
        }
    }

    private void SetColor(GameObject obj, Color color)
    {
        Renderer r = obj.GetComponent<Renderer>();
        if (r == null) return;
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        mat.SetFloat("_Glossiness", 0.2f);
        mat.SetFloat("_Metallic", 0.0f);
        r.material = mat;
    }
}
