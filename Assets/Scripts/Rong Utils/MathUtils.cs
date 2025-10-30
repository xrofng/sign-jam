using UnityEngine;

/// <summary>
/// Collection of handy mathematical utility functions for Unity.
/// Includes curve evaluation, remapping, and more.
/// </summary>
public static class MathUtils
{
    /// <summary>
    /// Returns the value of a normalized bell curve (Gaussian) at position x.
    /// The result peaks at 1 when x == mean.
    /// </summary>
    /// <param name="x">Input value.</param>
    /// <param name="mean">The center of the curve (peak position).</param>
    /// <param name="stdDev">The standard deviation controlling the curve’s width.</param>
    public static float BellCurve01(float x, float mean = 0.5f, float stdDev = 0.2f)
    {
        // Gaussian formula (no normalization constant)
        return Mathf.Exp(-Mathf.Pow(x - mean, 2f) / (2f * stdDev * stdDev));
    }

    /// <summary>
    /// Returns the value of a full Gaussian distribution with proper normalization.
    /// The total area under the curve equals 1.
    /// </summary>
    /// <param name="x">Input value.</param>
    /// <param name="mean">The mean (center of peak).</param>
    /// <param name="stdDev">Standard deviation (width of the bell).</param>
    public static float Gaussian(float x, float mean, float stdDev)
    {
        float a = 1f / (stdDev * Mathf.Sqrt(2f * Mathf.PI));
        float exponent = -Mathf.Pow(x - mean, 2f) / (2f * stdDev * stdDev);
        return a * Mathf.Exp(exponent);
    }

    /// <summary>
    /// Remaps a value from one range to another.
    /// </summary>
    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float t = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, t);
    }

    /// <summary>
    /// Clamps a value between 0 and 1 (same as Mathf.Clamp01 but included for consistency).
    /// </summary>
    public static float Clamp01(float value)
    {
        return Mathf.Clamp01(value);
    }

    /// <summary>
    /// Returns a smoothed value using a bell curve profile between 0 and 1.
    /// Handy for soft falloffs.
    /// </summary>
    public static float BellFalloff(float t)
    {
        // Center at 0.5, narrow curve for soft peak
        return BellCurve01(t, 0.5f, 0.25f);
    }
}
