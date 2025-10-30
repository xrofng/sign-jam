using System.Collections.Generic;
using UnityEngine;

public class House : BetterMonoBehaviour
{
    [SerializeField] HouseSO TestHouseSO;
    [SerializeField] HouseArea[] HouseAreas;
    [SerializeField] InteractAction_Dialogue SignDialogue;
    [SerializeField] SpriteRenderer BoundSprite;
    [SerializeField] ProceduralHouseGenerator HouseGenerator;

    [System.Serializable]
    public class HouseArea
    {
        public HouseSO.EArea Area;
        public SpriteRenderer[] AreaSpriteRenderer;

        public int DecorCount { get; set; }

        public SpriteRenderer GetSpriteRenderer()
        {
            return AreaSpriteRenderer[Random.Range(0, AreaSpriteRenderer.Length)];
        }
    }

    private Dictionary<HouseSO.EArea, HouseArea> _areaToHouseArea;

    public class HouseDecorData
    {
        public List<Decoration> Decorations = new List<Decoration>();
        public int Count => Decorations.Count;

        public HouseDecorData(Decoration decoration)
        {
            Decorations.Add(decoration);
        }
    }
    private Dictionary<string, HouseDecorData> _decorToData;
    private float _randFromBell;

    public Dictionary<string, HouseDecorData> DecorData => _decorToData;

    public const float GROUND_POSY = -1.5f;
    public Bounds HouseBound => BoundSprite.bounds;


    protected override void Awake()
    {
        base.Awake();

        RandomGenerateHouse();

        CacheAreaToDict();
        // Optional: initialize if HouseSO exists
        if (TestHouseSO != null)
        {
            ConstructHouse(TestHouseSO.HouseData);
        }
    }

    private void RandomGenerateHouse()
    {
        _randFromBell = MathUtils.BellCurve01(Random.Range(.0f, 1));
        HouseGenerator.customWidth = Mathf.Lerp(4, 7, _randFromBell);

        _randFromBell = MathUtils.BellCurve01(Random.Range(.0f, 1));
        HouseGenerator.customHeight = Mathf.Lerp(3, 7, _randFromBell);

        HouseGenerator.transform.localPosition = Vector3.up * HouseGenerator.customHeight / 2;
    }

    private void CacheAreaToDict()
    {
        // Construct the dictionary
        _areaToHouseArea = new Dictionary<HouseSO.EArea, HouseArea>();

        foreach (var area in HouseAreas)
        {
            if (area == null)
                continue;

            if (!_areaToHouseArea.ContainsKey(area.Area))
            {
                _areaToHouseArea.Add(area.Area, area);
            }
            else
            {
                Debug.LogWarning($"Duplicate area '{area.Area}' detected in {name}.");
            }
        }
    }

    public void ConstructHouse(HouseSO.HouseSetting houseSO)
    {
        _decorToData = new Dictionary<string, HouseDecorData>();
        foreach (DecorationSO decoration in houseSO.Decorations)
        {
            Decoration newDec = Instantiate(decoration.BasePrefab, transform) as Decoration;
            HouseSO.EArea area = RandomValueArea(decoration.AssociatedArea);
            newDec.SetDecorationSO(decoration, area);

            if (_decorToData.ContainsKey(decoration.DecorationID) == false)
            {
                _decorToData.Add(decoration.DecorationID, new HouseDecorData(newDec));
            }
            else
            {
                _decorToData[decoration.DecorationID].Decorations.Add(newDec);
            }

            newDec.SetPosition(CalculateDecorationPosition(area));
            newDec.SetReady();
        }

        SignDialogue.SetDialogueText(houseSO.HouseName);
    }

    private Vector3 CalculateDecorationPosition(HouseSO.EArea area)
    {
        _areaToHouseArea[area].DecorCount += 1;
        return TransformUtils.RandomPointInsideSprite(_areaToHouseArea[area].GetSpriteRenderer());
    }

    private HouseSO.EArea RandomValueArea(HouseSO.EArea associatedArea)
    {
        HouseSO.EArea area = EnumUtils.GetRandomFlag(associatedArea);
        while (_areaToHouseArea[area].DecorCount + 1 > GetDecorLimit(area))
        {
            area = EnumUtils.GetRandomFlag(associatedArea);
        }

        return area;
    }

    private int GetDecorLimit(HouseSO.EArea area)
    {
        switch (area)
        {
            case HouseSO.EArea.None:
                return 0;
            case HouseSO.EArea.FrontYard:
                return 3;
            case HouseSO.EArea.Backyard:
                return 1;
            case HouseSO.EArea.Roof:
                return 2;
            case HouseSO.EArea.Fence:
                return 1;
            case HouseSO.EArea.Wall:
                return 3;
            case HouseSO.EArea.Door:
                return 1;
            default:
                return 0;
        }
    }
}
