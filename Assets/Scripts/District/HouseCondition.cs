using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class HouseCondition
{
    [ReadOnly]
    public string Summary;

    public string ListLabel
    {
        get
        {
            return CalculateListLabel();
        }
    }

    protected abstract bool EvaluateCondition(HouseSO houseSO, House house);
    protected abstract string CalculateListLabel();
    public abstract HouseGenerationRequest GetGenerationRequest();

    public enum EComparisonMethod
    {
        Equal,
        NotEqual,
        More,
        Less,
        MoreEqual,

    }

    protected string GetComparisonMethodSymbol(EComparisonMethod comparisonMethod)
    {
        switch (comparisonMethod)
        {
            case EComparisonMethod.Equal: return "=";
            case EComparisonMethod.NotEqual: return "!=";
            case EComparisonMethod.Less: return "<";
            case EComparisonMethod.More: return ">";
            case EComparisonMethod.MoreEqual: return ">=";
        }
        return ";";
    }

}

public class HouseGenerationRequest
{
    public List<DecorationRequest> DecorationRequests = new List<DecorationRequest>();

    public string RequestedHouseName = "";

    public virtual void ApplyRequest()
    {

    }
}

public class DecorationRequest
{
    public DecorationSO Decoration;
    // index of required text, if -1 mean text shouldn't be specify
    public string RequestedInspectionText;
    public int MinQuantity;

    public DecorationRequest(DecorationSO decoration, int v1, int v2)
    {
        this.Decoration = decoration;
        if (v1 >= 0)
        {
            this.RequestedInspectionText = decoration.InspectionTexts[v1];
        }
        else
        {
            this.RequestedInspectionText = decoration.RandomInspectionText();
        }
        this.MinQuantity = v2;
    }
}