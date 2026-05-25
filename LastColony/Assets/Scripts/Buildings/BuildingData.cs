using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "LastColony/BuildingData")]
public class BuildingData : ScriptableObject
{
    [Header("Temel Bilgiler")]
    public string buildingName;
    public string buildingNameTR;
    public Sprite toolbarIcon;
    public GameObject prefab;

    [Header("Maliyetler")]
    public int costLumber;
    public int costProcessedStone;
    public int costMetal;

    [Header("Açıklama")]
    [TextArea(2, 4)]
    public string description;
}
