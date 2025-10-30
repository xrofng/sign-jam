using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HouseCreationController : MMSingleton<HouseCreationController>
{
    public float HouseOffset = 5;
    public int PreferedDistrict = 4;
    public int HousePerDistrict = 4;

    [Header("Reference")]
    public HouseSO NormalHouseSO;
    public GhostHouseRulesetSO[] AllGhostHouseRuleset;
    public House HousePrefab;
    public Decoration SignPrefab;

    [Header("Scene Obj Ref")]
    public Transform StaringPos;
    public GhostHouseRulesetSO CurrentGhostHouseRuleset => AllGhostHouseRuleset[_currGhostHouseRulesetId];

    private int _currGhostHouseRulesetId;

    private void Start()
    {
        RandomRuleset();
        Vector3 spawnPos = StaringPos.position;
        for(int i = 0; i < PreferedDistrict; i++)
        {
            Decoration sign = Instantiate(SignPrefab, spawnPos, Quaternion.identity);
            spawnPos += Vector3.right * sign.MainSpriteRenderer.bounds.size.x / 2;
            spawnPos += Vector3.right * HouseOffset * Random.Range(1.2f,1.5f);
            sign.InteractionDialogue.SetDialogueText(CurrentGhostHouseRuleset.DistrictName);

            for (int j = 0; j < HousePerDistrict; j++)
            {
                HouseSO.HouseSetting housedata = CreateHouse();
                House newHouse = Instantiate(HousePrefab, spawnPos, Quaternion.identity);
                newHouse.ConstructHouse(housedata);
                spawnPos += Vector3.right * newHouse.HouseBound.size.x / 2;
                spawnPos += Vector3.right * HouseOffset;
            }
            _currGhostHouseRulesetId = Random.Range(0, AllGhostHouseRuleset.Length);
        }
    }

    private HouseSO.HouseSetting CreateHouse()
    {
        // if ghost
        if (Random.Range(0,2) == 1)
        {
            HouseRequest decorationRequestList
            = new HouseRequest(CurrentGhostHouseRuleset.GetRandomGhostRule().GhostConditions);
            return new HouseSO.HouseSetting(decorationRequestList.RequestedDecorations, decorationRequestList.RequestedHouseName);
        }
        return NormalHouseSO.HouseData;
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

    public HouseRequest(List<HouseCondition> houseConditions)
    {
        RequestedDecorations = new List<DecorationRequest>();
        RequestedHouseName = "-vhost house-";
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
        }
    }
}