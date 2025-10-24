using TMPro;
using UnityEngine;

public class TimerUI : BaseTextUI
{
    private Timer timer;

    protected override void Awake()
    {
        base.Awake();
        timer = FindFirstObjectByType<Timer>();
    }

    protected override string GetText()
    {
        return timer.GetTimeleft().ToString();
    }
}
