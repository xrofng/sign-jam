using UnityEngine;

[CreateAssetMenu(fileName = "WindowRule", menuName = "ProceduralHouse/Rule/WindowRule")]
public class WindowRule : ProceduralRule
{
    [Header("Window Settings")]
    [Tooltip("Horizontal spacing from the center door for the windows.")]
    public float horizontalSpacing = 2f;

    // For simplicity, we'll hardcode the door height assumption for now.
    // In a more complex system, the DoorRule could output the door's position/height.
    public float windowVerticalOffsetFromBottom = 1.0f;

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Action<HouseObjectData, Vector3> spawn
    )
    {
        HouseObjectData windowData = generator.houseData.GetObject(objectType);
        if (windowData == null || windowData.objectPrefab == null)
        {
            Debug.LogWarning($"Window object data not found for rule: {name}");
            return;
        }

        // Calculate a sensible Y position for the windows
        float windowY = -halfSize.y + generator.bottomMargin + windowVerticalOffsetFromBottom;

        Vector3 leftPos = new Vector3(-horizontalSpacing, windowY, 0);
        Vector3 rightPos = new Vector3(horizontalSpacing, windowY, 0);

        // Bound checks using generator's margins
        if (leftPos.x >= -halfSize.x + generator.leftMargin)
            spawn(windowData, leftPos);

        if (rightPos.x <= halfSize.x - generator.rightMargin)
            spawn(windowData, rightPos);
    }
}