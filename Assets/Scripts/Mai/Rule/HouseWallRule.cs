using UnityEngine;

[CreateAssetMenu(fileName = "HouseWallRule", menuName = "ProceduralHouse/Rule/Wall Rule")]
public class HouseWallRule : ProceduralRule
{
    [Header("Wall Sizing")]
    [Tooltip("Amount to deduct from the top of the house height (e.g., to account for the roof overlap or top margin).")]
    public float roofDeductionHeight = 1.0f;

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        // 1. Get the single wall object data (we assume this rule uses a non-randomized wall asset)
        HouseObjectData wallData = generator.houseData.GetObject(objectType);
        if (wallData == null || wallData.objectPrefab == null)
        {
            Debug.LogError($"Wall data not found for object type: {objectType}. Cannot generate wall.");
            return;
        }

        GameObject wallPrefab = wallData.objectPrefab;
        SpriteRenderer wallRenderer = wallPrefab.GetComponent<SpriteRenderer>();

        if (wallRenderer == null || wallRenderer.sprite == null)
        {
            Debug.LogError("Wall prefab must have a SpriteRenderer with a Sprite assigned.");
            return;
        }

        // --- 2. Calculate Target World Dimensions (based on house size and margins) ---

        // The total house width is 2 * halfSize.x
        float totalWidth = 2f * halfSize.x;
        float targetWallWidth = totalWidth - generator.leftMargin - generator.rightMargin;

        // The total house height is 2 * halfSize.y
        float totalHeight = 2f * halfSize.y;
        float targetWallHeight = totalHeight - generator.bottomMargin - roofDeductionHeight;

        // --- 3. Calculate Target Position (Center of the wall area) ---

        // X Position: Center of the total width, accounting for uneven left/right margins
        float targetX = (generator.rightMargin - generator.leftMargin) / 2f;

        // Y Position: Bottom of the house (-halfSize.y) + bottom margin + half the wall height
        float targetY = -halfSize.y + generator.bottomMargin + (targetWallHeight / 2f);

        Vector3 targetPosition = new Vector3(targetX, targetY, 0);

        // --- 4. Spawn and Scale the Wall Object ---

        // Spawn the wall at the calculated center position
        GameObject wallObject = spawn(wallData, targetPosition);

        if (wallObject != null)
        {
            SpriteRenderer spawnedRenderer = wallObject.GetComponent<SpriteRenderer>();
            if (spawnedRenderer != null && spawnedRenderer.sprite != null)
            {
                // Calculate the required scale factor (Target World Size / Current Sprite Size in World Units)
                float currentWidth = spawnedRenderer.sprite.bounds.size.x;
                float currentHeight = spawnedRenderer.sprite.bounds.size.y;

                float scaleX = targetWallWidth / currentWidth;
                float scaleY = targetWallHeight / currentHeight;

                // Apply the calculated scale to the object's transform
                wallObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            }
        }
    }
}
