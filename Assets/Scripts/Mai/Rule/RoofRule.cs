using UnityEngine;

[CreateAssetMenu(fileName = "RoofRule", menuName = "ProceduralHouse/Rule/Roof Rule")]
public class RoofRule : ProceduralRule
{
    [Header("Roof Placement")]
    [Tooltip("Offset from the top boundary to position the roof. Positive values move the roof down, negative values move it up.")]
    public float verticalOffset = 0f;

    [Header("Horizontal Margins")]
    [Tooltip("Margin to reduce the roof width from the left edge (can be negative to extend beyond building).")]
    public float leftHorizontalMargin = -0.5f;

    [Tooltip("Margin to reduce the roof width from the right edge (can be negative to extend beyond building).")]
    public float rightHorizontalMargin = -0.5f;

    [Header("Roof Scaling")]
    [Tooltip("Height of the roof sprite after scaling.")]
    public float roofHeight = 1.5f;

    [Tooltip("If enabled, maintains the roof sprite's aspect ratio when scaling. Height setting will be ignored.")]
    public bool maintainAspectRatio = false;

    [Header("Alignment")]
    [Tooltip("Horizontal alignment of the roof. 0 = left-aligned, 0.5 = centered, 1 = right-aligned.")]
    [Range(0f, 1f)]
    public float horizontalAlignment = 0.5f;

    [Header("Debug")]
    [Tooltip("Enable debug logs for roof placement calculations.")]
    public bool enableDebugLogs = false;

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        HouseObjectData roofData = generator.houseData.GetRandomObject(objectType);
        if (roofData == null || roofData.objectPrefab == null)
        {
            if (enableDebugLogs)
            {
                Debug.LogWarning("[RoofRule] No roof object found in HouseData.");
            }
            return;
        }

        SpriteRenderer roofRenderer = roofData.objectPrefab.GetComponent<SpriteRenderer>();
        if (roofRenderer == null || roofRenderer.sprite == null)
        {
            Debug.LogError("[RoofRule] Roof prefab must have a SpriteRenderer with a Sprite assigned.");
            return;
        }

        float buildingWidth = halfSize.x * 2f;
        float buildingHeight = halfSize.y * 2f;

        float topY = halfSize.y - generator.topMargin;
        float roofCenterY = topY + verticalOffset;

        float leftBoundary = -halfSize.x + leftHorizontalMargin;
        float rightBoundary = halfSize.x - rightHorizontalMargin;
        float roofWidth = rightBoundary - leftBoundary;

        if (roofWidth <= 0f)
        {
            Debug.LogWarning($"[RoofRule] Roof width is too small or negative ({roofWidth:F2}). Adjust horizontal margins.");
            return;
        }

        float roofCenterX = Mathf.Lerp(leftBoundary, rightBoundary, horizontalAlignment);

        Vector3 roofPosition = new Vector3(roofCenterX, roofCenterY, 0);
        GameObject roofObject = spawn(roofData, roofPosition);

        if (roofObject != null)
        {
            ApplyScaling(roofObject, roofRenderer.sprite, roofWidth);
        }

        if (enableDebugLogs)
        {
            Debug.Log($"[RoofRule] Placed roof at Y: {roofCenterY:F2}, X: {roofCenterX:F2}, Width: {roofWidth:F2}");
        }
    }

    private void ApplyScaling(GameObject roofObject, Sprite sprite, float targetWidth)
    {
        SpriteRenderer spawnedRenderer = roofObject.GetComponent<SpriteRenderer>();
        if (spawnedRenderer == null || sprite == null)
        {
            return;
        }

        float currentWidth = sprite.bounds.size.x;
        float currentHeight = sprite.bounds.size.y;

        float scaleX = targetWidth / currentWidth;
        float scaleY;

        if (maintainAspectRatio)
        {
            scaleY = scaleX;
        }
        else
        {
            scaleY = roofHeight / currentHeight;
        }

        roofObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        if (enableDebugLogs)
        {
            Debug.Log($"[RoofRule] Applied scale - X: {scaleX:F2}, Y: {scaleY:F2}");
        }
    }
}
