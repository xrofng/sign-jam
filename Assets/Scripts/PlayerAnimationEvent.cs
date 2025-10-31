using UnityEngine;

public class PlayerAnimationEvent : ObjectWithSprite
{
    Animator animator;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAnimation(string AnimationName)
    {
        if (MainSpriteRenderer.flipX)
        {
            animator.SetFloat("floatIsFlip", 0);
        }
        else
        {
            animator.SetFloat("floatIsFlip", 1);
        }
        animator.Play(AnimationName, 0, 0);
    }

    //public void OnPickUpEvent()
    //{

    //}
}
