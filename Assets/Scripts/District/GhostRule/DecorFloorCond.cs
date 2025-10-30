using Sirenix.OdinInspector;
using System;
using Unity.Collections.LowLevel.Unsafe;

[System.Serializable]
public class DecorFloorCond : HouseCondition
{
    [Required]
    public DecorationSO Decoration;
    public EComparisonMethod ComparisonMethod = EComparisonMethod.MoreEqual;
    public int TargetNumber = 1;


    protected override string CalculateListLabel()
    {
        if (Decoration)
        {
            Summary = Decoration.DecorationID + " " + GetComparisonMethodSymbol(ComparisonMethod) + " " + TargetNumber;
            return Summary;
        }
        return " NEED Decoration";
    }

    protected override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        return true;
    }

    public override HouseGenerationRequest GetGenerationRequest()
    {
        return new DecorationGenRequest(Decoration, TargetNumber);
    }
}
