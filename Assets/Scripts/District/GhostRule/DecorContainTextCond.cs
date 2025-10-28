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

    public override HouseGenerationRequest GetGenerationRequest()
    {
        return new DecorationGenRequest(Decoration, 1, ContainingText);
    }
}

public class DecorationGenRequest : HouseGenerationRequest
{
    public DecorationGenRequest(DecorationSO decoration, int requiredQuantity, string containingText)
    {
        for (int i = 0; i < decoration.InspectionTexts.Length; i++)
        {
            if (decoration.InspectionTexts[i].Contains(containingText))
            {
                DecorationRequests.Add(new DecorationRequest(decoration, i, requiredQuantity));
            }
        }
    }

    public DecorationGenRequest(DecorationSO decoration, int requiredQuantity)
    {
        DecorationRequests.Add(new DecorationRequest(decoration, -1, requiredQuantity));
    }
}