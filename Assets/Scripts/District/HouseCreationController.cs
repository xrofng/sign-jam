using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class HouseCreationController : MMSingleton<HouseCreationController>
{
    [Title("Game")]
    [Range(0,100)]
    public float InitialGhostPercent = 20;
    public int IncrementWhenSurvuve = 20;
    public int DecrementWhenForced = 30;
    [ReadOnly]
    private int GhostPercent;

    public float HouseOffset = 5;
    public int PreferedDistrict = 4;
    public int HousePerDistrict = 4;

    [Header("Reference")]
    public HouseSO NormalHouseSO;
    public GhostHouseRulesetSO[] AllGhostHouseRuleset;
    public House HousePrefab;
    public Decoration SignPrefab;
    public AllDecorationsSO AllDecorationsSO;

    [Header("Scene Obj Ref")]
    public Transform StaringPos;
    public GhostHouseRulesetSO CurrentGhostHouseRuleset => AllGhostHouseRuleset[_currGhostHouseRulesetId];

    private int _currGhostHouseRulesetId;

    private void Start()
    {
        GhostPercent = (int)InitialGhostPercent;
        RandomRuleset();
        Vector3 spawnPos = StaringPos.position;
        for (int i = 0; i < PreferedDistrict; i++)
        {
            Decoration sign = Instantiate(SignPrefab, spawnPos, Quaternion.identity);
            spawnPos += Vector3.right * sign.MainSpriteRenderer.bounds.size.x / 2;
            spawnPos += Vector3.right * HouseOffset * Random.Range(1.2f, 1.5f);
            sign.InteractionDialogue.SetDialogueText(CurrentGhostHouseRuleset.DistrictName);

            for (int j = 0; j < HousePerDistrict; j++)
            {
                HouseSO.HouseSetting housedata = CreateHouse();
                House newHouse = Instantiate(HousePrefab, spawnPos, Quaternion.identity);
                newHouse.ConstructHouse(housedata);
                newHouse.SetRule(CurrentGhostHouseRuleset);
                //bool isGhost = isGhostHouse(newHouse, CurrentGhostHouseRuleset);
                //Debug.Log("ij " + i + "," + j + " " + isGhost);
                //newHouse.setIsGhostHouse(isGhost);

                spawnPos += Vector3.right * newHouse.HouseBound.size.x / 2;
                spawnPos += Vector3.right * HouseOffset;
            }
            _currGhostHouseRulesetId = Random.Range(0, AllGhostHouseRuleset.Length);
        }
    }



    bool isGhostHouse(House house, GhostHouseRulesetSO ghostRuleSO)
    {
        foreach (GhostRule ghostRule in ghostRuleSO.GhostRules)
        {
            if (ghostRule.EvaluateIsGhost(house))
            {
                return true;
            }
        }

        return false;
    }


    private HouseSO.HouseSetting CreateHouse()
    {
        // if ghost
        if (Random.Range(0, 99) < GhostPercent)
        {
            HouseRequest decorationRequestList
            = new HouseRequest(CurrentGhostHouseRuleset.GetRandomGhostRule().GhostConditions);
            GhostPercent -= DecrementWhenForced;
            return new HouseSO.HouseSetting(decorationRequestList);
        }
        GhostPercent += IncrementWhenSurvuve;
        return new HouseSO.HouseSetting(new HouseRequest(AllDecorationsSO));
    }

    private void RandomRuleset()
    {
        _currGhostHouseRulesetId = Random.Range(0, AllGhostHouseRuleset.Length);
    }
}


public struct HouseRequest
{
    public List<DecorationRequest> RequestedDecorations;
    public string RequestedHouseName;
    public string RequestedColorPaletteId;
    public int RequestedFloorNumber;

    public HouseRequest(List<HouseCondition> houseConditions)
    {
        RequestedDecorations = new List<DecorationRequest>();
        RequestedHouseName = string.Empty;
        RequestedColorPaletteId = string.Empty;
        RequestedFloorNumber = -1;

        foreach (HouseCondition condition in houseConditions)
        {
            if (condition.GetGenerationRequest().DecorationRequests.Count > 0)
            {
                RequestedDecorations.AddRange(condition.GetGenerationRequest().DecorationRequests);
            }
            if (condition.GetGenerationRequest().RequestedHouseName.Length > 0)
            {
                RequestedHouseName = condition.GetGenerationRequest().RequestedHouseName;
            }
            if (condition.GetGenerationRequest().RequstedColorPaletteId.Length > 0)
            {
                RequestedColorPaletteId = condition.GetGenerationRequest().RequstedColorPaletteId;
            }
            if (condition.GetGenerationRequest().RequestedFloorNumber > 0)
            {
                RequestedFloorNumber = condition.GetGenerationRequest().RequestedFloorNumber;
            }
        }
    }

    public HouseRequest(AllDecorationsSO allDecorations)
    {
        RequestedDecorations = new List<DecorationRequest>();
        for (int i = 0; i < Random.Range(4, 7); i++)
        {
            DecorationSO randDecSo = allDecorations.GetRandomDecoration();
            int randTextId = 0;
            if (randDecSo.HasInpectionText())
            {
                randTextId = randDecSo.RandomInspectionTextId(); ;
            }
            RequestedDecorations.Add(new DecorationRequest(randDecSo, randTextId, 1));
        }

        RequestedHouseName = allDecorations.GetRandomName();
        RequestedColorPaletteId = allDecorations.GetRandomHouseColorId();

        RequestedFloorNumber = Random.Range(1, 2);
        RequestedFloorNumber = RequestedFloorNumber + Random.Range(0, 4) == 0 ? 1 : 0;
    }
}
