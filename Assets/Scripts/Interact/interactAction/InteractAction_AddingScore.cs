using System.Collections;
using UnityEngine;
public class InteractAction_AddingScore : InteractAction
{
    [Header("Score Setting")]
    bool _isGhostHouse;
    [SerializeField] Vector2 _positiveScoreRange;
    [SerializeField] Vector2 _negativeScoreRange;

    [Header("Delay")]
    [SerializeField] float delayBeforeAddingScore;

    int Score;



    private void Awake()
    {

        this.transform.parent.parent.TryGetComponent(out House house);

        house.OnSetGhost += SetScore;
    }

    public void SetScore(bool isGhostHouse)
    {
        if (isGhostHouse)
        {
            Score = -Mathf.RoundToInt(Random.Range(_negativeScoreRange.x, _negativeScoreRange.y));
        }
        else
        {
            Score = Mathf.RoundToInt(Random.Range(_positiveScoreRange.x, _positiveScoreRange.y));
        }
    }


    protected override void OnDoingAction()
    {
        StartCoroutine(countDown(delayBeforeAddingScore));
    }


    IEnumerator countDown(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        Debug.Log($"Adding Score : {Score}");
        ScoreController.Instance.AddScore(Score);
    }
}
