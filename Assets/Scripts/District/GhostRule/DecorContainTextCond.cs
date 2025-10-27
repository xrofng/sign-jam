using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class DecorContainTextCond : HouseCondition
{
    [Required]
    public DecorationSO Decoration;
    public string ContainingText;

    protected override string CalculateListLabel()
    {
        if (Decoration)
        {
            Summary = ContainingText + " on " + Decoration.DecorationID;
            return Summary;
        }
        return " NEED Decoration";
    }

    protected override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        if (house.DecorData.ContainsKey(Decoration.DecorationID) == false)
        {
            return false;
        }
        foreach (Decoration decorationObject in house.DecorData[Decoration.DecorationID].Decorations)
        {
            if (decorationObject.InspectionText.Contains(ContainingText))
            {
                return true;
            }
        }
        return false;
    }
}
