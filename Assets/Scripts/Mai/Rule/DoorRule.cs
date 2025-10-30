using UnityEngine;

[CreateAssetMenu(fileName = "DoorRule", menuName = "ProceduralHouse/Rule/Door Rule")]
public class DoorRule : ProceduralRule
{
    [Header("Door Settings")]
    public float doorVerticalOffset = 0.1f;

    [Header("Random X Position")]
    [Tooltip("Enable random X-axis positioning for the door.")]
    public bool enableRandomXPosition = false;

    [Tooltip("Minimum spacing from the left edge (in addition to the generator's leftMargin).")]
    public float minSpacingFromLeft = 0.5f;

    [Tooltip("Minimum spacing from the right edge (in addition to the generator's rightMargin).")]
    public float minSpacingFromRight = 0.5f;

    [Tooltip("Additional deduction margin applied to both sides, reducing the valid placement area.")]
    public float deductionMargin = 0.3f;

    public const string DoorBoundsKey = "MainDoor";

    public override void Execute(
    ProceduralHouseGenerator generator,
    Vector2 halfSize,
    System.Func<HouseObjectData, Vector3, GameObject> spawn
)
    {
        HouseObjectData doorData = generator.houseData.GetObject(objectType);
        if (doorData == null || doorData.objectPrefab == null) return;

        SpriteRenderer doorSR = doorData.objectPrefab.GetComponent<SpriteRenderer>();
        if (doorSR == null) return;

        // Get the prefab's extents and size. These are in local space.
        Vector3 doorExtents = doorSR.bounds.extents;
        Vector3 doorSize = doorSR.bounds.size;
        float doorHalfWidth = doorExtents.x;
        float doorPivotY = -halfSize.y + generator.bottomMargin + doorVerticalOffset;

        float doorPivotX = 0f;

        if (enableRandomXPosition)
        {
            float leftBoundary = -halfSize.x + generator.leftMargin + minSpacingFromLeft + deductionMargin + doorHalfWidth;
            float rightBoundary = halfSize.x - generator.rightMargin - minSpacingFromRight - deductionMargin - doorHalfWidth;

            if (leftBoundary < rightBoundary)
            {
                doorPivotX = Random.Range(leftBoundary, rightBoundary);
            }
            else
            {
                Debug.LogWarning($"Door placement area is too narrow. Defaulting to center. Left: {leftBoundary}, Right: {rightBoundary}");
                doorPivotX = 0f; // Default to center
            }
        }

        // This is the door's local-space pivot position
        Vector3 doorPivotPos = new Vector3(doorPivotX, doorPivotY, 0);

        // Spawn the door at its local pivot position
        GameObject doorInstance = spawn(doorData, doorPivotPos);

        if (doorInstance != null)
        {
            Vector3 localBoundsCenter = doorPivotPos + new Vector3(0, doorExtents.y, 0);
            Bounds localBounds = new Bounds(localBoundsCenter, doorSize);
            generator.StorePlacedObjectBounds(DoorBoundsKey, localBounds);
        }
    }
}
