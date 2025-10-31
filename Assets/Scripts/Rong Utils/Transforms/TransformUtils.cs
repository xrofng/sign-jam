using UnityEngine;

public static class TransformUtils
{
    /// <summary>
    /// Returns a random world-space position located inside the rectangular bounds of the SpriteRenderer.
    /// Works with rotation/scale because it samples in the sprite's local space then transforms to world.
    /// </summary>
    /// <param name="spriteRenderer">SpriteRenderer on the GameObject (required)</param>
    /// <returns>Random world-space Vector3 inside the sprite rectangle. If null, returns Vector3.zero.</returns>
    public static Vector3 RandomPointInsideSprite(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return Vector3.zero;

        // Sprite.bounds is in the sprite's local space (centered at sprite.bounds.center)
        Bounds localBounds = spriteRenderer.sprite.bounds;

        // pick a random point inside the local bounds (z = 0 relative to sprite plane)
        float x = Random.Range(localBounds.min.x, localBounds.max.x);
        float y = Random.Range(localBounds.min.y, localBounds.max.y);
        Vector3 localPoint = new Vector3(x, y, 0f);

        // convert local (sprite) space to world space using the sprite's Transform
        return spriteRenderer.transform.TransformPoint(localPoint);
    }

    /// <summary>
    /// Sets the position on a specific axis (X, Y, or Z) without affecting other axes.
    /// </summary>
    /// <param name="transform">Target transform to modify.</param>
    /// <param name="axis">Axis to change ("x", "y", or "z").</param>
    /// <param name="value">New position value for that axis.</param>
    public static void SetPositionAxis(Transform transform, char axis, float value)
    {
        Vector3 pos = transform.position;

        switch (char.ToLower(axis))
        {
            case 'x':
                pos.x = value;
                break;
            case 'y':
                pos.y = value;
                break;
            case 'z':
                pos.z = value;
                break;
            default:
                Debug.LogWarning($"[TransformUtils] Invalid axis '{axis}'. Use 'x', 'y', or 'z'.");
                return;
        }

        transform.position = pos;
    }

    public static void SetScaleAxis(Transform transform, char axis, float value)
    {
        if (transform == null)
            return;

        Vector3 scale = transform.localScale;

        switch (char.ToLower(axis))
        {
            case 'x':
                scale.x = value;
                break;
            case 'y':
                scale.y = value;
                break;
            case 'z':
                scale.z = value;
                break;
            default:
                Debug.LogWarning($"SetScaleAxis: Unknown axis '{axis}'. Use 'x', 'y', or 'z'.");
                return;
        }

        transform.localScale = scale;
    }
}
