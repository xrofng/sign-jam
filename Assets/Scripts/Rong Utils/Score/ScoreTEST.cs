using UnityEngine;

public class ScoreTEST : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreController.Instance.AddScore(100);
        ScoreController.Instance.AddScore(100);
        ScoreController.Instance.AddScore(100);
    }
}
