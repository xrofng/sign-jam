using UnityEngine;

[System.Serializable]
public class HouseNameCondition : HouseCondition
{
    public string HouseName;

    protected override string CalculateListLabel()
    {
        return "Is " + HouseName;
    }

    protected override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        return HouseName == houseSO.HouseName;
    }
}
