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

    protected override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        return HouseName == houseSO.HouseName;
    }
}
