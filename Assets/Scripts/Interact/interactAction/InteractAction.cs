using UnityEngine;

public abstract class InteractAction : MonoBehaviour
{
    [SerializeField] InteractObject interactObject;


    private void Start()
    {
        interactObject.OnIntearctEvent += OnDoingAction;
        OnStart();
    }


    protected virtual void OnStart()
    {

    }

    public abstract void OnDoingAction();
}
