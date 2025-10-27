using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProceduralHouseGenerator : MonoBehaviour
{
    [Header("References")]
    public HouseData houseData;

    [Header("Margins (Local Space)")]
    [Tooltip("Distance from each edge to consider as a margin for object placement.")]
    public float topMargin = 0.3f;
    public float bottomMargin = 0.3f;
    public float leftMargin = 0.3f;
    public float rightMargin = 0.3f;

    [Header("Spacing Settings")]
    [Tooltip("Horizontal spacing between door and windows.")]
    public float horizontalSpacing = 2f;

    [Tooltip("Vertical offset from bottom margin for door placement.")]
    public float doorVerticalOffset = 0.1f;

    [Header("Parent Container")]
    public Transform partsParent;

    // Internal data
    private SpriteRenderer sr;
    private Vector2 halfSize;


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

        UpdateHalfSize();

        // === 1. Door at bottom center ===
        // Get door bottom position
        Vector3 doorPos = GetBottomCenterPosition();
        var door = houseData.GetObject(ObjectType.Door);
        if (door && door.objectPrefab)
        {
            SpawnObject(door, doorPos);
        }

        // === 2. Windows left and right ===
        var window = houseData.GetObject(ObjectType.Window);
        float doorHeight = door.objectPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        float windowY = doorPos.y + doorHeight / 2f; // center of the door
        if (window && window.objectPrefab)
        {
            Vector3 leftPos = new Vector3(doorPos.x - horizontalSpacing, windowY, 0);
            Vector3 rightPos = new Vector3(doorPos.x + horizontalSpacing, windowY, 0);

            // Bound checks
            if (leftPos.x >= -halfSize.x + leftMargin)
                SpawnObject(window, leftPos);
            if (rightPos.x <= halfSize.x - rightMargin)
                SpawnObject(window, rightPos);
        }
    }

    private void SpawnObject(HouseObjectData data, Vector3 position)
    {
        // Simply spawn at target position using prefab pivot
        var obj = Instantiate(data.objectPrefab, partsParent);
        obj.transform.localPosition = position;
    }

    private Vector3 GetBottomCenterPosition()
    {
        return new Vector3(0, -halfSize.y + bottomMargin + doorVerticalOffset, 0);
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

    // --- GIZMOS VISUALIZATION ---
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

        // Door + window hint
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pos + GetBottomCenterPosition(), 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pos + GetBottomCenterPosition() + Vector3.left * horizontalSpacing, 0.1f);
        Gizmos.DrawSphere(pos + GetBottomCenterPosition() + Vector3.right * horizontalSpacing, 0.1f);
    }
}

public static class TransformExtensions
{
    public static Transform FindChildWithTag(this Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }
}
