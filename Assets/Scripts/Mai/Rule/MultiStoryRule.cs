using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MultiStoryRule", menuName = "ProceduralHouse/Rule/Multi Story Rule")]
public class MultiStoryRule : ProceduralRule
{
    // ... (All your existing public variables remain unchanged) ...
    [Header("Story Conditions")]
    public float minHeightForMultiStory = 8.0f;
    public float storyHeight = 4.0f;
    [Header("Debug")]
    public bool enableDebugLogs = true;
    [Header("Story Roof Settings")]
    public float storyRoofHeight = 0.3f;
    public float storyRoofHorizontalMargin = 0.2f;
    [Header("Upper Story Margins")]
    public float upperStoryTopMargin = 0.3f;
    public float upperStoryBottomMargin = 0.1f;
    public float upperStoryLeftMargin = 0.3f;
    public float upperStoryRightMargin = 0.3f;
    [Header("Rules to Execute")]
    public List<ProceduralRule> upperStoryRules = new List<ProceduralRule>();

    // --- Main Execution ---

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        float totalHeight = halfSize.y * 2f;
        Log($"Total height: {totalHeight:F2} | Min: {minHeightForMultiStory:F2} | Story: {storyHeight:F2}");

        // Guard clause: Check if tall enough for *any* multi-story
        if (totalHeight < minHeightForMultiStory)
        {
            Log($"House too short for multi-story ({totalHeight:F2} < {minHeightForMultiStory:F2}). Skipping.");
            return;
        }

        int numStories = Mathf.FloorToInt(totalHeight / storyHeight);

        // Guard clause: Check if tall enough for at least 2 stories
        if (numStories < 2)
        {
            Log($"Not enough height for 2+ stories. Need at least {storyHeight * 2:F2}.");
            return;
        }

        Log($"Calculating {numStories} total stories. Generating {numStories - 1} upper story/stories.");

        // Loop starts at 1 (the first *upper* story)
        for (int i = 1; i < numStories; i++)
        {
            // Calculate the Y position for the *bottom* of the new story (i.e., the floor)
            float storyFloorY = -halfSize.y + (i * storyHeight);
            Log($"Generating story {i + 1} at Y: {storyFloorY:F2}");

            // 1. Spawn the floor/roof separator
            SpawnStoryRoof(generator, spawn, halfSize.x, storyFloorY);

            // 2. Execute rules for the story *above* that floor
            ExecuteUpperStoryRules(generator, spawn, halfSize.x, storyFloorY);
        }
    }

    // --- Helper Methods ---
    private void SpawnStoryRoof(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float houseHalfWidth,
        float floorY
    )
    {
        HouseObjectData roofData = generator.houseData.GetObject(ObjectType.StoryRoof);
        if (roofData == null || roofData.objectPrefab == null)
        {
            Log("Story roof object not found. Skipping story roof generation.");
            return;
        }

        GameObject roofPrefab = roofData.objectPrefab;
        SpriteRenderer prefabRenderer = roofPrefab.GetComponent<SpriteRenderer>();

        if (prefabRenderer == null || prefabRenderer.sprite == null)
        {
            Debug.LogError("[MultiStoryRule] Story roof prefab must have a SpriteRenderer with a Sprite.");
            return;
        }

        float targetRoofWidth = (houseHalfWidth * 2f) - (storyRoofHorizontalMargin * 2f);
        Vector3 roofPosition = new Vector3(0, floorY, 0); // Center X, at the floor Y
        GameObject roofObject = spawn(roofData, roofPosition);

        if (roofObject != null)
        {
            SpriteRenderer spawnedRenderer = roofObject.GetComponent<SpriteRenderer>();
            if (spawnedRenderer != null && spawnedRenderer.sprite != null)
            {
                // Use the prefab's sprite bounds for calculation
                float spriteWidth = prefabRenderer.sprite.bounds.size.x;
                float spriteHeight = prefabRenderer.sprite.bounds.size.y;

                if (spriteWidth == 0 || spriteHeight == 0) return; // Avoid divide by zero

                float scaleX = targetRoofWidth / spriteWidth;
                float scaleY = storyRoofHeight / spriteHeight;

                roofObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            }
        }
    }

    private void ExecuteUpperStoryRules(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float houseHalfWidth,
        float storyFloorY
    )
    {
        //Define the "Virtual" Story Space
        float storyBottom = storyFloorY + storyRoofHeight; // Bottom is *above* the roof
        float storyTop = storyFloorY + this.storyHeight;
        float storyHeight = storyTop - storyBottom;

        Vector2 upperStoryHalfSize = new Vector2(houseHalfWidth, storyHeight * 0.5f);
        float upperStoryCenterY = storyBottom + (storyHeight * 0.5f);
        Vector3 spawnOffset = new Vector3(0, upperStoryCenterY, 0);
        System.Func<HouseObjectData, Vector3, GameObject> upperStorySpawn =
            (data, localPos) => spawn(data, localPos + spawnOffset);

        // Store the generator's current state
        float originalTop = generator.topMargin;
        float originalBottom = generator.bottomMargin;
        float originalLeft = generator.leftMargin;
        float originalRight = generator.rightMargin;
        Bounds? savedDoorBounds = generator.GetPlacedObjectBounds(DoorRule.DoorBoundsKey);

        try
        {
            // Modify the state for the upper story rules
            generator.topMargin = upperStoryTopMargin;
            generator.bottomMargin = upperStoryBottomMargin;
            generator.leftMargin = upperStoryLeftMargin;
            generator.rightMargin = upperStoryRightMargin;

            // Hide the main door from the upper story rules
            if (savedDoorBounds.HasValue)
            {
                generator.RemovePlacedObjectBounds(DoorRule.DoorBoundsKey);
            }

            foreach (var rule in upperStoryRules)
            {
                if (rule != null)
                {
                    rule.Execute(generator, upperStoryHalfSize, upperStorySpawn);
                }
            }
        }
        finally
        {
            // This 'finally' block guarantees the generator is restored,
            generator.topMargin = originalTop;
            generator.bottomMargin = originalBottom;
            generator.leftMargin = originalLeft;
            generator.rightMargin = originalRight;
            if (savedDoorBounds.HasValue)
            {
                generator.StorePlacedObjectBounds(DoorRule.DoorBoundsKey, savedDoorBounds.Value);
            }
        }
    }
    private void Log(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[MultiStoryRule] {message}");
        }
    }
}