using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xrofng;

public class WinUI : MonoBehaviour
{
    [Header("MMF to play")]
    private MMF_Player finishFeedback;

    [SerializeField] private Timer _timer;
    bool gameOver = false;

    private void Awake()
    {
        finishFeedback = GetComponent<MMF_Player>();    
    }
    bool dirty = false;    
    public void HandleCountdownFinished()
    {
        if (dirty) return;
        if (finishFeedback != null)
        {
            finishFeedback.StopFeedbacks();
            finishFeedback.PlayFeedbacks();
            dirty = true;
        }
    }

    private void Update()
    {
        if (!_timer.isGameOver()) return;
        if (_timer.isGameOver()) HandleCountdownFinished();
        
        if (Input.GetKeyUp(KeyCode.R)) ReloadCurrentScene();
    }

    public void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
