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
        Debug.Log("On Select");
        OnSelectEvent?.Invoke();
    }

    public virtual void OnDeselect()
    {
        Debug.Log("On Deselect");
        OnDeselectEvent?.Invoke();
    }
    public virtual void Interact()
    {
        if (_oneTimeInteract && InteractTime > 0) return;
        OnIntearctEvent?.Invoke();
        InteractTime++;
    }

}
