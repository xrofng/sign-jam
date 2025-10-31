using UnityEngine;

public readonly struct TimeBreakdown
{
    public int Minute { get; }
    public int Second { get; }
    public int Centisecond { get; }

    public TimeBreakdown(float totalTime)
    {
        Minute = Mathf.FloorToInt(totalTime / 60f);
        Second = Mathf.FloorToInt(totalTime % 60f);
        Centisecond = Mathf.FloorToInt((totalTime * 100f) % 100f);
    }

    public TimeBreakdown(float minute, float second)
    {
        float totalTime = minute * 60f + second;
        Minute = Mathf.FloorToInt(totalTime / 60f);
        Second = Mathf.FloorToInt(totalTime % 60f);
        Centisecond = 0; // Default to 0 since input doesn't include fractions
    }

    public override string ToString()
    {
        return $"{Minute:00} : {Second:00}";
    }
}
