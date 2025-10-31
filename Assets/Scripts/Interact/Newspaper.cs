using UnityEngine;

public class Newspaper : InteractObject
{
    [SerializeField] private SimpleMMSoundPlayer IntroBGM;
    [SerializeField] private SimpleMMSoundPlayer GameBGM;

    public override void Interact()
    {
        base.Interact();
        IntroBGM.StopClip();
        GameBGM.PlayClip();
        EventBus.TriggerEvent(new EvsGameStateChanged(EGameState.Game));
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
