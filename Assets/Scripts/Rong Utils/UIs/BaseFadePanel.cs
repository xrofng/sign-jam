using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BaseFadePanel : BaseUIPanel
{
    [SerializeField] private float FadeDuration = 0.3f;
    [SerializeField] bool ShowPanelOnStart = true;

    private CanvasGroup _canvasGroup;
    private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();


    private Coroutine fadeCoroutine;

    protected override void Start()
    {
        base.Start();
        if (ShowPanelOnStart)
        {
            ShowPanel();
        }
    }

    protected override void OnShowingPanel()
    {
        IsShowing = true;
        FadeToAlpha(1f);
    }

    protected override void OnHidingPanel()
    {
        IsShowing = false;
        FadeToAlpha(0f);
    }

    private void FadeToAlpha(float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCanvas(targetAlpha));
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = CanvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < FadeDuration)
        {
            CanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / FadeDuration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        CanvasGroup.alpha = targetAlpha;
        CanvasGroup.interactable = targetAlpha > 0.95f;
        CanvasGroup.blocksRaycasts = targetAlpha > 0.95f;
    }
}
