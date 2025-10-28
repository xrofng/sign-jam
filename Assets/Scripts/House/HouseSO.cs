using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that defines a house and its associated data,
/// including its name, decorations, and ambient sound.
/// </summary>
[CreateAssetMenu(fileName = "H_", menuName = "SignGame/House")]
public class HouseSO : ScriptableObject
{
    /// <summary>
    /// Represents different exterior areas of a house.
    /// These values are marked with [System.Flags] to allow
    /// combination using bitwise OR operations (e.g., FrontYard | Roof).
    /// </summary>
    [System.Flags]
    public enum EArea
    {
        None = 0,
        FrontYard = 1 << 0, // 1
        Backyard = 1 << 1,  // 2
        Roof = 1 << 2,      // 4
        Fence = 1 << 3,     // 8
        Wall = 1 << 4,      // 16
        Door = 1 << 5       // 32
    }

    /// <summary>
    /// Core data of this house, including name and decorations.
    /// </summary>
    public HouseSetting HouseData;

    /// <summary>
    /// Ambient sound or background audio associated with this house or area.
    /// </summary>
    [Tooltip("Ambient sound or background audio associated with this area.")]
    public AudioClip AmbientSound;

    /// <summary>
    /// Contains detailed information about a specific house instance.
    /// </summary>
    [System.Serializable]
    public class HouseSetting
    {
        [Header("House Information")]
        /// <summary>
        /// Name of the house or property.
        /// </summary>
        [Tooltip("Name of the house or property.")]
        public string HouseName;

        /// <summary>
        /// List of decorations placed or available for this house.
        /// </summary>
        [Tooltip("List of decorations should be on the scene.")]
        public List<DecorationSO> Decorations;

        /// <summary>
        /// Constructs a new house data object by generating a list of decorations
        /// based on a collection of decoration requests. Each request specifies the minimum number of items (MinQuantity)
        /// </summary>
        /// <param name="allDecorationRequests">All decoration requests to include in the house.</param>
        /// <param name="houseName">The name to assign to this house.</param>
        public HouseSetting(List<DecorationRequest> allDecorationRequests, string houseName)
        {
            HouseName = houseName;
            Decorations = new List<DecorationSO>();

            // Populate decorations based on the requested quantity
            foreach (DecorationRequest request in allDecorationRequests)
            {
                for (int i = 0; i < request.MinQuantity; i++)
                {
                    Decorations.Add(request.Decoration);
                }
            }
        }
    }
}
