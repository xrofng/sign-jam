using System.Collections.Generic;
using UnityEngine;
using static House;

/// <summary>
/// Randomly assigns colors to each part of a house.
/// Optionally uses a HouseColorSO as a palette reference.
/// </summary>
public class RandomHouseColor : MonoBehaviour
{
    [SerializeField] private Transform GenHouseGroup;
    [SerializeField] private HouseColorSO[] AllColorPalettes;
    private int _currRand;
    private Dictionary<string, List<SpriteRenderer>> partIdToRenderer;

    private HouseColorSO CurrPalette => AllColorPalettes[_currRand];

    private void Start()
    {
        _currRand = Random.Range(0, AllColorPalettes.Length);
        // Build a lookup for quick access
        partIdToRenderer = new Dictionary<string, List<SpriteRenderer>>();
        foreach(SpriteRenderer spriteRenderer in GenHouseGroup.GetComponentsInChildren<SpriteRenderer>())
        {
            ApplyColorToPart(spriteRenderer, "Wall", "TrapezoidShape");
            ApplyColorToPart(spriteRenderer, "Light", "Light");
            ApplyColorToPart(spriteRenderer, "Roof", "Roof");
            ApplyColorToPart(spriteRenderer, "Storey", "Roof_Based");
            ApplyColorToPart(spriteRenderer, "Door", "Door");
        }
    }

    private void ApplyColorToPart(SpriteRenderer spriteRenderer, string colorId, string subName)
    {
        Debug.Log(spriteRenderer.name);
        if (spriteRenderer.gameObject.name.Contains(subName))
        {
            ApplyColors(spriteRenderer, CurrPalette, colorId);
        }
    }

    private void ApplyColors(SpriteRenderer spriteRenderer,HouseColorSO houseColorSO, string v)
    {
        spriteRenderer.color = houseColorSO.GetColor(v);
    }
}
