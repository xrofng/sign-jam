using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void TriggerAnimation(string AnimationName)
    {
        if (spriteRenderer.flipX)
        {
            animator.SetFloat("floatIsFlip", 0);
        }
        else
        {
            animator.SetFloat("floatIsFlip", 1);
        }
        animator.Play(AnimationName, 0, 0);
    }

    public void OnPickUpEvent()
    {

    }
}
