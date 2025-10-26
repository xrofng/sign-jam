using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class InteractAction_Dialogue : InteractAction
{
    [SerializeField, TextArea] private string _dialogueText;
    [SerializeField] float textShowDelay;
    [SerializeField] float textDisappearTimer;
    [SerializeField] TextMeshPro _dialougeTextMeshPro;

    protected override void OnStart()
    {
        _dialougeTextMeshPro.text = "";
    }

    protected override void OnDoingAction()
    {
        StopAllCoroutines();
        _dialougeTextMeshPro.text = "";
        StartCoroutine(textAnimation());
    }


    IEnumerator textAnimation()
    {
        for (int i = 0; i < _dialogueText.Length; i++)
        {
            _dialougeTextMeshPro.text += _dialogueText[i];

            yield return new WaitForSeconds(textShowDelay);
        }
        yield return new WaitForSeconds(textDisappearTimer);
        _dialougeTextMeshPro.text = "";

    }

    public void SetDialogueText(string houseName)
    {
        _dialogueText = houseName;
    }
}
