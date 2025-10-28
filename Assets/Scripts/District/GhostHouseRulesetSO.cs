using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "R_", menuName = "SignGame/Ruleset")]
public class GhostHouseRulesetSO : ScriptableObject
{
    public string DistrictName = "Babbling Gum";

    [Tooltip("RULE to be considered as Ghost House")]
    [ListDrawerSettings(ListElementLabelName = "RuleSummary")]
    public List<GhostRule> GhostRules = new List<GhostRule>();

    [Tooltip("How many RULE should be True to consider house as Ghost.")]
    public int RequiredScoreForGhost = 1;

    public GhostRule GetRandomGhostRule()
    {
        return GhostRules[Random.Range(0, GhostRules.Count)];
    }
}

[System.Serializable]
public class GhostRule
{
    public string RuleSummary = null;
    [Tooltip("If all of condition in list is True, the Rule will return 1 True score to be Ghost.")]
    [SerializeReference]
    [ListDrawerSettings(ListElementLabelName = "ListLabel", ShowIndexLabels = true)]
    List<HouseCondition> HouseConditions;

    public List<HouseCondition> GhostConditions => HouseConditions;
}
