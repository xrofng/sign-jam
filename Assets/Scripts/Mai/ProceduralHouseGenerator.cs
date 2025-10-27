using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class ProceduralHouseGenerator : MonoBehaviour
{
    [Header("References")]
    public HouseData houseData;

    [Tooltip("List of ScriptableObject rules that define the placement logic.")]
    public List<ProceduralRule> proceduralRules = new List<ProceduralRule>();

    [Header("Margins (Local Space)")]
    [Tooltip("Distance from each edge to consider as a margin for object placement.")]
    public float topMargin = 0.3f;
    public float bottomMargin = 0.3f;
    public float leftMargin = 0.3f;
    public float rightMargin = 0.3f;

    [Header("Parent Container")]
    public Transform partsParent;

    // Internal data
    private SpriteRenderer sr;
    private Vector2 halfSize;

    // The "blackboard" for rules to share data
    private Dictionary<string, Bounds> _placedObjectBounds = new Dictionary<string, Bounds>();

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            GenerateHouse();
        }
    }

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
                // Pass the SpawnObject method, which now matches the Func signature
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

    private void ClearAndSetupParent()
    {
        // Clear existing
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
        // Simply spawn at target position using prefab pivot
        var obj = Instantiate(data.objectPrefab, partsParent);
        obj.transform.localPosition = position;
        return obj; // Return the new instance
    }

    private void UpdateHalfSize()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        if (sr.sprite)
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

    #region
    // Gizmos for visualization
    private void OnDrawGizmos()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        UpdateHalfSize();

        Vector3 pos = transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(pos, new Vector3(halfSize.x * 2, halfSize.y * 2, 0.05f));

        // Margins
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

        // (Note: To show rule-based hints in Gizmos, the rules would need a separate DrawGizmos method)
    }
    #endregion
}