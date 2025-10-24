using System.Collections;
using UnityEngine;
public class InteractAction_AddingScore : InteractAction
{
    [Header("Score Setting")]
    [SerializeField] bool _isPositive;
    [SerializeField] Vector2 _scoreRange;

    [Header("Delay")]
    [SerializeField] float delayBeforeAddingScore;

    int Score;

    protected override void OnStart()
    {
        if (_isPositive)
        {
            Score = Mathf.RoundToInt(Random.Range(_scoreRange.x, _scoreRange.y));
        }
        else
        {
            Score = Mathf.RoundToInt(Random.Range(_scoreRange.x, _scoreRange.y) * -1);

        }
    }


    public override void OnDoingAction()
    {
        StartCoroutine(countDown(delayBeforeAddingScore));
    }


    IEnumerator countDown(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        Debug.Log($"Adding Score : {Score}");
    }
}
