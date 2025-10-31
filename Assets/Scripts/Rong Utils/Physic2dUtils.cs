using UnityEngine;

public static class Physics2DUtils
{
    /// <summary>
    /// Performs a BoxCast that matches the size and position of a given BoxCollider2D.
    /// </summary>
    /// <param name="collider">The BoxCollider2D to match.</param>
    /// <param name="direction">The direction to cast in.</param>
    /// <param name="distance">The cast distance.</param>
    /// <param name="layerMask">LayerMask to filter collisions.</param>
    /// <returns>RaycastHit2D of the first hit, or default if none.</returns>
    public static RaycastHit2D BoxCastEqualToCollider(BoxCollider2D collider, Vector2 direction, float distance, LayerMask layerMask)
    {
        if (collider == null)
        {
            Debug.LogWarning("[Physics2DUtils] BoxCollider2D is null.");
            return default;
        }

        // Use collider’s world position and rotation
        Vector2 origin = collider.bounds.center;
        Vector2 size = collider.bounds.size;
        float angle = collider.transform.eulerAngles.z;

        // Perform a box cast equal to the collider’s world-space size
        return Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask);
    }

    /// <summary>
    /// Performs an overlap check instead of a cast — useful if you just want to detect what's inside the collider area.
    /// </summary>
    public static Collider2D[] OverlapBoxEqualToCollider(BoxCollider2D collider, LayerMask layerMask)
    {
        if (collider == null)
        {
            Debug.LogWarning("[Physics2DUtils] BoxCollider2D is null.");
            return new Collider2D[0];
        }

        Vector2 center = collider.bounds.center;
        Vector2 size = collider.bounds.size;
        float angle = collider.transform.eulerAngles.z;

        return Physics2D.OverlapBoxAll(center, size, angle, layerMask);
    }
}
