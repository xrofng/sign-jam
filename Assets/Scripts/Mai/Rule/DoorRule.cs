using UnityEngine;

[CreateAssetMenu(fileName = "DoorRule", menuName = "ProceduralHouse/Rule/Door Rule")]
public class DoorRule : ProceduralRule
{
    [Header("Door Settings")]
    [Tooltip("Vertical offset from bottom margin for door placement.")]
    public float doorVerticalOffset = 0.1f;

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Action<HouseObjectData, Vector3> spawn
    )
    {
        HouseObjectData doorData = generator.houseData.GetObject(objectType);
        if (doorData == null || doorData.objectPrefab == null)
        {
            Debug.LogWarning($"Door object data not found for rule: {name}");
            return;
        }

        // Calculate bottom center position using generator's margins
        float bottomY = -halfSize.y + generator.bottomMargin + doorVerticalOffset;
        Vector3 doorPos = new Vector3(0, bottomY, 0);

        spawn(doorData, doorPos);
    }
}