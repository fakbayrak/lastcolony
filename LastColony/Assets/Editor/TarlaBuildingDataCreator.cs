using UnityEditor;
using UnityEngine;

public class TarlaBuildingDataCreator
{
    [MenuItem("LastColony/Create Tarla BuildingData")]
    public static void CreateTarlaBuildingData()
    {
        BuildingData data = ScriptableObject.CreateInstance<BuildingData>();
        data.buildingName = "Tarla";
        data.buildingNameTR = "Tarla";
        data.description = "Yiyecek üretir. Her gün koloni erzağına katkı sağlar.";
        data.costLumber = 20;
        data.costProcessedStone = 0;
        data.costMetal = 0;

        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");

        AssetDatabase.CreateAsset(data, "Assets/ScriptableObjects/Tarla.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Tarla.asset created.");
    }
}
