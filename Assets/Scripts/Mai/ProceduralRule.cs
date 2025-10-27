using UnityEngine;

/// Abstract base class for all procedural generation rules.
public abstract class ProceduralRule : ScriptableObject
{
    [Tooltip("The type of object this rule attempts to place.")]
    public ObjectType objectType;

    /// Executes the placement logic for this rule.
    /// "generator" Reference to the main generator component.
    /// "half size" Half-extents of the house background sprite (scaled).
    /// "spawn" Action to spawn the part: (HouseObjectData, position).
    public abstract void Execute(
        ProceduralHouseGenerator generator,
        Vector2 halfSize,
        System.Func<HouseObjectData, Vector3, GameObject> spawn
    );
}