using UnityEngine;

public class PumpkinDecoration : Decoration
{
    [SerializeField] Sprite[] EyeSprites;
    [SerializeField] Sprite[] MouthSprites;
    [SerializeField] SpriteRenderer EyeRenderer;
    [SerializeField] SpriteRenderer MouthRenderer;
    [SerializeField] Transform FaceGroup;

    public float BiggestPumpkin = 137;

    public override void SetDecorationSO(DecorationSO decoration, HouseSO.EArea area)
    {
        base.SetDecorationSO(decoration, area);
        EyeRenderer.sprite = EyeSprites[Random.Range(0, EyeSprites.Length)];
        EyeRenderer.sortingLayerName = GetSortingLayer(area);

        MouthRenderer.sprite = MouthSprites[Random.Range(0, MouthSprites.Length)];
        MouthRenderer.sortingLayerName = GetSortingLayer(area);

        FaceGroup.localScale = Vector3.one * MainSpriteRenderer.bounds.size.y / 1.37f;
    }

    public override void SetPosition(Vector3 targetPos)
    {
        base.SetPosition(targetPos);
        EyeRenderer.sortingOrder = MainSpriteRenderer.sortingOrder + 1;
        MouthRenderer.sortingOrder = MainSpriteRenderer.sortingOrder + 1;
    }
}
