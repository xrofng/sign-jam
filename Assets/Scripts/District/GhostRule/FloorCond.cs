using Sirenix.OdinInspector;
using System;
using Unity.Collections.LowLevel.Unsafe;

[System.Serializable]
public class FloorCond : HouseCondition
{
    [Required]
    public EComparisonMethod ComparisonMethod = EComparisonMethod.MoreEqual;
    public int TargetNumber = 1;

    protected override string CalculateListLabel()
    {
        if (TargetNumber <=0)
        {
            Summary = " Floor Cond: " + TargetNumber;
            return Summary;
        }
        return " Need To Assign Floor";
    }

    public override bool EvaluateCondition(HouseSO houseSO, House house)
    {
        return true;
    }

    public override HouseGenerationRequest GetGenerationRequest()
    {
        return new HouseFloorGenRequest(TargetNumber);
    }
}

public class HouseFloorGenRequest : HouseGenerationRequest
{
    public HouseFloorGenRequest(int floorNumber)
    {
        RequestedFloorNumber = floorNumber;
    }

    public override void ApplyRequest()
    {

    }
}
