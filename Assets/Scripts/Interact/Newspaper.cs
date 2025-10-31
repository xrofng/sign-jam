using UnityEngine;

public class Newspaper : InteractObject, IEventSubcriber<Newspaper.EvsGameStateChanged>
{
    [SerializeField] private SimpleMMSoundPlayer IntroBGM;
    [SerializeField] private SimpleMMSoundPlayer GameBGM;
    private bool _triggered;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe<Newspaper.EvsGameStateChanged>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe<Newspaper.EvsGameStateChanged>(this);
    }

    public override void Interact()
    {
        base.Interact();
        if (_triggered)
        {
            return;
        }
        IntroBGM.StopClip();
        GameBGM.PlayClip();
        EventBus.TriggerEvent(new EvsGameStateChanged(EGameState.Game));
    }

    public void OnEventBusTrigger(EvsGameStateChanged eventType)
    {
        _triggered = true;
    }

    public enum EGameState
    {
        Intro,
        Game,
        End
    }

    public struct EvsGameStateChanged
    {
        public EGameState GameState;

        public EvsGameStateChanged(EGameState gameState)
        {
            GameState = gameState;
        }
    }
}
