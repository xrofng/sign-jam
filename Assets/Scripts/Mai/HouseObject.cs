using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewHouseData", menuName = "ProceduralHouse/House Data", order = 1)]
public class HouseData : ScriptableObject
{
    [Header("House Identity")]
    [Tooltip("A unique name or ID for this house type.")]
    public string houseName;

    [Tooltip("For Generating Wall Textures")]
    public List<Sprite> WallSprites;

    [Header("House Parts")]
    [Tooltip("All modular parts belonging to this house.")]
    public List<HouseObjectData> houseObjects = new List<HouseObjectData>();

    [Header("Decoration Pool")]
    [Tooltip("Extra decorative objects to place around the house.")]
    public List<HouseObjectData> decorations = new List<HouseObjectData>();

    /// Get all objects of a certain type (e.g., all doors or all windows)
    public List<HouseObjectData> GetObjectsByType(ObjectType type)
    {
        return houseObjects.FindAll(obj => obj.objectType == type);
    }

    /// Get a random object of a given type 
    public HouseObjectData GetRandomObject(ObjectType type)
    {
        var list = GetObjectsByType(type);
        if (list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }

    public HouseObjectData GetObject(ObjectType type)
    {
        var list = GetObjectsByType(type);
        if (list == null || list.Count == 0) return null;
        return list[0];
    }
}
