using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(MoreMountains.Feedbacks.MMF_Player))]
public class InteractAction_MMF : InteractAction
{
    MMF_Player mmf;

    protected override void OnStart()
    {
        base.OnStart();
        TryGetComponent(out mmf);
    }

    protected override void OnDoingAction()
    {
        mmf.PlayFeedbacks();
    }
}
