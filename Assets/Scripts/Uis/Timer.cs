using UnityEngine;
using Xrofng;

public class Timer : Automation
{
    [SerializeField] CountdownClock Countdown;

    protected override void Initialization()
    {
        base.Initialization();
    }

    protected override void DoAutomation()
    {
        base.DoAutomation();
        Countdown.StartTimer();
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
}
