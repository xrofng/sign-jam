using UnityEngine;
using UnityEngine.Events;
public class InteractAction_UnityEvent : InteractAction
{
    [SerializeField] UnityEvent _onInteract;
    [SerializeField] float _delay = 0;
    protected override void OnDoingAction()
    {
        Invoke(nameof(InvokeUnityEvent), _delay);
    }

    void InvokeUnityEvent()
    {
        _onInteract?.Invoke();
    }
}
