using UnityEngine;
using MoreMountains.Tools;

public class ScoreController : MMSingleton<ScoreController>
{
    private int _score;

    public int Score => _score;

    public virtual void AddScore(int increment)
    {
        _score += increment;
        EventBus.TriggerEvent(new EvsScoreChanged(_score, increment));
    }
}

public struct EvsScoreChanged
{
    public int NewScore;
    public int ScoreIncrement;

    public EvsScoreChanged(int newScore, int scoreIncrement)
    {
        NewScore = newScore;
        ScoreIncrement = scoreIncrement;
    }
}
