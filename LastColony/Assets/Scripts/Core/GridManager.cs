using UnityEngine;

[System.Serializable]
public class GridCell
{
    public bool isOccupied;
    public GameObject occupyingObject;
}

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public int width = 20;
    public int height = 20;

    public int Width  => width;
    public int Height => height;

    private GridCell[,] grid;

    void Awake()
    {
        Instance = this;

        grid = new GridCell[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = new GridCell();
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.z);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(x + 0.5f, 0f, y + 0.5f);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return GridToWorld(gridPos.x, gridPos.y);
    }

    public void OccupyCell(int x, int y, GameObject obj)
    {
        if (!IsInBounds(x, y)) return;
        grid[x, y].isOccupied = true;
        grid[x, y].occupyingObject = obj;
    }

    public void FreeCell(int x, int y)
    {
        if (!IsInBounds(x, y)) return;
        grid[x, y].isOccupied = false;
        grid[x, y].occupyingObject = null;
    }

    public bool IsCellOccupied(int x, int y)
    {
        if (!IsInBounds(x, y)) return true;
        return grid[x, y].isOccupied;
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
        for (int x = 0; x <= width; x++)
            Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, 0, height));
        for (int y = 0; y <= height; y++)
            Gizmos.DrawLine(new Vector3(0, 0, y), new Vector3(width, 0, y));
    }
}
