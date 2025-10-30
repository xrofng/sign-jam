using UnityEngine;

public class Decoration : ObjectWithSprite
{
    [SerializeField] InteractAction_Dialogue interactAction_Dialogue;

    public string InspectionText => interactAction_Dialogue.GetDialogueText();

    public InteractAction_Dialogue InteractionDialogue => interactAction_Dialogue;
    public HouseSO.EArea area;

    private DecorationSO decorationSO;

    public virtual void SetDecorationSO(DecorationSO decoration, HouseSO.EArea area)
    {
        decorationSO = decoration;
        MainSpriteRenderer.sprite = decorationSO.GetRandomTexture();
        this.area = area;
        MainSpriteRenderer.sortingLayerName = GetSortingLayer(area);
        if (decorationSO.HasInpectionText())
        {
            interactAction_Dialogue.SetDialogueText(decorationSO.RandomInspectionText());
        }
    }

    protected string GetSortingLayer(HouseSO.EArea area)
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

    public virtual void SetPosition(Vector3 targetPos)
    {
        transform.position = targetPos;

        if (decorationSO)
        {
            MainSpriteRenderer.sortingOrder = (int)Mathf.Abs(transform.position.y * 100);
        }
    }

    public void SetReady()
    {
        CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();
        circleCol.isTrigger = true;
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