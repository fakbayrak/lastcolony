using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacement : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject buildingPrefab;

    private Camera mainCamera;
    private GameObject highlightQuad;
    private Material highlightMaterial;
    private bool isPlacingBuilding;

    private static readonly Color colorValid   = new Color(0f, 1f, 0f, 0.4f);
    private static readonly Color colorInvalid = new Color(1f, 0f, 0f, 0.4f);

    void Start()
    {
        mainCamera = Camera.main;
        InitHighlight();
        isPlacingBuilding = true;
    }

    void InitHighlight()
    {
        highlightQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(highlightQuad.GetComponent<Collider>());
        highlightQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        highlightMaterial = new Material(Shader.Find("Standard"));
        SetMaterialTransparent(highlightMaterial);
        highlightQuad.GetComponent<Renderer>().material = highlightMaterial;

        highlightQuad.SetActive(false);
    }

    void Update()
    {
        if (!isPlacingBuilding) return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ExitPlacementMode();
            return;
        }

        Vector2Int? gridPos = GetMouseGridPosition();
        if (!gridPos.HasValue)
        {
            highlightQuad.SetActive(false);
            return;
        }

        UpdateHighlight(gridPos.Value);

        if (Mouse.current.leftButton.wasPressedThisFrame)
            TryPlace(gridPos.Value);
    }

    Vector2Int? GetMouseGridPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float dist))
            return gridManager.WorldToGrid(ray.GetPoint(dist));

        return null;
    }

    void UpdateHighlight(Vector2Int gridPos)
    {
        Vector3 center = gridManager.GridToWorld(gridPos.x, gridPos.y);
        highlightQuad.transform.position = new Vector3(center.x, 0.01f, center.z);
        highlightQuad.SetActive(true);

        highlightMaterial.color = gridManager.IsCellOccupied(gridPos.x, gridPos.y)
            ? colorInvalid
            : colorValid;
    }

    void TryPlace(Vector2Int gridPos)
    {
        if (gridManager.IsCellOccupied(gridPos.x, gridPos.y)) return;

        Vector3 center = gridManager.GridToWorld(gridPos.x, gridPos.y);
        GameObject building = buildingPrefab != null
            ? Instantiate(buildingPrefab, center, Quaternion.identity)
            : CreateDefaultCube(center);

        gridManager.OccupyCell(gridPos.x, gridPos.y, building);
    }

    GameObject CreateDefaultCube(Vector3 position)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = Vector3.one;
        return cube;
    }

    public void EnterPlacementMode()
    {
        isPlacingBuilding = true;
    }

    void ExitPlacementMode()
    {
        isPlacingBuilding = false;
        highlightQuad.SetActive(false);
    }

    void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }

    void OnDestroy()
    {
        if (highlightQuad != null) Destroy(highlightQuad);
        if (highlightMaterial != null) Destroy(highlightMaterial);
    }
}
