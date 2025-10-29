using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MultiStoryRule", menuName = "ProceduralHouse/Rule/Multi Story Rule")]
public class MultiStoryRule : ProceduralRule
{
    [Header("Story Conditions")]
    [Tooltip("Minimum height required for the house to generate multiple stories.")]
    public float minHeightForMultiStory = 8.0f;

    [Tooltip("Height of each story (floor).")]
    public float storyHeight = 4.0f;

    [Header("Story Roof Settings")]
    [Tooltip("Height of the story roof that separates floors.")]
    public float storyRoofHeight = 0.3f;

    [Tooltip("Horizontal margin for the story roof (reduces width on both sides).")]
    public float storyRoofHorizontalMargin = 0.2f;

    [Header("Upper Story Margins")]
    [Tooltip("Top margin for the upper story.")]
    public float upperStoryTopMargin = 0.3f;

    [Tooltip("Bottom margin for the upper story (above the story roof).")]
    public float upperStoryBottomMargin = 0.1f;

    [Tooltip("Left margin for the upper story.")]
    public float upperStoryLeftMargin = 0.3f;

    [Tooltip("Right margin for the upper story.")]
    public float upperStoryRightMargin = 0.3f;

    [Header("Rules to Execute")]
    [Tooltip("Rules to execute for the upper story. Door rule should be excluded.")]
    public List<ProceduralRule> upperStoryRules = new List<ProceduralRule>();

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        float totalHeight = halfSize.y * 2f;

        if (totalHeight < minHeightForMultiStory)
        {
            return;
        }

        int numStories = Mathf.FloorToInt(totalHeight / storyHeight);
        if (numStories < 2)
        {
            return;
        }

        float fixedFirstStoryWidth = halfSize.x * 2f;
        float fixedFirstStoryHalfWidth = fixedFirstStoryWidth * 0.5f;

        for (int i = 1; i < numStories; i++)
        {
            float storyRoofY = -halfSize.y + (i * storyHeight);

            GenerateStoryRoof(generator, spawn, fixedFirstStoryHalfWidth, storyRoofY);

            GenerateUpperStory(generator, spawn, fixedFirstStoryHalfWidth, storyRoofY, i);
        }
    }

    private void GenerateStoryRoof(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float fixedHalfWidth,
        float storyRoofY
    )
    {
        HouseObjectData storyRoofData = generator.houseData.GetObject(ObjectType.StoryRoof);
        if (storyRoofData == null || storyRoofData.objectPrefab == null)
        {
            Debug.LogWarning("Story roof object not found. Skipping story roof generation.");
            return;
        }

        GameObject roofPrefab = storyRoofData.objectPrefab;
        SpriteRenderer roofRenderer = roofPrefab.GetComponent<SpriteRenderer>();

        if (roofRenderer == null || roofRenderer.sprite == null)
        {
            Debug.LogError("Story roof prefab must have a SpriteRenderer with a Sprite assigned.");
            return;
        }

        float targetRoofWidth = (fixedHalfWidth * 2f) - (storyRoofHorizontalMargin * 2f);
        float roofCenterX = 0f;
        float roofCenterY = storyRoofY;

        Vector3 roofPosition = new Vector3(roofCenterX, roofCenterY, 0);
        GameObject roofObject = spawn(storyRoofData, roofPosition);

        if (roofObject != null)
        {
            SpriteRenderer spawnedRenderer = roofObject.GetComponent<SpriteRenderer>();
            if (spawnedRenderer != null && spawnedRenderer.sprite != null)
            {
                float currentWidth = spawnedRenderer.sprite.bounds.size.x;
                float currentHeight = spawnedRenderer.sprite.bounds.size.y;

                float scaleX = targetRoofWidth / currentWidth;
                float scaleY = storyRoofHeight / currentHeight;

                roofObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            }
        }
    }

    private void GenerateUpperStory(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        float fixedHalfWidth,
        float storyRoofY,
        int storyIndex
    )
    {
        float upperStoryBottom = storyRoofY + storyRoofHeight;
        float upperStoryTop = storyRoofY + storyHeight;
        float upperStoryHeight = upperStoryTop - upperStoryBottom;
        float upperStoryHalfHeight = upperStoryHeight * 0.5f;

        Vector2 upperStoryHalfSize = new Vector2(fixedHalfWidth, upperStoryHalfHeight);

        float upperStoryCenterY = upperStoryBottom + upperStoryHalfHeight;

        UpperStorySpawner upperStorySpawner = new UpperStorySpawner(spawn, new Vector3(0, upperStoryCenterY, 0));

        float originalTopMargin = generator.topMargin;
        float originalBottomMargin = generator.bottomMargin;
        float originalLeftMargin = generator.leftMargin;
        float originalRightMargin = generator.rightMargin;

        generator.topMargin = upperStoryTopMargin;
        generator.bottomMargin = upperStoryBottomMargin;
        generator.leftMargin = upperStoryLeftMargin;
        generator.rightMargin = upperStoryRightMargin;

        foreach (var rule in upperStoryRules)
        {
            if (rule != null)
            {
                rule.Execute(generator, upperStoryHalfSize, upperStorySpawner.Spawn);
            }
        }

        generator.topMargin = originalTopMargin;
        generator.bottomMargin = originalBottomMargin;
        generator.leftMargin = originalLeftMargin;
        generator.rightMargin = originalRightMargin;
    }

    private class UpperStorySpawner
    {
        private System.Func<HouseObjectData, Vector3, GameObject> originalSpawn;
        private Vector3 offset;

        public UpperStorySpawner(System.Func<HouseObjectData, Vector3, GameObject> spawn, Vector3 offset)
        {
            this.originalSpawn = spawn;
            this.offset = offset;
        }

        public GameObject Spawn(HouseObjectData data, Vector3 position)
        {
            return originalSpawn(data, position + offset);
        }
    }
}
