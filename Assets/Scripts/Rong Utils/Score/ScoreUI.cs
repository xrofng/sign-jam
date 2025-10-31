using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class ScoreUI : BaseTextUI, IEventSubcriber<EvsScoreChanged>
{
    private string _currScore;
    [SerializeField] MMF_Player GetScoreMMF;
    [SerializeField] MMF_Player LoseScoreMMF;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
        _currScore = "0";
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(EvsScoreChanged eventType)
    {
        _currScore = eventType.NewScore.ToString();
        if (eventType.ScoreIncrement > 0)
        {
            GetScoreMMF?.PlayFeedbacks();
        }
        else
        {
            LoseScoreMMF?.PlayFeedbacks();
        }
    }

    protected override string GetText()
    {
        return _currScore;
    }
}
