using UnityEngine;

[CreateAssetMenu(fileName = "HouseObject", menuName = "ProceduralHouse/House Object Data", order = 0)]
public class HouseObjectData : ScriptableObject
{
    [Header("Main Properties")]
    public GameObject objectPrefab;

    [Tooltip("Type of this house object (door, window, etc.)")]
    public ObjectType objectType;

    [Tooltip("Sign type of this object (bad, good, default)")]
    public SignType signType;

    [Tooltip("Candy type associated with this object (if any)")]
    public CandyType candyType;

    [Range(-10, 10)]
    [Tooltip("Optional level that represents how strong or valuable this sign is.")]
    public int signLevel = 0;

    [Header("Interaction Settings")]
    public bool IsInteractable = false;

    [Tooltip("Defines the effect type when interacting with this object.")]
    public InteractionEffectType interactionEffectType;

    [Tooltip("Optional sound or visual effect to play when interacted with.")]
    public InteractionEffect interactionEffectAsset;
}

public enum ObjectType
{
    Door,
    Window,
    WideWindow,
    SideRoof,
    WideSideRoof,
    ShortSideRoof,
    ThinRoof,
    GarageDoor,
    GarageWindow,
    SideFence,
    Fence,
    Stair,
    Decoration
}

public enum SignType
{
    Default,
    Good,
    Bad
}

public enum CandyType
{
    None,
    NormalCandy,
    Gummy,
    Chocolate,
    PeanutButterCups
}

public enum InteractionEffectType
{
    None,
    Sound,
    Text
}


[System.Serializable]
public class InteractionEffect
{
    [Header("Effect Assets (Optional)")]
    public AudioClip soundEffect;
    [TextArea] public string interactionText;
    public GameObject visualEffectPrefab;
}
