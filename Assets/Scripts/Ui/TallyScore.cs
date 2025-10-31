using MoreMountains.Tools;
using System.Collections;
using TMPro;
using UnityEngine;

public class TallyScore : MonoBehaviour
{

    [Header("UI")]
    public TMP_Text scoreText; // Assign your TMP_Text

    [Header("Tick Speed Curve")]
    [Tooltip("X = progress (0 to 1), Y = tick speed (ticks per second). Higher Y = faster.")]
    public AnimationCurve tickSpeedCurve = AnimationCurve.EaseInOut(0f, 5f, 1f, 30f);

    [Header("Audio (optional)")]
    public AudioSource audioSource;
    public AudioClip tickClip;
    public float basePitch = 1f;
    public float pitchPerStep = 0.07f;
    public float maxPitch = 2f;
    public float resetPitchTime = 0.3f;

    private Coroutine tallyCo;

    /// <summary>
    /// Call this once at the end of the game.
    /// </summary>
    public void PlayFinalTally()
    {
        if (tallyCo != null) StopCoroutine(tallyCo);
        tallyCo = StartCoroutine(TallyRoutine());
    }

    private IEnumerator TallyRoutine()
    {
        int target = ScoreController.Instance.Score;
        int display = 0;
        float pitchSteps = 0f;

        SetPitch(basePitch);
        Write(display);

        if (target <= 0)
        {
            yield return ResetPitchCo();
            yield break;
        }

        float tickAccumulator = 0f;

        while (display < target)
        {
            // X axis = progress, Y = ticks per second
            float progress = (float)display / target;
            float tickSpeed = tickSpeedCurve.Evaluate(progress); // directly used

            // Accumulate time * speed
            tickAccumulator += Time.deltaTime * tickSpeed;

            // Emit ticks
            while (tickAccumulator >= 1f && display < target)
            {
                tickAccumulator -= 1f;
                display++;
                Write(display);

                pitchSteps++;
                float p = Mathf.Clamp(basePitch + pitchSteps * pitchPerStep, 0.1f, maxPitch);
                SetPitch(p);
                PlayTick();
            }

            yield return null;
        }

        yield return ResetPitchCo();
    }

    private void Write(int v)
    {
        if (scoreText) scoreText.text = v.ToString();
    }

    private void PlayTick()
    {
        if (!audioSource) return;
        if (tickClip)
            audioSource.PlayOneShot(tickClip);
        else if (audioSource.clip)
            audioSource.Play();
    }

    private void SetPitch(float p)
    {
        if (audioSource) audioSource.pitch = p;
    }

    private IEnumerator ResetPitchCo()
    {
        if (!audioSource) yield break;
        float from = audioSource.pitch;
        float t = 0f;

        while (t < resetPitchTime)
        {
            t += Time.deltaTime;
            SetPitch(Mathf.Lerp(from, basePitch, t / resetPitchTime));
            yield return null;
        }

        SetPitch(basePitch);
    }

}

