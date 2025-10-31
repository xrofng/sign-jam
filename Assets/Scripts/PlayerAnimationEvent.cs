using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TriggerAnimation(string AnimationName)
    {
        animator.Play(AnimationName, 0, 0);
    }

    public void OnPickUpEvent()
    {

    }
}
