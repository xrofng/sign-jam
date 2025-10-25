using UnityEngine;

public abstract class InteractAction : MonoBehaviour
{
    [SerializeField] InteractObject interactObject;


    private void Start()
    {
        interactObject.OnIntearctEvent += OnDoingAction;
        interactObject.OnDeselectEvent += OnDeselect;
        interactObject.OnSelectEvent += OnSelect;
        OnStart();
    }


    protected virtual void OnStart()
    {

    }

    protected abstract void OnDoingAction();

    protected virtual void OnSelect()
    {

    }

    protected virtual void OnDeselect()
    {

    }

}
