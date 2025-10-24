using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MouseInput mouseInput;

    void Start()
    {
        mouseInput.OnClickEvent += movePlayerCharacter;
    }

    void movePlayerCharacter(Vector3 mousePosition, InteractObject interactObject)
    {
        if (interactObject == null)
        {
            this.transform.position = mousePosition;
        }
        else
        {
            this.transform.position = interactObject.PlayerDestination.position;
            interactObject.Interact();
        }
    }
}
