using TMPro;
using UnityEngine;

public class ScoreUI : BaseTextUI, IEventSubcriber<EvsScoreChanged>
{
    private string _currScore;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(EvsScoreChanged eventType)
    {
        _currScore = eventType.NewScore.ToString();
    }

    protected override string GetText()
    {
        return _currScore;
    }
}
