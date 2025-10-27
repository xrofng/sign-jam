using Sirenix.OdinInspector;
using System;
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