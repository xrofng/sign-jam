using System.Collections.Generic;
using UnityEngine;

public class House : BetterMonoBehaviour
{
    public event System.Action<bool> OnSetGhost;

    [SerializeField] HouseSO TestHouseSO;
    [SerializeField] HouseArea[] HouseAreas;
    [SerializeField] public InteractAction_Dialogue SignDialogue;

    [SerializeField] SpriteRenderer BoundSprite;
    [SerializeField] ProceduralHouseGenerator HouseGenerator;

    public bool IsGhostHouse;



    public void SetIsGhostHouse(bool set)
    {
        IsGhostHouse = set;
        OnSetGhost?.Invoke(set);
    }


    [System.Serializable]
    public class HouseArea
    {
        public HouseSO.EArea Area;
        public SpriteRenderer[] AreaSpriteRenderer;

        public int DecorCount { get; set; }

        public SpriteRenderer GetSpriteRenderer()
        {
            return AreaSpriteRenderer[UnityEngine.Random.Range(0, AreaSpriteRenderer.Length)];
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
    private GhostHouseRulesetSO _ghostHouseRulesetSO;

    public Dictionary<string, HouseDecorData> DecorData => _decorToData;

    public const float GROUND_POSY = -1.5f;
    public Bounds HouseBound => BoundSprite.bounds;

    public int FloorCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();


        CacheAreaToDict();
        // Optional: initialize if HouseSO exists
        if (TestHouseSO != null)
        {
            RandomGenerateHouse();
            ConstructHouse(TestHouseSO.HouseData);
        }
    }


    private void RandomGenerateHouse(int floor = -1)
    {
        _randFromBell = MathUtils.BellCurve01(UnityEngine.Random.Range(.0f, 1));
        HouseGenerator.customWidth = Mathf.Lerp(4, 7, _randFromBell);

        _randFromBell = MathUtils.BellCurve01(UnityEngine.Random.Range(.0f, 1));
        if (floor > 0)
        {
            HouseGenerator.customHeight = GetHeight(floor);
        }
        else
        {
            float chance = Random.Range(0, 10);
            float height = GetHeight(1); // 1 Floor 
            if (chance == 0)             // 3 Floor 
            {
                height = GetHeight(3);
            }
            else if (chance >= 6)       // 2 Floor
            {
                height = GetHeight(2);
            }
            HouseGenerator.customHeight = height;
        }

        HouseGenerator.transform.localPosition = Vector3.up * HouseGenerator.customHeight / 2;
    }

    private float GetHeight(int floor)
    {
        FloorCount = floor;
        switch (floor)
        {
            case 1: return Random.Range(3.5f, 4.5f);
            case 2: return Random.Range(6f, 7f);
            case 3: return Random.Range(8.5f, 9f);
        }
        return 5;
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

        if (houseSO.Decorations.Count > 0)
        {
            ConstructDecorations(houseSO.Decorations);
        }
        else
        {

        }

        if (houseSO.Floor > 0)
        {
            RandomGenerateHouse(houseSO.Floor);
        }
        else
        {
            RandomGenerateHouse();
        }

        if (houseSO.HouseName.Length > 0)
        {
            SignDialogue.SetDialogueText(houseSO.HouseName);
        }
        else
        {

        }

        if (houseSO.ColorPaletteId.Length > 0)
        {
            GetComponent<RandomHouseColor>().SetFixedPalette(houseSO.ColorPaletteId);
        }
        else
        {

        }
    }

    private void ConstructFloor(int floor)
    {

    }

    private void ConstructDecorations(List<DecorationSO> decorationSO)
    {
        foreach (DecorationSO decoration in decorationSO)
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
    }

    private Vector3 CalculateDecorationPosition(HouseSO.EArea area)
    {
        _areaToHouseArea[area].DecorCount += 1;
        return TransformUtils.RandomPointInsideSprite(_areaToHouseArea[area].GetSpriteRenderer());
    }

    private HouseSO.EArea RandomValueArea(HouseSO.EArea associatedArea)
    {
        HouseSO.EArea area = EnumUtils.GetRandomFlag(associatedArea);
        /*while (_areaToHouseArea[area].DecorCount + 1 > GetDecorLimit(area))
        {
            area = EnumUtils.GetRandomFlag(associatedArea);
        }*/

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

    public void SetRule(GhostHouseRulesetSO currentGhostHouseRuleset)
    {
        _ghostHouseRulesetSO  = currentGhostHouseRuleset;
    }


    bool EvaluateIsGhostHouse()
    {
        foreach (GhostRule ghostRule in _ghostHouseRulesetSO.GhostRules)
        {
            if (ghostRule.EvaluateIsGhost(this))
            {
                return true;
            }
        }

        return false;
    }
}
