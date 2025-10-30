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
    public HouseColorSO[] AllColorPalettes;
    private int _currRand;
    private bool _colorSetted = false;

    private HouseColorSO CurrPalette => AllColorPalettes[_currRand];
    public string CurrentColorId => CurrPalette.PaletteId;

    private void Start()
    {
        if (_colorSetted == false)
        {
            _currRand = Random.Range(0, AllColorPalettes.Length);
            // Build a lookup for quick access
            ApplyColor();
        }
    }

    private void ApplyColor()
    {
        _colorSetted = true;
        foreach (SpriteRenderer spriteRenderer in GenHouseGroup.GetComponentsInChildren<SpriteRenderer>())
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
        if (spriteRenderer.gameObject.name.Contains(subName))
        {
            ApplyColors(spriteRenderer, CurrPalette, colorId);
        }
    }

    private void ApplyColors(SpriteRenderer spriteRenderer, HouseColorSO houseColorSO, string colorId)
    {
        //Debug.Log(houseColorSO.PaletteId + spriteRenderer.name + v  + houseColorSO.GetColor(v));
        if (CurrPalette.PaletteId == "Yellow")
        {
            Debug.Log(spriteRenderer.name + CurrPalette.GetColor(colorId));
        }
        spriteRenderer.color = houseColorSO.GetColor(colorId);
    }

    public void SetFixedPalette(string colorPaletteId)
    {
        for (int i = 0; i < AllColorPalettes.Length; i++)
        {
            if (AllColorPalettes[i].PaletteId == colorPaletteId)
            {
                _currRand = i;
                Debug.Log("Find color" + colorPaletteId);
                Debug.Log("-- curr" + CurrPalette.PaletteId + _colorSetted);
                ApplyColor();
            }
        }
    }
}
