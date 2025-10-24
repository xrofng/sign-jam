using UnityEngine;

public class BaseUIPanel : BetterMonoBehaviour
{
    public bool IsShowing { get; protected set; }
    public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
    private RectTransform _rectTransform;

    #region Unity Lifecycle

    protected override void Update()
    {
        base.Update();
        OnFrameInitialization();
        if (IsShowing) OnUpdate();
    }
    
    private void OnValidate() => OnInspectorChanged();
    #endregion

    #region Initialization
    #endregion

    #region Panel Visibility
    public void ShowPanel()
    {
        IsShowing = true;
        OnShowingPanel();
        OnVisibleChanged(true);
    }

    public void HidePanel()
    {
        IsShowing = false;
        OnHidingPanel();
        OnVisibleChanged(false);
    }

    public void ShowPanelSilent() => IsShowing = true;
    public void HidePanelSilent() => IsShowing = false;
    public void SetShowPanel(bool isShow)
    {
        if (isShow)
        {
            ShowPanel();
        }
        else
        {
            HidePanel();
        }
    }

    protected virtual void OnShowingPanel() { }
    protected virtual void OnHidingPanel() { }
    protected virtual void OnVisibleChanged(bool currentVisibility) { }
    #endregion

    #region Extension Hooks
    protected virtual void OnFrameInitialization() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnInspectorChanged() { }
    #endregion
}
