using UnityEngine;

public class interactAction_PlayPlayerAnimation : InteractAction
{

    [SerializeField] string animationToPlay;
    PlayerAnimationEvent playerAnimation;
    protected override void OnStart()
    {
        playerAnimation = FindAnyObjectByType<PlayerAnimationEvent>();
    }

    protected override void OnDoingAction()
    {
        playerAnimation.TriggerAnimation(animationToPlay);
    }
}
