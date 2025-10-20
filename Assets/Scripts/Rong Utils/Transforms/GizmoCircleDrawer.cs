using UnityEngine;

public class GizmoCircleDrawer : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public float gizmoRadius = 1f;
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.5f); // Default: semi-transparent green

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
