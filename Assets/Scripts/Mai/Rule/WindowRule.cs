using UnityEngine;

[CreateAssetMenu(fileName = "WindowRule", menuName = "Procedural/Rule/Window Rule")]
public class WindowRule : ProceduralRule
{
    [Header("Window Settings")]
    [Tooltip("Horizontal spacing between the window's center and the door's edge (or house center for fallback).")]
    public float spacing = 0.5f;

    [Tooltip("Vertical offset from the bottom margin for window placement when door bounds are NOT available.")]
    public float verticalOffsetFromBottom = 1.0f;

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        HouseObjectData windowData = generator.houseData.GetObject(objectType);
        if (windowData == null || windowData.objectPrefab == null) return;

        float windowHalfWidth = windowData.objectPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

        float windowY;
        float leftWindowX;
        float rightWindowX;

        Bounds? doorBounds = generator.GetPlacedObjectBounds(DoorRule.DoorBoundsKey);

        if (doorBounds.HasValue)
        {
            // **Mode 1: Door-Dependent**
            Bounds door = doorBounds.Value;
            windowY = -halfSize.y + generator.bottomMargin + verticalOffsetFromBottom;

            // Positions relative to door edges
            leftWindowX = door.min.x - spacing - windowHalfWidth;
            rightWindowX = door.max.x + spacing + windowHalfWidth;
        }
        else
        {
            // **Mode 2: Fallback (No Door Found)**
            Debug.LogWarning("Window Rule fell back to bottom-margin placement as Door bounds were not found.");

            // Calculate Y position relative to the house bottom
            windowY = -halfSize.y + generator.bottomMargin + verticalOffsetFromBottom;

            // Positions relative to house center (0,0)
            leftWindowX = 0f - spacing - windowHalfWidth;
            rightWindowX = 0f + spacing + windowHalfWidth;
        }


        Vector3 leftPos = new Vector3(leftWindowX, windowY, 0);
        Vector3 rightPos = new Vector3(rightWindowX, windowY, 0);

        // Check bounds before spawning (Left Window)
        if (leftPos.x - windowHalfWidth >= -halfSize.x + generator.leftMargin)
            spawn(windowData, leftPos);

        // Check bounds before spawning (Right Window)
        if (rightPos.x + windowHalfWidth <= halfSize.x - generator.rightMargin)
            spawn(windowData, rightPos);

    }
}