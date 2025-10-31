using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class InteractAction_Dialogue : InteractAction
{
    [SerializeField, TextArea] private string _dialogueText;
    public string DialogueText => _dialogueText;

    [SerializeField] float letterDelay = .1f;
    [SerializeField] float textShowingDuration = 2;
    [SerializeField] TextMeshPro _dialougeTextMeshPro;

    protected override void OnStart()
    {
        _dialougeTextMeshPro.text = "";
    }

    protected override void OnDoingAction()
    {
        StopAllCoroutines();
        _dialougeTextMeshPro.text = "";
        EventBus.TriggerEvent(new EvsDialogueAction(this));
        StartCoroutine(textAnimation());
    }

    IEnumerator textAnimation()
    {
        _dialougeTextMeshPro.text += _dialogueText;
        //for (int i = 0; i < _dialogueText.Length; i+= )
        //{
        //    _dialougeTextMeshPro.text += _dialogueText[i];
        //    yield return new WaitForSeconds(letterDelay);
        //}
        yield return new WaitForSeconds(textShowingDuration);
        _dialougeTextMeshPro.text = "";

    }

    public void SetDialogueText(string houseName)
    {
        _dialogueText = houseName;
    }

    public string GetDialogueText()
    {
        return _dialogueText;
    }
}

public struct EvsDialogueAction
{
    public InteractAction_Dialogue interactAction_Dialogue;

    public EvsDialogueAction(InteractAction_Dialogue interactAction_Dialogue)
    {
        this.interactAction_Dialogue = interactAction_Dialogue;
    }
}