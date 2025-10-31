using UnityEngine;

public class XPositionForGameStart : BetterMonoBehaviour, IEventSubcriber<Newspaper.EvsGameStateChanged>
{
    [SerializeField] private PlayerMovement Player;
    [SerializeField] private SimpleMMSoundPlayer IntroBGM;
    [SerializeField] private SimpleMMSoundPlayer GameBGM;
    private bool _triggered;

    public void OnEventBusTrigger(Newspaper.EvsGameStateChanged eventType)
    {
        _triggered = true;
    }

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

    protected override void Update()
    {
        if (_triggered)
        {
            return;
        }

        if (Player.transform.position.x > 1 && _triggered == false)
        {
            _triggered = true;
            EventBus.TriggerEvent(new Newspaper.EvsGameStateChanged(Newspaper.EGameState.Game));
        }
    }
}
