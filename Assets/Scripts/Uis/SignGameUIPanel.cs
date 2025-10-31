using MoreMountains.Feedbacks;
using UnityEngine;

public class SignGameUIPanel : BetterMonoBehaviour, IEventSubcriber<Newspaper.EvsGameStateChanged>
{
    [SerializeField] MMF_Player GameStartMMF;
    [SerializeField] CanvasGroup GameUI;

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

    public void OnEventBusTrigger(Newspaper.EvsGameStateChanged eventType)
    {
        if (eventType.GameState == Newspaper.EGameState.Game)
        {
            GameUI.alpha = 1;
            GameStartMMF?.PlayFeedbacks();
        }
        else
        {
            GameUI.alpha = 0;
        }
    }
}
