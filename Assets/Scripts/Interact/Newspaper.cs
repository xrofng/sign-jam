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
        
        EventBus.TriggerEvent(new EvsGameStateChanged(EGameState.Game));
    }

    public void OnEventBusTrigger(EvsGameStateChanged eventType)
    {
        if (eventType.GameState == EGameState.Game && _triggered == false)
        {
            _triggered = true;
            IntroBGM.StopClip();
            GameBGM.PlayClip();
        }
        else if(eventType.GameState == EGameState.End)
        {
            GameBGM.StopClip();
        }
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
