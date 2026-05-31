using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingClickDetector : MonoBehaviour
{
    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            CheckClick();
    }

    private void CheckClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        // Tıklanan objenin grid pozisyonunu bul
        var placedBuildings = buildingPlacement.GetPlacedBuildings();
        foreach (var record in placedBuildings)
        {
            if (record.buildingType == "Atolye" || record.buildingType == "Atölye")
            {
                Vector3 worldPos = GridManager.Instance.GridToWorld(record.gridX, record.gridY);
                float dist = Vector3.Distance(hit.point, worldPos);
                if (dist < 1.0f)
                {
                    ProcessingUI.Instance?.TogglePanel();
                    return;
                }
            }
        }
    }
}
