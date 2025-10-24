using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public delegate void OnClickDelegate(Vector3 mousePosition, InteractObject _foundedInteractObject);

    public event OnClickDelegate OnClickEvent;


    [SerializeField] private float detectRadius = 1.5f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private LayerMask moveLayer;
    Vector3 getMousePositon()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert to world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Set Z to 0 for 2D games (since camera depth affects conversion)
        mouseWorldPosition.z = 0f;

        return mouseWorldPosition;
    }
    InteractObject previosInteractObject;
    void Update()
    {
        Vector3 mousePosition = getMousePositon();
        setUpInteractObject(mousePosition);
        inputHolder(mousePosition);
    }


    #region setUp interact Object
    void setUpInteractObject(Vector3 mousePosition)
    {
        InteractObject _foundInteractObject = FindInteractObject(mousePosition);

        if (_foundInteractObject != previosInteractObject)
        {
            previosInteractObject?.OnDeselect();
            _foundInteractObject?.OnSelect();
        }

        previosInteractObject = _foundInteractObject;
    }
    InteractObject FindInteractObject(Vector3 position)
    {
        // Perform a circle cast from position with 0 direction (since we only want overlap)
        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, detectRadius, Vector2.zero, 0f, interactLayer);

        foreach (RaycastHit2D hit in hits)
        {
            InteractObject interactObj = hit.collider.GetComponent<InteractObject>();
            if (interactObj != null)
            {
                return interactObj;
            }
        }

        // If nothing found, return null
        return null;
    }
    #endregion



    bool CanMove(Vector3 position)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, detectRadius, Vector2.zero, 0f, moveLayer);

        foreach (RaycastHit2D hit in hits)
        {

            return true;

        }

        // If nothing found, return null
        return false;
    }

    void inputHolder(Vector3 mousePosition)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanMove(mousePosition) || previosInteractObject != null)
            {
                OnClickEvent?.Invoke(mousePosition, previosInteractObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectRadius);
    }

}
