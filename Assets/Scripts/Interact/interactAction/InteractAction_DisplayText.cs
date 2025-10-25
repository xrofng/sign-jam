using UnityEngine;
public class InteractAction_DisplayText : InteractAction
{
    [SerializeField] string displayText;

    protected override void OnDoingAction()
    {
    }

    protected override void OnDeselect()
    {
        TextFollowMouseCursor.Instance.SetUpText("");

    }

    protected override void OnSelect()
    {
        TextFollowMouseCursor.Instance.SetUpText(displayText);
    }
}
