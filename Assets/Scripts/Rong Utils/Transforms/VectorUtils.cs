using UnityEngine;

/// <summary>
/// Provides static utility methods for working with Vector3.
/// </summary>
public static class VectorUtils
{
    /// <summary>
    /// Returns a new Vector3 with the specified y-coordinate,
    /// keeping the original x and z coordinates.
    /// </summary>
    /// <param name="originalVector">The Vector3 to modify.</param>
    /// <param name="newY">The new value for the y-coordinate.</param>
    /// <returns>A new Vector3 with the updated y-coordinate.</returns>
    public static Vector3 SetY(Vector3 originalVector, float newY)
    {
        // Vector3 is a struct, so modifying it creates a copy.
        // We create a new Vector3 using the original x and z, and the new y.
        return new Vector3(originalVector.x, newY, originalVector.z);
    }
}