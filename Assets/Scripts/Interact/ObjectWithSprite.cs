using UnityEngine;

public class ObjectWithSprite : BetterMonoBehaviour
{
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
}
