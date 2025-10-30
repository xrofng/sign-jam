using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Dec_", menuName = "SignGame/Decoration")]
public class DecorationSO : ScriptableObject
{
    /// <summary>
    /// Unique identifier for this decoration.
    /// Can be used to distinguish between multiple decorations of the same type.
    /// </summary>
    [Header("Identification")]
    [Tooltip("Unique ID for this decoration.")]
    public string DecorationID;

    /// <summary>
    /// The exterior area of the house where this decoration is meant to be placed.
    /// </summary>
    [Header("Association")]
    [Tooltip("The exterior area this decoration belongs to.")]
    public HouseSO.EArea AssociatedArea;

    public Decoration BasePrefab;

    [PreviewField]
    public Sprite[] Textures;

    public string[] InspectionTexts;

    public Sprite GetRandomTexture()
    {
        return Textures[Random.Range(0, Textures.Length)];
    }

    public bool HasInpectionText()
    {
        return InspectionTexts != null && InspectionTexts.Length > 0;
    }

    public string RandomInspectionText()
    {
        Debug.Log(DecorationID);
        return InspectionTexts[Random.Range(0, InspectionTexts.Length)];
    }
}
