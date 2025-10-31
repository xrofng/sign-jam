using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutomationRandomSprite : Automation
{
    [Header("Obj Ref")]
    [SerializeField] Sprite[] PossibleSprites;
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
        MainSpriteRenderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
    }
}
