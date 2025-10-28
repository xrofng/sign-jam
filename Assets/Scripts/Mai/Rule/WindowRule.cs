using UnityEngine;

[CreateAssetMenu(fileName = "WindowRule", menuName = "Procedural/Rule/Window Rule")]
public class WindowRule : ProceduralRule
{
    [Header("Window Placement")]
    [Tooltip("Minimum horizontal spacing maintained between the centers of two adjacent window units.")]
    public float spacing = 0.5f;

    [Tooltip("Vertical offset added to the window's center Y position. Aligns windows relative to the door's center (dependent mode) or the house bottom (fallback mode).")]
    public float verticalAlignmentOffset = 1.0f;

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        HouseObjectData windowData = generator.houseData.GetObject(objectType);
        if (windowData == null || windowData.objectPrefab == null) return;

        // Calculate core measurements for placement
        float windowHalfWidth = windowData.objectPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        float windowUnitWidth = (2f * windowHalfWidth) + spacing;

        float windowY;
        float initialRightX;
        float initialLeftX;

        Bounds? doorBounds = generator.GetPlacedObjectBounds(DoorRule.DoorBoundsKey);

        if (doorBounds.HasValue)
        {
            // Door-Dependent Mode
            Bounds door = doorBounds.Value;

            windowY = -halfSize.y + generator.bottomMargin + verticalAlignmentOffset;

            // Calculate initial positions relative to door edges
            initialRightX = door.max.x + (spacing / 2f) + windowHalfWidth;
            initialLeftX = door.min.x - (spacing / 2f) - windowHalfWidth;
        }
        else
        {
            // Fallback Mode
            Debug.LogWarning("Window Rule fell back to center placement as Door bounds were not found.");

            windowY = -halfSize.y + generator.bottomMargin + verticalAlignmentOffset;

            // Calculate initial positions symmetrically around the center (0,0)
            float offsetFromCenter = (spacing / 2f) + windowHalfWidth;

            initialRightX = offsetFromCenter;
            initialLeftX = -offsetFromCenter;
        }

        // --- Iterative Placement to the Right ---

        float currentXRight = initialRightX;
        float rightBoundary = halfSize.x - generator.rightMargin;

        while (currentXRight + windowHalfWidth <= rightBoundary)
        {
            spawn(windowData, new Vector3(currentXRight, windowY, 0));
            currentXRight += windowUnitWidth;
        }

        // --- Iterative Placement to the Left ---

        float currentXLeft = initialLeftX;
        float leftBoundary = -halfSize.x + generator.leftMargin;

        while (currentXLeft - windowHalfWidth >= leftBoundary)
        {
            spawn(windowData, new Vector3(currentXLeft, windowY, 0));
            currentXLeft -= windowUnitWidth;
        }
    }
}