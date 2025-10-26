using UnityEngine;

public class SignGameCursorController : CursorController, IEventSubcriber<InteractObject.EvsSelectInteractObject>
{
    public Texture2D SelectedCursor;

    private void OnEnable()
    {
        EventBusRegister.EventBusSubcribe(this);
    }

    private void OnDisable()
    {
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(InteractObject.EvsSelectInteractObject eventType)
    {
        if (eventType.IsSelected)
        {
            SetCursorTexture(SelectedCursor);
        }
        else
        {
            SetCursorTexture(DefaultCursor);
        }
    }
}
