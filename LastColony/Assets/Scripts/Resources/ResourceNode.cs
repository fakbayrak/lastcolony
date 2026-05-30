using System.Collections;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int totalAmount = 100;
    [SerializeField] private int gatherAmountPerTrip = 10;
    [SerializeField] private float gatherDuration = 2f;

    public ResourceType ResourceType => resourceType;

    public bool IsGathering { get; private set; }

    public bool IsAvailable() => totalAmount > 0;

    // NPC bu node'a varıp Working state'e geçince çağrılır.
    // Coroutine NPC'de değil node'da çalışır; böylece NPC ölse bile IsGathering takılı kalmaz.
    public void Gather(NPC npc)
    {
        if (IsGathering)
            return;
        StartCoroutine(GatherRoutine(npc));
    }

    private IEnumerator GatherRoutine(NPC npc)
    {
        IsGathering = true;

        // NPC bu node'da çalışmaya devam ettiği sürece periyodik olarak topla
        while (totalAmount > 0 && npc != null && npc.Health > 0 && npc.CurrentState == NPCState.Working)
        {
            yield return new WaitForSeconds(gatherDuration);

            if (npc == null || npc.Health <= 0 || npc.CurrentState != NPCState.Working)
                break;

            int amount = Mathf.Min(gatherAmountPerTrip, totalAmount);
            totalAmount -= amount;

            ResourceManager.Instance.AddResource(resourceType, amount);
            Debug.Log($"[ResourceNode] {npc.name} topladı: {amount}x {resourceType} (kalan: {totalAmount})");

            if (totalAmount <= 0)
            {
                Debug.Log($"[ResourceNode] {resourceType} tükendi.");
                if (npc != null && npc.Health > 0)
                    npc.SetIdle();
                IsGathering = false;
                Destroy(gameObject);
                yield break;
            }
        }

        IsGathering = false;
    }
}
