using UnityEngine;
public class Interaction_OnSelect : InteractAction
{
    [SerializeField] SpriteRenderer _outLine;
    protected override void OnDeselect()
    {
        _outLine.enabled = false;
    }

    protected override void OnDoingAction()
    {
    }

    protected override void OnSelect()
    {
        _outLine.enabled = true;
    }


}
