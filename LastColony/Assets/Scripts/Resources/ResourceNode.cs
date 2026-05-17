using System.Collections;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int totalAmount = 100;
    [SerializeField] private int gatherAmountPerTrip = 10;
    [SerializeField] private float gatherDuration = 2f;

    public bool IsAvailable() => totalAmount > 0;

    public void Gather(NPC npc)
    {
        StartCoroutine(GatherCoroutine(npc));
    }

    private IEnumerator GatherCoroutine(NPC npc)
    {
        yield return new WaitForSeconds(gatherDuration);

        int gathered = Mathf.Min(gatherAmountPerTrip, totalAmount);
        ResourceManager.Instance.AddResource(resourceType, gathered);
        totalAmount -= gathered;

        npc.SetIdle();

        if (totalAmount <= 0)
            Destroy(gameObject);
    }
}
