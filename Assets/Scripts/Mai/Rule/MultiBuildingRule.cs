using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MultiBuildingRule", menuName = "ProceduralHouse/Rule/Multi Building Rule")]
public class MultiBuildingRule : ProceduralRule
{
    [Header("Multi-Building Settings")]
    [Tooltip("Enable generation of an additional building when width exceeds limit.")]
    public bool enableMultipleBuildings = true;

    [Header("Stretch Limit")]
    [Tooltip("Maximum stretch percentage allowed before generating a second building (e.g., 0.2 = 20%).")]
    [Range(0f, 1f)]
    public float maxStretchLimit = 0.2f;

    [Tooltip("Base width for the main building.")]
    public float baseBuildingWidth = 5f;

    [Tooltip("Base height for the main building. If 0, uses the original height from halfSize.")]
    public float baseBuildingHeight = 0f;

    [Header("Additional Building Settings")]
    [Tooltip("Height of the additional building. If 0, uses the same height as the main building.")]
    public float additionalBuildingHeight = 0f;

    [Tooltip("Horizontal gap between the main and additional building.")]
    public float buildingGap = 0.5f;

    [Tooltip("Position of the additional building relative to the main building.")]
    public BuildingPosition additionalBuildingPosition = BuildingPosition.Right;

    [Tooltip("Sorting order offset to apply to the additional building's sprites (e.g., -10).")]
    public int sortingOrderOffset = -10;

    [Header("Rules to Execute Per Building")]
    [Tooltip("List of procedural rules to execute for the main building.")]
    public List<ProceduralRule> mainBuildingRules = new List<ProceduralRule>();

    [Tooltip("List of procedural rules to execute for the additional building.")]
    public List<ProceduralRule> additionalBuildingRules = new List<ProceduralRule>();

    [Header("Debug")]
    public bool enableDebugLogs = true;

    public enum BuildingPosition
    {
        Left,
        Right,
        Random
    }

    public override void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    )
    {
        if (!enableMultipleBuildings)
        {
            Log("Multi-building generation is disabled. Skipping.");
            return;
        }

        float totalWidth = halfSize.x * 2f;
        float maxAllowedWidth = baseBuildingWidth * (1f + maxStretchLimit);

        Log($"Total width: {totalWidth:F2} | Base building width: {baseBuildingWidth:F2} | Max allowed width: {maxAllowedWidth:F2}");

        if (totalWidth <= maxAllowedWidth)
        {
            Log($"Width within stretch limit. No need for additional building.");
            return;
        }

        Log($"Width exceeds limit. Generating main building + additional building.");

        BuildingPosition actualPosition = additionalBuildingPosition;
        if (additionalBuildingPosition == BuildingPosition.Random)
        {
            actualPosition = Random.value > 0.5f ? BuildingPosition.Right : BuildingPosition.Left;
        }

        float totalBuildingsWidth = totalWidth;
        float startX = -totalBuildingsWidth * 0.5f;

        float mainBuildingActualWidth = baseBuildingWidth;
        float additionalBuildingActualWidth = totalWidth - mainBuildingActualWidth - buildingGap;

        if (additionalBuildingActualWidth <= 0)
        {
            Log($"Warning: Additional building has no width ({additionalBuildingActualWidth:F2}). TotalWidth ({totalWidth:F2}) may be too small for baseBuildingWidth ({mainBuildingActualWidth:F2}) + gap ({buildingGap:F2}). Skipping split.");
            return;
        }

        // Calculate main building height
        float mainBuildingActualHeight = baseBuildingHeight > 0f ? baseBuildingHeight : (halfSize.y * 2f);
        Vector2 mainBuildingHalfSize = new Vector2(mainBuildingActualWidth * 0.5f, mainBuildingActualHeight * 0.5f);

        // Calculate additional building height
        float actualAdditionalHeight = additionalBuildingHeight > 0f ? additionalBuildingHeight : mainBuildingActualHeight;
        Vector2 additionalBuildingHalfSize = new Vector2(additionalBuildingActualWidth * 0.5f, actualAdditionalHeight * 0.5f);

        // Calculate Y offset to align bottom margins
        // Original building bottom is at: -halfSize.y
        // Main building bottom is at: -mainBuildingHalfSize.y
        // Additional building bottom is at: -additionalBuildingHalfSize.y
        float originalBottomY = -halfSize.y;
        float mainBuildingYOffset = originalBottomY + mainBuildingHalfSize.y;
        float additionalBuildingYOffset = originalBottomY + additionalBuildingHalfSize.y;

        float mainBuildingCenterX;
        float additionalBuildingCenterX;

        if (actualPosition == BuildingPosition.Left)
        {
            additionalBuildingCenterX = startX + additionalBuildingHalfSize.x;
            mainBuildingCenterX = additionalBuildingCenterX + additionalBuildingHalfSize.x + buildingGap + mainBuildingHalfSize.x;

            Log($"Additional building (Left side) at X: {additionalBuildingCenterX:F2}, Y: {additionalBuildingYOffset:F2} (width: {additionalBuildingActualWidth:F2}, height: {actualAdditionalHeight:F2})");
            Log($"Main building (Right side) at X: {mainBuildingCenterX:F2}, Y: {mainBuildingYOffset:F2} (width: {mainBuildingActualWidth:F2}, height: {mainBuildingActualHeight:F2})");

            GenerateBuilding(generator, spawn, additionalBuildingRules, additionalBuildingHalfSize, additionalBuildingCenterX, additionalBuildingYOffset, "AdditionalBuilding", true);
            GenerateBuilding(generator, spawn, mainBuildingRules, mainBuildingHalfSize, mainBuildingCenterX, mainBuildingYOffset, "MainBuilding", false);
        }
        else
        {
            mainBuildingCenterX = startX + mainBuildingHalfSize.x;
            additionalBuildingCenterX = mainBuildingCenterX + mainBuildingHalfSize.x + buildingGap + additionalBuildingHalfSize.x;

            Log($"Main building (Left side) at X: {mainBuildingCenterX:F2}, Y: {mainBuildingYOffset:F2} (width: {mainBuildingActualWidth:F2}, height: {mainBuildingActualHeight:F2})");
            Log($"Additional building (Right side) at X: {additionalBuildingCenterX:F2}, Y: {additionalBuildingYOffset:F2} (width: {additionalBuildingActualWidth:F2}, height: {actualAdditionalHeight:F2})");

            GenerateBuilding(generator, spawn, mainBuildingRules, mainBuildingHalfSize, mainBuildingCenterX, mainBuildingYOffset, "MainBuilding", false);
            GenerateBuilding(generator, spawn, additionalBuildingRules, additionalBuildingHalfSize, additionalBuildingCenterX, additionalBuildingYOffset, "AdditionalBuilding", true);
        }
    }

    private void GenerateBuilding(
        ProceduralHouseGenerator generator,
        System.Func<HouseObjectData, Vector3, GameObject> spawn,
        List<ProceduralRule> rules,
        Vector2 buildingHalfSize,
        float buildingCenterX,
        float buildingCenterY,
        string buildingName,
        bool isAdditionalBuilding
    )
    {
        Dictionary<string, Bounds> savedBounds = new Dictionary<string, Bounds>();
        Bounds? mainDoorBounds = generator.GetPlacedObjectBounds(DoorRule.DoorBoundsKey);
        if (mainDoorBounds.HasValue)
        {
            savedBounds[DoorRule.DoorBoundsKey] = mainDoorBounds.Value;
            generator.RemovePlacedObjectBounds(DoorRule.DoorBoundsKey);
        }

        GameObject buildingParent = null;
        List<GameObject> spawnedObjects = new List<GameObject>();

        try
        {
            Vector3 buildingOffset = new Vector3(buildingCenterX, buildingCenterY, 0);

            System.Func<HouseObjectData, Vector3, GameObject> buildingSpawn = (data, localPos) =>
            {
                GameObject obj = spawn(data, localPos + buildingOffset);
                if (obj != null)
                {
                    spawnedObjects.Add(obj);
                }
                return obj;
            };

            Log($"Executing {rules.Count} rules for {buildingName} (HalfSize: {buildingHalfSize}, Offset: {buildingOffset})");

            foreach (var rule in rules)
            {
                if (rule != null)
                {
                    rule.Execute(generator, buildingHalfSize, buildingSpawn);
                }
            }

            if (spawnedObjects.Count > 0 && spawnedObjects[0] != null)
            {
                Transform parentTransform = spawnedObjects[0].transform.parent;

                buildingParent = new GameObject(buildingName);
                buildingParent.transform.SetParent(parentTransform);
                buildingParent.transform.localPosition = Vector3.zero;
                buildingParent.transform.localRotation = Quaternion.identity;
                buildingParent.transform.localScale = Vector3.one;

                foreach (var obj in spawnedObjects)
                {
                    if (obj != null)
                    {
                        obj.transform.SetParent(buildingParent.transform);
                    }
                }

                if (isAdditionalBuilding)
                {
                    AdjustSortingOrder(buildingParent, sortingOrderOffset);
                }
            }
        }
        finally
        {
            generator.RemovePlacedObjectBounds(DoorRule.DoorBoundsKey);

            foreach (var kvp in savedBounds)
            {
                generator.StorePlacedObjectBounds(kvp.Key, kvp.Value);
            }
        }
    }

    private void AdjustSortingOrder(GameObject parent, int orderOffset)
    {
        SpriteRenderer[] spriteRenderers = parent.GetComponentsInChildren<SpriteRenderer>(true);

        int adjustedCount = 0;
        foreach (var sr in spriteRenderers)
        {
            if (sr != null)
            {
                sr.sortingOrder += orderOffset;
                adjustedCount++;
            }
        }

        Log($"Adjusted {adjustedCount} sprite renderer(s) sorting order by {orderOffset} in '{parent.name}'");
    }

    private void Log(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[MultiBuildingRule] {message}");
        }
    }
}