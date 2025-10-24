using UnityEngine;
using UnityEngine.Events;
public class InteractAction_UnityEvent : InteractAction
{
    [SerializeField] UnityEvent OnInteract;

    public override void OnDoingAction()
    {
        OnInteract?.Invoke();
    }
}
