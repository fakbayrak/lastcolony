using System.Collections;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int gatherAmountPerTrip = 8;
    [SerializeField] private float gatherDuration = 4f;

    public ResourceType ResourceType => resourceType;

    public bool IsGathering { get; private set; }

    // Node artık sonsuz kaynak; her zaman toplanabilir
    public bool IsAvailable() => true;

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

        // NPC bu node'da çalışmaya devam ettiği sürece periyodik olarak topla — node tükenmez
        while (npc != null && npc.Health > 0 && npc.CurrentState == NPCState.Working)
        {
            yield return new WaitForSeconds(gatherDuration);

            if (npc == null || npc.Health <= 0 || npc.CurrentState != NPCState.Working)
                break;

            ResourceManager.Instance.AddResource(resourceType, gatherAmountPerTrip);
            Debug.Log($"[ResourceNode] {npc.name} topladı: {gatherAmountPerTrip}x {resourceType} (sonsuz kaynak)");
        }

        IsGathering = false;
    }
}
