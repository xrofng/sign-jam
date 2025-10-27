using MoreMountains.Tools;
using UnityEngine;

public class District : MMSingleton<District>
{
    [SerializeField] HouseRulesetSO[] AllRuleset;
    public HouseRulesetSO CurrentRuleset;

    protected override void Awake()
    {
        base.Awake();
        CurrentRuleset = AllRuleset[Random.Range(0, AllRuleset.Length)];
        EventBus.TriggerEvent(new EvsRulesetChanged(CurrentRuleset));
    }

    public struct EvsRulesetChanged
    {
        public HouseRulesetSO Ruleset;

        public EvsRulesetChanged(HouseRulesetSO ruleset)
        {
            Ruleset = ruleset;
        }
    }
}
