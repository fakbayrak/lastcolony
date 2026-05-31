using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingClickDetector : MonoBehaviour
{
    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BuildingData[] buildingDataList;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            CheckClick();
    }

    private void CheckClick()
    {
        // Placement modundaysa bina bilgisi açma
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (!groundPlane.Raycast(ray, out float dist)) return;

        Vector3 hitPoint = ray.GetPoint(dist);
        Vector2Int clickedGrid = GridManager.Instance.WorldToGrid(hitPoint);

        var placedBuildings = buildingPlacement.GetPlacedBuildings();
        foreach (var record in placedBuildings)
        {
            if (record.gridX == clickedGrid.x && record.gridY == clickedGrid.y)
            {
                BuildingData data = FindBuildingData(record.buildingType);
                if (data != null)
                {
                    if (BuildingInfoUI.Instance.IsVisible())
                        BuildingInfoUI.Instance.HidePanel();
                    else
                        BuildingInfoUI.Instance.ShowPanel(data,
                            new Vector2Int(record.gridX, record.gridY));
                }
                return;
            }
        }

        // Boş yere tıklandıysa paneli kapat
        BuildingInfoUI.Instance?.HidePanel();
    }

    private BuildingData FindBuildingData(string buildingName)
    {
        foreach (var data in buildingDataList)
        {
            if (data.buildingName == buildingName)
                return data;
        }
        return null;
    }
}
