using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HouseNameCond : HouseCondition
{
    public string HouseName;

    protected override string CalculateListLabel()
    {
        Summary = "Is sign " + HouseName;
        return Summary;
    }

    public override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        return HouseName == houseSO.HouseData.HouseName;
    }

    public override HouseGenerationRequest GetGenerationRequest()
    {
        return new HouseNameGenRequest(HouseName);
    }
}

public class HouseNameGenRequest : HouseGenerationRequest
{
    public HouseNameGenRequest(string houseName)
    {
        RequestedHouseName = houseName;
    }

    public override void ApplyRequest()
    {
        
    }
}
