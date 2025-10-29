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

        float doorHalfWidth = doorSR.bounds.extents.x;
        float bottomY = -halfSize.y + generator.bottomMargin + doorVerticalOffset;

        float doorX = 0f;

        if (enableRandomXPosition)
        {
            float leftBoundary = -halfSize.x + generator.leftMargin + minSpacingFromLeft + deductionMargin + doorHalfWidth;
            float rightBoundary = halfSize.x - generator.rightMargin - minSpacingFromRight - deductionMargin - doorHalfWidth;

            if (leftBoundary < rightBoundary)
            {
                doorX = Random.Range(leftBoundary, rightBoundary);
            }
            else
            {
                Debug.LogWarning($"Door placement area is too narrow. Defaulting to center. Left: {leftBoundary}, Right: {rightBoundary}");
                doorX = 0f;
            }
        }

        Vector3 doorPos = new Vector3(doorX, bottomY, 0);
        GameObject doorInstance = spawn(doorData, doorPos);

        if (doorInstance != null)
        {
            SpriteRenderer instanceSR = doorInstance.GetComponent<SpriteRenderer>();
            if (instanceSR != null)
            {
                generator.StorePlacedObjectBounds(DoorBoundsKey, instanceSR.bounds);
            }
        }
    }
}
