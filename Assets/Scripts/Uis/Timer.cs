using UnityEngine;
using Xrofng;

public class Timer : Automation, IEventSubcriber<Newspaper.EvsGameStateChanged>
{
    [SerializeField] CountdownClock Countdown;

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

    protected override void Initialization()
    {
        base.Initialization();
        Countdown.OnCountdownFinished += ShootEndGameEvent;
    }

    void ShootEndGameEvent()
    {
        EventBus.TriggerEvent(new Newspaper.EvsGameStateChanged(Newspaper.EGameState.End));
    }

    protected override void DoAutomation()
    {
        base.DoAutomation();
        //Countdown.StartTimer();
    }

    protected override void ProcessAutomation()
    {
        base.ProcessAutomation();
        Countdown.UpdateTimer(Time.deltaTime);
    }

    public int GetTimeleft()
    {
        return (int)Countdown.TimeLeft;
    }
    bool startCount = false;
    public void OnEventBusTrigger(Newspaper.EvsGameStateChanged eventType)
    {
        Countdown.StartTimer();
        startCount = true;
    }
    public bool isGameOver()
    {
        if (GetTimeleft() <= 0 && startCount) return true;
        return false;
    }
}
