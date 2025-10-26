using System;
using UnityEngine;

public class InteractObject : ObjectWithSprite, IEventSubcriber<Decoration.EvsDecorationReadied>
{
    public event System.Action OnSelectEvent;
    public event System.Action OnDeselectEvent;
    public event System.Action OnIntearctEvent;

    [SerializeField] Transform _playerDestination;
    [SerializeField] bool _oneTimeInteract;

    int InteractTime = 0;

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

    public Vector3 GetNearObjectPos(Vector3 interactorPos, Bounds playerBound)
    {
        Vector3 dir = (interactorPos - _playerDestination.transform.position).normalized;
        dir.y = 0;
        dir.z = 0;

        Vector3 offset = dir * (MainSpriteRenderer.bounds.size.x / 2);
        offset += dir * (playerBound.size.x / 2);
        offset += Vector3.down * (playerBound.size.y / 4);

        return _playerDestination.transform.position + offset;
    }

    private void OnDrawGizmosSelected()
    {
        if (!MainSpriteRenderer) { return; }
        Gizmos.DrawLine(_playerDestination.transform.position + Vector3.right * (MainSpriteRenderer.bounds.size.x / 2),
            _playerDestination.transform.position + Vector3.left * (MainSpriteRenderer.bounds.size.x / 2));
    }

    public void OnEventBusTrigger(Decoration.EvsDecorationReadied eventType)
    {
        if (eventType.ReadiedDecor.Equals(gameObject))
        {
            if (eventType.Area == HouseSO.EArea.Wall)
            {
                _playerDestination.position = VectorUtils.SetY(_playerDestination.position, House.GROUND_POSY);
            }
        }
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
