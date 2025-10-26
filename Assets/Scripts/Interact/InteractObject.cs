using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public event System.Action OnSelectEvent;
    public event System.Action OnDeselectEvent;
    public event System.Action OnIntearctEvent;

    [SerializeField] Transform _playerDestination;
    [SerializeField] bool _oneTimeInteract;
    public Transform PlayerDestination => _playerDestination;

    int InteractTime = 0;

    public virtual void OnSelect()
    {
        if (_oneTimeInteract && InteractTime > 0) return;
        OnSelectEvent?.Invoke();
        EventBus.TriggerEvent(new EvsSelectInteractObject(true));
    }

    public virtual void OnDeselect()
    {
        OnDeselectEvent?.Invoke();
        EventBus.TriggerEvent(new EvsSelectInteractObject(false));
    }
    public virtual void Interact()
    {
        if (_oneTimeInteract && InteractTime > 0) return;
        OnIntearctEvent?.Invoke();
        InteractTime++;
        OnDeselect();
    }

    /// <summary>
    /// Evs - stand for Event Struct
    /// </summary>
    public struct EvsSelectInteractObject
    {
        public bool IsSelected;

        public EvsSelectInteractObject(bool isSelected)
        {
            IsSelected = isSelected;
        }
    }
}
