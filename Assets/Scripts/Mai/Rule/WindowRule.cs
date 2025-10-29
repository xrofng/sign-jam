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

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        HouseObjectData windowData = generator.houseData.GetRandomObject(objectType);
        if (windowData == null || windowData.objectPrefab == null) return;

        float windowHalfWidth = windowData.objectPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        float windowFullWidth = windowHalfWidth * 2f;

        float windowY = -halfSize.y + generator.bottomMargin + verticalAlignmentOffset;

        float rightBoundary = halfSize.x - generator.rightMargin - rightDeductionMargin;
        float leftBoundary = -halfSize.x + generator.leftMargin + leftDeductionMargin;

        Bounds? doorBounds = generator.GetPlacedObjectBounds(DoorRule.DoorBoundsKey);

        if (doorBounds.HasValue)
        {
            Bounds door = doorBounds.Value;
            float rightStartX = door.max.x;
            float leftStartX = door.min.x;

            // Directly call Percentage mode logic
            PlaceSidePercentage(generator, spawn, windowY, windowFullWidth,
                                rightStartX, rightBoundary, true);
            PlaceSidePercentage(generator, spawn, windowY, windowFullWidth,
                                leftStartX, leftBoundary, false);
        }
        else
        {
            Debug.Log("Window Rule: No door found, distributing windows across entire width.");

            // Directly call Percentage mode logic for no-door case
            PlaceWindowsPercentageNoDoor(generator, spawn, windowY, windowFullWidth,
                                        leftBoundary, rightBoundary);
        }
    }

    private void PlaceWindowsPercentageNoDoor(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float windowY,
        float windowFullWidth,
        float leftBoundary,
        float rightBoundary
    )
    {
        float windowHalfWidth = windowFullWidth * 0.5f;
        float availableSpace = rightBoundary - leftBoundary;

        if (availableSpace <= windowFullWidth + minSpacing)
            return;

        int maxWindowCount = Mathf.FloorToInt((availableSpace - minSpacing) / (windowFullWidth + minSpacing));
        if (maxWindowCount <= 0)
            return;

        float totalWindowWidth = maxWindowCount * windowFullWidth;
        float leftoverSpace = availableSpace - totalWindowWidth;

        int gapCount = maxWindowCount + 1;
        float calculatedSpacing = (leftoverSpace * spacingPercentage) / gapCount;
        calculatedSpacing = Mathf.Max(calculatedSpacing, minSpacing);

        float totalSpacingNeeded = calculatedSpacing * gapCount;

        if (totalSpacingNeeded + totalWindowWidth > availableSpace)
        {
            maxWindowCount--;
            if (maxWindowCount <= 0)
                return;

            totalWindowWidth = maxWindowCount * windowFullWidth;
            leftoverSpace = availableSpace - totalWindowWidth;
            gapCount = maxWindowCount + 1;
            calculatedSpacing = (leftoverSpace * spacingPercentage) / gapCount;
            calculatedSpacing = Mathf.Max(calculatedSpacing, minSpacing);
        }

        for (int i = 0; i < maxWindowCount; i++)
        {
            HouseObjectData windowData = generator.houseData.GetRandomObject(objectType);
            if (windowData == null)
                continue;

            float windowX = leftBoundary + calculatedSpacing + windowHalfWidth + (i * (windowFullWidth + calculatedSpacing));

            spawn(windowData, new Vector3(windowX, windowY, 0));
        }
    }

    private void PlaceSidePercentage(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float windowY,
        float windowFullWidth,
        float startX,
        float boundary,
        bool isRightSide
    )
    {
        float windowHalfWidth = windowFullWidth * 0.5f;
        float availableSpace = isRightSide ? (boundary - startX) : (startX - boundary);

        if (availableSpace <= windowFullWidth + minSpacing)
            return;

        int maxWindowCount = Mathf.FloorToInt((availableSpace - minSpacing) / (windowFullWidth + minSpacing));
        if (maxWindowCount <= 0)
            return;

        float totalWindowWidth = maxWindowCount * windowFullWidth;
        float leftoverSpace = availableSpace - totalWindowWidth;

        int gapCount = maxWindowCount + 1;
        float calculatedSpacing = (leftoverSpace * spacingPercentage) / gapCount;
        calculatedSpacing = Mathf.Max(calculatedSpacing, minSpacing);

        float totalSpacingNeeded = calculatedSpacing * gapCount;

        if (totalSpacingNeeded + totalWindowWidth > availableSpace)
        {
            maxWindowCount--;
            if (maxWindowCount <= 0)
                return;

            totalWindowWidth = maxWindowCount * windowFullWidth;
            leftoverSpace = availableSpace - totalWindowWidth;
            gapCount = maxWindowCount + 1;
            calculatedSpacing = (leftoverSpace * spacingPercentage) / gapCount;
            calculatedSpacing = Mathf.Max(calculatedSpacing, minSpacing);
        }

        for (int i = 0; i < maxWindowCount; i++)
        {
            HouseObjectData windowData = generator.houseData.GetRandomObject(objectType);
            if (windowData == null)
                continue;

            float offset = calculatedSpacing + windowHalfWidth + (i * (windowFullWidth + calculatedSpacing));
            float windowX = isRightSide ? startX + offset : startX - offset;

            spawn(windowData, new Vector3(windowX, windowY, 0));
        }
    }
}