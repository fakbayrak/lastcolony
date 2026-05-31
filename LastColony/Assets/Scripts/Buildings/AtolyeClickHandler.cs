using UnityEngine;

public class AtolyeClickHandler : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (ProcessingUI.Instance != null)
            ProcessingUI.Instance.TogglePanel();
    }
}
