using UnityEngine;

[CreateAssetMenu(fileName = "WindowRule", menuName = "ProceduralHouse/Rule/Window Rule")]
public class WindowRule : ProceduralRule
{
    [Header("Window Placement")]
    [Tooltip("Vertical offset added to the window's center Y position. Aligns windows relative to the door's center (dependent mode) or the house bottom (fallback mode).")]
    public float verticalAlignmentOffset = 1.0f;

    [Header("Spacing")]
    [Tooltip("Percentage of leftover space to use as spacing (0-1). Distributes space between door, windows, and edges.")]
    [Range(0f, 1f)]
    public float spacingPercentage = 0.5f;

    [Tooltip("Minimum spacing to maintain between windows and from the door.")]
    public float minSpacing = 0.2f;

    [Header("Edge Margins")]
    [Tooltip("Additional deduction margin from the left edge (beyond generator's leftMargin). Prevents windows from generating too close to the left edge.")]
    public float leftDeductionMargin = 0.3f;

    [Tooltip("Additional deduction margin from the right edge (beyond generator's rightMargin). Prevents windows from generating too close to the right edge.")]
    public float rightDeductionMargin = 0.3f;

    public const string DoorBoundsKey = "MainDoor"; // Re-use the constant from DoorRule for clarity

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        // 1. Get ONE window prefab and measure its size ONCE.
        HouseObjectData windowData = generator.houseData.GetRandomObject(objectType);
        if (windowData == null || windowData.objectPrefab == null) return;

        SpriteRenderer windowSR = windowData.objectPrefab.GetComponent<SpriteRenderer>();
        if (windowSR == null) return;

        float windowFullWidth = windowSR.bounds.size.x;
        float windowY = -halfSize.y + generator.bottomMargin + verticalAlignmentOffset;
        float rightBoundary = halfSize.x - generator.rightMargin - rightDeductionMargin;
        float leftBoundary = -halfSize.x + generator.leftMargin + leftDeductionMargin;

        Bounds? doorBounds = generator.GetPlacedObjectBounds(DoorBoundsKey);

        if (doorBounds.HasValue)
        {
            Bounds door = doorBounds.Value;
            PlaceWindowsInSpace(generator, spawn, windowY, windowFullWidth, door.max.x, rightBoundary);
            PlaceWindowsInSpace(generator, spawn, windowY, windowFullWidth, leftBoundary, door.min.x);
        }
        else
        {
            Debug.Log("Window Rule: No door found, distributing windows across entire width.");

            // NO DOOR: Start at left boundary, end at right boundary
            PlaceWindowsInSpace(generator, spawn, windowY, windowFullWidth,
                                leftBoundary, rightBoundary);
        }
    }

    private void PlaceWindowsInSpace(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float windowY,
        float windowFullWidth,
        float startX,
        float endX
    )
    {
        // Ensure startX is less than endX for consistent calculations
        if (startX > endX)
        {
            float temp = startX;
            startX = endX;
            endX = temp;
        }

        float windowHalfWidth = windowFullWidth * 0.5f;
        float availableSpace = endX - startX;

        // Exit if there isn't enough room for one window + min spacing
        if (availableSpace < windowFullWidth + minSpacing)
            return;

        // Calculate maximum count and necessary spacing
        int maxWindowCount = Mathf.FloorToInt((availableSpace - minSpacing) / (windowFullWidth + minSpacing));
        if (maxWindowCount <= 0)
            return;

        float totalWindowWidth = maxWindowCount * windowFullWidth;
        float leftoverSpace = availableSpace - totalWindowWidth;
        int gapCount = maxWindowCount + 1; 

        // Calculate spacing, prioritizing spacingPercentage but enforcing minSpacing
        float calculatedSpacing = (leftoverSpace * spacingPercentage) / gapCount;
        calculatedSpacing = Mathf.Max(calculatedSpacing, minSpacing);

        // Loop and spawn
        for (int i = 0; i < maxWindowCount; i++)
        {

            //random window for EACH iteration 
            HouseObjectData windowData = generator.houseData.GetRandomObject(objectType);
            if (windowData == null) continue;
            float windowX = startX
                          + calculatedSpacing
                          + windowHalfWidth
                          + (i * (windowFullWidth + calculatedSpacing));

            spawn(windowData, new Vector3(windowX, windowY, 0));
        }
    }
}