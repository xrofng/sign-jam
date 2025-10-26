using System;
using System.Linq;
using UnityEngine;

public static class EnumUtils
{
    /// <summary>
    /// Returns a random active flag from a [Flags] enum value.
    /// </summary>
    public static T GetRandomFlag<T>(T selectedFlags) where T : Enum
    {
        // Get all enum values
        var allValues = Enum.GetValues(typeof(T)).Cast<T>();

        // Filter only flags that are set in the selectedFlags
        var activeFlags = allValues.Where(v =>
        {
            int intValue = Convert.ToInt32(v);
            int selectedValue = Convert.ToInt32(selectedFlags);
            return intValue != 0 && (selectedValue & intValue) == intValue;
        }).ToList();

        // Return random one if exists
        if (activeFlags.Count == 0)
            return default;

        return activeFlags[UnityEngine.Random.Range(0, activeFlags.Count)];
    }
}
