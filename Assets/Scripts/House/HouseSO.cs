using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "H_", menuName = "SignGame/House")]
public class HouseSO : ScriptableObject
{
    /// <summary>
    /// Represents different exterior areas of a house.
    /// Can be combined as flags (e.g., FrontYard | Roof).
    /// </summary>
    [System.Flags]
    public enum EArea
    {
        None = 0,
        FrontYard = 1 << 0, // 1
        Backyard = 1 << 1, // 2
        Roof = 1 << 2, // 4
        Fence = 1 << 3, // 8
        Wall = 1 << 4, // 16
        Door = 1 << 5,  // 32
    }

    public Data HouseData;


    [Tooltip("Ambient sound or background audio associated with this area.")]
    public AudioClip AmbientSound;


    [System.Serializable]
    public class Data
    {
        [Header("House Information")]
        [Tooltip("Name of the house or property.")]
        public string HouseName;

        public List<DecorationSO> Decorations;

        public Data(List<DecorationRequest> allDecorationRequests, string houseName)
        {
            HouseName = houseName;
            Decorations = new List<DecorationSO>();
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
