using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Works like a CanvasGroup, but for SpriteRenderers.
/// Controls opacity of all child SpriteRenderers, and optionally tint.
/// </summary>
[ExecuteAlways]
public class SpriteGroup : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float alpha = 1f;
    [SerializeField] private bool applyTint = false;
    [SerializeField] private Color tintColor = Color.white;

    private List<SpriteRenderer> _renderers = new();

    public float Alpha
    {
        get => alpha;
        set
        {
            alpha = Mathf.Clamp01(value);
            ApplyProperties();
        }
    }

    public bool ApplyTint
    {
        get => applyTint;
        set
        {
            applyTint = value;
            ApplyProperties();
        }
    }

    public Color TintColor
    {
        get => tintColor;
        set
        {
            tintColor = value;
            ApplyProperties();
        }
    }

    private void Awake()
    {
        CacheRenderers();
        ApplyProperties();
    }

    private void OnValidate()
    {
        CacheRenderers();
        ApplyProperties();
    }

    private void CacheRenderers()
    {
        _renderers.Clear();
        GetComponentsInChildren(true, _renderers);
    }

    public void ApplyProperties()
    {
        foreach (var r in _renderers)
        {
            if (r == null) continue;

            var c = r.color;
            c.a = alpha;
            r.color = applyTint ? new Color(tintColor.r, tintColor.g, tintColor.b, alpha) : c;
        }
    }
}
