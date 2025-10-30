using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class WallColorCond : HouseCondition
{
    public enum EHouseColorId
    {
        None = 0,
        Blue,
        Yellow,
        Sky,
        Teal,
        Grey
    }
    [Required]
    public EHouseColorId PaletteId;

    protected override string CalculateListLabel()
    {
        return "Is wall " + PaletteId;
    }

    public override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        return house.GetComponent<RandomHouseColor>().CurrentColorId == PaletteId.ToString();
    }

    public override HouseGenerationRequest GetGenerationRequest()
    {
        return new HouseWallColorGenRequest(PaletteId.ToString());
    }
}

public class HouseWallColorGenRequest : HouseGenerationRequest
{
    public HouseWallColorGenRequest(string paletteId)
    {
        RequstedColorPaletteId = paletteId;
    }

    public override void ApplyRequest()
    {

    }
}
