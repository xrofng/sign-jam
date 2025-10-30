using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WindowRoofRule", menuName = "ProceduralHouse/Rule/Window Roof Rule")]
public class WindowRoofRule : ProceduralRule
{
    [Header("Window Roof Placement")]
    [Tooltip("Vertical offset from the top boundary to position the window roof. Similar to RoofRule positioning.")]
    public float verticalOffset = 0f;

    [Header("Window Count")]
    [Tooltip("Total number of window roofs to generate.")]
    [Range(0, 10)]
    public int windowCount = 1;

    [Header("Random Placement Boundaries")]
    [Tooltip("Margin from the left edge for random window placement.")]
    public float leftMargin = 0.5f;

    [Tooltip("Margin from the right edge for random window placement.")]
    public float rightMargin = 0.5f;

    [Header("Spacing")]
    [Tooltip("Minimum spacing between windows to prevent overlapping.")]
    public float minSpacingBetweenWindows = 0.3f;

    [Tooltip("Maximum attempts to find a non-overlapping position for each window.")]
    public int maxPlacementAttempts = 50;

    [Header("Window Scaling")]
    [Tooltip("Scale multiplier for window roof objects.")]
    public float windowScale = 1f;

    [Header("Debug")]
    [Tooltip("Enable debug logs for window roof placement calculations.")]
    public bool enableDebugLogs = false;

    public const string DoorBoundsKey = "MainDoor";

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        if (windowCount <= 0) return;

        HouseObjectData windowRoofData = generator.houseData.GetRandomObject(objectType);
        if (windowRoofData == null || windowRoofData.objectPrefab == null)
        {
            if (enableDebugLogs)
            {
                Debug.LogWarning("[WindowRoofRule] No window roof object found in HouseData.");
            }
            return;
        }

        SpriteRenderer windowSR = windowRoofData.objectPrefab.GetComponent<SpriteRenderer>();
        if (windowSR == null || windowSR.sprite == null)
        {
            Debug.LogError("[WindowRoofRule] Window roof prefab must have a SpriteRenderer with a Sprite assigned.");
            return;
        }

        float windowWidth = windowSR.bounds.size.x * windowScale;
        float windowHalfWidth = windowWidth * 0.5f;

        float topY = halfSize.y - generator.topMargin;
        float windowRoofY = topY + verticalOffset;

        Bounds? doorBounds = generator.GetPlacedObjectBounds(DoorBoundsKey);
        float doorCenterX = doorBounds.HasValue ? doorBounds.Value.center.x : 0f;

        List<float> placedXPositions = new List<float>();

        PlaceWindowRoof(generator, spawn, windowRoofData, doorCenterX, windowRoofY);
        placedXPositions.Add(doorCenterX);

        if (windowCount > 1)
        {
            float leftBoundary = -halfSize.x + leftMargin + windowHalfWidth;
            float rightBoundary = halfSize.x - rightMargin - windowHalfWidth;

            int placedCount = 1;

            for (int i = 1; i < windowCount; i++)
            {
                HouseObjectData additionalWindowData = generator.houseData.GetRandomObject(objectType);
                if (additionalWindowData == null) continue;

                float validX = FindNonOverlappingPosition(
                    leftBoundary,
                    rightBoundary,
                    placedXPositions,
                    windowHalfWidth,
                    minSpacingBetweenWindows
                );

                if (!float.IsNaN(validX))
                {
                    PlaceWindowRoof(generator, spawn, additionalWindowData, validX, windowRoofY);
                    placedXPositions.Add(validX);
                    placedCount++;
                }
                else
                {
                    if (enableDebugLogs)
                    {
                        Debug.LogWarning($"[WindowRoofRule] Could not find valid position for window {i + 1}. Skipping.");
                    }
                }
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[WindowRoofRule] Successfully placed {placedCount}/{windowCount} window roof(s) at Y: {windowRoofY:F2}");
            }
        }
        else if (enableDebugLogs)
        {
            Debug.Log($"[WindowRoofRule] Placed 1 window roof at Y: {windowRoofY:F2}");
        }
    }

    private float FindNonOverlappingPosition(
        float leftBoundary,
        float rightBoundary,
        List<float> placedXPositions,
        float windowHalfWidth,
        float minSpacing
    )
    {
        for (int attempt = 0; attempt < maxPlacementAttempts; attempt++)
        {
            float candidateX = Random.Range(leftBoundary, rightBoundary);

            bool isValid = true;
            float requiredDistance = windowHalfWidth * 2f + minSpacing;

            foreach (float placedX in placedXPositions)
            {
                if (Mathf.Abs(candidateX - placedX) < requiredDistance)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                return candidateX;
            }
        }

        return float.NaN;
    }

    private void PlaceWindowRoof(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        HouseObjectData windowRoofData,
        float xPosition,
        float yPosition
    )
    {
        Vector3 windowRoofPosition = new Vector3(xPosition, yPosition, 0);
        GameObject windowRoofObject = spawn(windowRoofData, windowRoofPosition);

        if (windowRoofObject != null && windowScale != 1f)
        {
            windowRoofObject.transform.localScale = Vector3.one * windowScale;
        }

        if (enableDebugLogs)
        {
            Debug.Log($"[WindowRoofRule] Placed window roof at X: {xPosition:F2}, Y: {yPosition:F2}");
        }
    }
}
