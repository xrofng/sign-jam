using UnityEngine;

public class BaseUIPanel : MonoBehaviour
{
    public bool IsShowing { get; protected set; }
    public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
    private RectTransform _rectTransform;

    #region Unity Lifecycle
    private void Start() => Initialization();
    private void Update()
    {
        OnFrameInitialization();
        if (IsShowing) ProcessPanel();
    }
    private void OnEnable() => OnPanelEnable();
    private void OnDisable() => OnPanelDisable();
    private void OnValidate() => OnInspectorChanged();
    #endregion

    #region Initialization
    protected virtual void Initialization() { }
    protected virtual void OnFrameInitialization() { }
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
    protected virtual void OnPanelEnable() { }
    protected virtual void OnPanelDisable() { }
    protected virtual void ProcessPanel() { }
    protected virtual void OnInspectorChanged() { }
    #endregion
}
