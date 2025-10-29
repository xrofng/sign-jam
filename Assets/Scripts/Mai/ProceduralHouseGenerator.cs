using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class ProceduralHouseGenerator : MonoBehaviour
{
    [Header("References")]
    public HouseData houseData;

    [Tooltip("List of ScriptableObject rules that define the placement logic.")]
    public List<ProceduralRule> proceduralRules = new List<ProceduralRule>();

    [Header("Debug Size Override")]
    [Tooltip("Enable this to manually set width and height instead of using sprite bounds.")]
    public bool useCustomDimensions = false;

    [Tooltip("Custom width of the house area (only used if useCustomDimensions is true).")]
    public float customWidth = 5f;

    [Tooltip("Custom height of the house area (only used if useCustomDimensions is true).")]
    public float customHeight = 5f;

    [Header("Margins (Local Space)")]
    [Tooltip("Distance from each edge to consider as a margin for object placement.")]
    public float topMargin = 0.3f;
    public float bottomMargin = 0.3f;
    public float leftMargin = 0.3f;
    public float rightMargin = 0.3f;

    [Header("Parent Container")]
    public Transform partsParent;

    private SpriteRenderer sr;
    private Vector2 halfSize;
    private Dictionary<string, Bounds> _placedObjectBounds = new Dictionary<string, Bounds>();

    private float lastWidth;
    private float lastHeight;
    private bool dimensionsChanged = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            GenerateHouse();
            CacheDimensions();
        }
    }

    private void Update()
    {
        if (Application.isPlaying && useCustomDimensions)
        {
            if (customWidth != lastWidth || customHeight != lastHeight)
            {
                dimensionsChanged = true;
                lastWidth = customWidth;
                lastHeight = customHeight;
            }

            if (dimensionsChanged)
            {
                GenerateHouse();
                dimensionsChanged = false;
            }
        }
    }

    private void CacheDimensions()
    {
        lastWidth = customWidth;
        lastHeight = customHeight;
    }

    [ContextMenu("Regenerate House")]
    public void GenerateHouse()
    {
        if (!houseData)
        {
            Debug.LogWarning($"No HouseData assigned on {name}");
            return;
        }

        ClearAndSetupParent();
        UpdateHalfSize();
        _placedObjectBounds.Clear();

        foreach (var rule in proceduralRules)
        {
            if (rule != null)
            {
                rule.Execute(this, halfSize, SpawnObject);
            }
        }
    }

    public void StorePlacedObjectBounds(string key, Bounds bounds)
    {
        _placedObjectBounds[key] = bounds;
    }

    public Bounds? GetPlacedObjectBounds(string key)
    {
        if (_placedObjectBounds.TryGetValue(key, out Bounds bounds))
        {
            return bounds;
        }
        return null;
    }

    public bool RemovePlacedObjectBounds(string key)
    {
        return _placedObjectBounds.Remove(key);
    }

    private void ClearAndSetupParent()
    {
        if (partsParent == null)
        {
            var existing = transform.Find("GeneratedParts");
            partsParent = existing ? existing : new GameObject("GeneratedParts").transform;
            partsParent.SetParent(transform);
            partsParent.localPosition = Vector3.zero;
        }

        for (int i = partsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(partsParent.GetChild(i).gameObject);
        }
    }

    private GameObject SpawnObject(HouseObjectData data, Vector3 position)
    {
        var obj = Instantiate(data.objectPrefab, partsParent);
        obj.transform.localPosition = position;
        return obj;
    }

    private void UpdateHalfSize()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();

        if (useCustomDimensions)
        {
            halfSize = new Vector2(customWidth * 0.5f, customHeight * 0.5f);
        }
        else if (sr.sprite)
        {
            halfSize = sr.sprite.bounds.extents;
            halfSize.x *= transform.localScale.x;
            halfSize.y *= transform.localScale.y;
        }
        else
        {
            halfSize = Vector2.one;
        }
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        UpdateHalfSize();

        Vector3 pos = transform.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(pos, new Vector3(halfSize.x * 2, halfSize.y * 2, 0.05f));

        Gizmos.color = new Color(1f, 0.8f, 0f, 0.5f);
        Vector3 topLeft = pos + new Vector3(-halfSize.x + leftMargin, halfSize.y - topMargin, 0);
        Vector3 bottomRight = pos + new Vector3(halfSize.x - rightMargin, -halfSize.y + bottomMargin, 0);
        Vector3 marginCenter = (topLeft + bottomRight) * 0.5f;
        Vector3 marginSize = new Vector3(
            (halfSize.x * 2) - (leftMargin + rightMargin),
            (halfSize.y * 2) - (topMargin + bottomMargin),
            0.05f
        );
        Gizmos.DrawWireCube(marginCenter, marginSize);
    }
    #endregion
}
