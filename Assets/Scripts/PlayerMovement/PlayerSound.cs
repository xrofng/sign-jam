using UnityEngine;

public class PlayerSound : BetterMonoBehaviour, IEventSubcriber<EvsDialogueAction>
{
    [SerializeField] AudioClip[] Clips;
    [SerializeField] SimpleMMSoundPlayer SimpleMMSoundPlayer;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(EvsDialogueAction eventType)
    {
        SimpleMMSoundPlayer.PlayClip(RandomSoundClip());
    }

    private AudioClip RandomSoundClip()
    {
        return Clips[Random.Range(0, Clips.Length)];
    }
}
