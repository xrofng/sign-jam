using UnityEngine;

public class Decoration : BetterMonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if ( _spriteRenderer == null)
            {
                TryGetComponent(out _spriteRenderer);
            }
            return _spriteRenderer;
        }
    }

    public HouseSO.EArea area;

    private DecorationSO decorationSO;

    public void SetDecorationSO(DecorationSO decoration, HouseSO.EArea area)
    {
        decorationSO = decoration;
        SpriteRenderer.sprite = decorationSO.GetRandomTexture();
        this.area = area;
        SpriteRenderer.sortingLayerName = GetSortingLayer(area);
    }

    private string GetSortingLayer(HouseSO.EArea area)
    {
        switch (area)
        {
            case HouseSO.EArea.None:
                return "Decoration";
            case HouseSO.EArea.FrontYard:
                return "Decoration";
            case HouseSO.EArea.Backyard:
                return "Default";
            case HouseSO.EArea.Roof:
                return "Decoration";
            case HouseSO.EArea.Fence:
                return "Decoration";
            case HouseSO.EArea.Wall:
                return "Decoration";
            case HouseSO.EArea.Door:
                return "Decoration";
            default:
                return "Decoration";
        }
    }

    public void SetPosition(Vector3 targetPos)
    {
        transform.position = targetPos;

        if (decorationSO)
        {
            SpriteRenderer.sortingOrder = (int)Mathf.Abs(transform.position.y * 100);
        }
    }

    public void SetReady()
    {
        gameObject.AddComponent<CircleCollider2D>();
        EventBus.TriggerEvent(new EvsDecorationReadied(gameObject, area));
    }

    public struct EvsDecorationReadied
    {
        public GameObject ReadiedDecor;
        public HouseSO.EArea Area;

        public EvsDecorationReadied(GameObject readiedDecor, HouseSO.EArea area)
        {
            ReadiedDecor = readiedDecor;
            Area = area;
        }
    }
}