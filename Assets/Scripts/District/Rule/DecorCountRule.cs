using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class DecorCountCondition : HouseCondition
{
    [Required]
    public DecorationSO Decoration;
    public EComparisonMethod ComparisonMethod = EComparisonMethod.MoreEqual;
    public int TargetNumber = 1;

    protected override string CalculateListLabel()
    {
        if (Decoration)
        {
            return Decoration.name + " " + GetComparisonMethodSymbol(ComparisonMethod) + " " + TargetNumber;
        }
        return " NEED assign Decoration";
    }

    protected override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        switch (ComparisonMethod)
        {
            case EComparisonMethod.Equal:
                return house.DecorData[Decoration.DecorationID].Count == TargetNumber;
            case EComparisonMethod.NotEqual:
                return house.DecorData[Decoration.DecorationID].Count != TargetNumber;
            case EComparisonMethod.Less:
                return house.DecorData[Decoration.DecorationID].Count < TargetNumber;
            case EComparisonMethod.More:
                return house.DecorData[Decoration.DecorationID].Count > TargetNumber;
            case EComparisonMethod.MoreEqual:
                return house.DecorData[Decoration.DecorationID].Count >= TargetNumber;
        }
        return false;
    }
}
