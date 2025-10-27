using UnityEngine;

[CreateAssetMenu(fileName = "DoorRule", menuName = "Procedural/Rule/Door Rule")]
public class DoorRule : ProceduralRule
{
    [Header("Door Settings")]
    public float doorVerticalOffset = 0.1f;

    // A key to identify the door's data for other rules
    public const string DoorBoundsKey = "MainDoor";

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        HouseObjectData doorData = generator.houseData.GetObject(objectType);
        if (doorData == null || doorData.objectPrefab == null) return;

        // Calculate position
        float bottomY = -halfSize.y + generator.bottomMargin + doorVerticalOffset;
        Vector3 doorPos = new Vector3(0, bottomY, 0);

        // --- CHANGE ---
        // 1. Spawn the object AND get the instance
        GameObject doorInstance = spawn(doorData, doorPos);

        // 2. Get its bounds and store them in the generator
        if (doorInstance != null)
        {
            SpriteRenderer doorSR = doorInstance.GetComponent<SpriteRenderer>();
            if (doorSR != null)
            {
                // Tell the generator to remember the door's bounds
                generator.StorePlacedObjectBounds(DoorBoundsKey, doorSR.bounds);
            }
        }
    }
}