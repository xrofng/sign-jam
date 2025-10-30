using System.Collections.Generic;
using UnityEngine;

public class SetHouseOwner_Sprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer houseOwner;

    [SerializeField] List<Sprite> _ghostHouseOwnerList;
    [SerializeField] List<Sprite> _normalHouseOwner;


    private void Awake()
    {
        this.transform.parent.parent.TryGetComponent(out House house);

        house.OnSetGhost += setSprite;
    }

    void setSprite(bool isGhost)
    {
        if (isGhost)
        {
            houseOwner.sprite = _ghostHouseOwnerList[Random.Range(0, _ghostHouseOwnerList.Count)];
        }
        else
        {
            houseOwner.sprite = _normalHouseOwner[Random.Range(0, _normalHouseOwner.Count)];

        }
    }
}
