using MoreMountains.Tools;
using UnityEngine;

public class District : MMSingleton<District>
{
    [SerializeField] GhostHouseRulesetSO[] AllRuleset;
    public GhostHouseRulesetSO CurrentRuleset;

    protected override void Awake()
    {
        base.Awake();
        CurrentRuleset = AllRuleset[Random.Range(0, AllRuleset.Length)];
        EventBus.TriggerEvent(new EvsRulesetChanged(CurrentRuleset));
    }

    public struct EvsRulesetChanged
    {
        public GhostHouseRulesetSO Ruleset;

        public EvsRulesetChanged(GhostHouseRulesetSO ruleset)
        {
            Ruleset = ruleset;
        }
    }
}
