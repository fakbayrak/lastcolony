using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material gridLineMaterial;

    [Header("Zemin Renkleri")]
    [SerializeField] private Color grassColor = new Color(0.35f, 0.55f, 0.25f);
    [SerializeField] private Color dirtColor  = new Color(0.55f, 0.42f, 0.28f);
    [SerializeField] private float noiseScale = 3f;

    private void Start()
    {
        GenerateGround();
    }

    private void GenerateGround()
    {
        if (gridManager == null) return;

        int width  = gridManager.Width;
        int height = gridManager.Height;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 worldPos = gridManager.GridToWorld(new Vector2Int(x, z));

                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tile.name = $"Tile_{x}_{z}";
                tile.transform.SetParent(transform);
                tile.transform.position = new Vector3(worldPos.x, -0.01f, worldPos.z);
                tile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                tile.transform.localScale = Vector3.one * 0.98f;

                Destroy(tile.GetComponent<MeshCollider>());

                float noise = Mathf.PerlinNoise(
                    (x + 100f) / noiseScale,
                    (z + 100f) / noiseScale);

                Renderer rend = tile.GetComponent<Renderer>();
                if (groundMaterial != null)
                {
                    Material mat = new Material(groundMaterial);
                    mat.color = Color.Lerp(dirtColor, grassColor, noise);
                    rend.material = mat;
                }
                else
                {
                    rend.material = new Material(Shader.Find("Standard"));
                    rend.material.color = Color.Lerp(dirtColor, grassColor, noise);
                }
            }
        }
    }
}
