using UnityEngine;

[CreateAssetMenu(fileName = "HColor_", menuName = "SignGame/House Color")]
public class HouseColorSO : ScriptableObject
{
    public string PaletteId = "Blue";
    /// <summary>
    /// Represents color assignments for each part of a house.
    /// </summary>
    [System.Serializable]
    public class AreaColor
    {
        public string HousePartId;
        public Color Tint;
    }

    /// <summary>
    /// List of color settings for each house area.
    /// </summary>
    [SerializeField] private AreaColor[] areaColors;

    /// <summary>
    /// Retrieves the color assigned to the given house area.
    /// Returns white if the area is not defined.
    /// </summary>
    public Color GetColor(string id)
    {
        foreach (var entry in areaColors)
        {
            if (entry.HousePartId == id)
                return entry.Tint;
        }
        return Color.white;
    }

    /// <summary>
    /// Returns all defined area-color pairs.
    /// </summary>
    public AreaColor[] GetAllColors() => areaColors;
}
