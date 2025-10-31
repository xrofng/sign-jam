using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class SetHouseOwner_Sprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer houseOwner;
    [SerializeField] MMF_Player MMF;
    MMF_Sound _screamSoundPlayer;
    MMF_Sound _impactSoundPlayer;

    [SerializeField] List<Sprite> _ghostHouseOwnerList;
    [SerializeField] List<Sprite> _normalHouseOwner;

    [SerializeField] AudioClip FriendlyScreamClip;
    [SerializeField] AudioClip ScaryScreamClip;

    public AudioClip FriendlyJingleClip;
    public AudioClip ScaryViolClip;

    private void Awake()
    {
        this.transform.parent.parent.TryGetComponent(out House house);

        house.OnSetGhost += setSprite;

        foreach(MMF_Sound mMF_Sound in MMF.GetFeedbacksOfType<MMF_Sound>())
        {
            if (mMF_Sound.GetLabel().Contains("Varied Scream"))
            {
                _screamSoundPlayer = mMF_Sound;
            }
            if (mMF_Sound.GetLabel().Contains("Varied Impact"))
            {
                _impactSoundPlayer = mMF_Sound;
            }
        }
        house.OnSetGhost += SetSound;
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
    void SetSound(bool isGhost)
    {
        if (isGhost)
        {
            _screamSoundPlayer.Sfx = ScaryScreamClip;
            _impactSoundPlayer.Sfx = ScaryViolClip;
        }
        else
        {
            _screamSoundPlayer.Sfx = FriendlyScreamClip;
            _impactSoundPlayer.Sfx = FriendlyJingleClip;

        }
    }
}
