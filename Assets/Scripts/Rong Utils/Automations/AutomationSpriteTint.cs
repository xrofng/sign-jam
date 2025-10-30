using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomationSpriteTint : Automation
{
    [SerializeField] Color[] PossibleTints;

    [Header("Obj Ref")]
    [SerializeField] SpriteRenderer mainSpriteRenderer;

    public SpriteRenderer MainSpriteRenderer
    {
        get
        {
            if (mainSpriteRenderer == null)
            {
                TryGetComponent(out mainSpriteRenderer);
            }
            return mainSpriteRenderer;
        }
    }

    protected override void DoAutomation()
    {
        base.DoAutomation();
        MainSpriteRenderer.color = PossibleTints[Random.Range(0, PossibleTints.Length)];
    }
}
