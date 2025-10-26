using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectWithSprite
{
    [SerializeField] MouseInput mouseInput;
    [SerializeField] PathFindingManager pathFindingManager;
    [SerializeField] float moveSpeed;

    void Start()
    {
        mouseInput.OnClickEvent += movePlayerCharacter;
    }


    bool onMove;
    void movePlayerCharacter(Vector3 mousePosition, InteractObject interactObject)
    {
        if (onMove) return;

        onMove = true;
        if (interactObject == null)
        {
            StartCoroutine(Move(mousePosition, interactObject));
            //       this.transform.position = mousePosition;
        }
        else
        {
            StartCoroutine(Move(interactObject.GetNearObjectPos(transform.position, MainSpriteRenderer.bounds), interactObject));
            //     this.transform.position = interactObject.PlayerDestination.position;
        }
    }


    IEnumerator Move(Vector3 endPos, InteractObject interactObject)
    {
        List<Transform> pathList = pathFindingManager.GetPath(transform.position, endPos);
        List<Vector3> movePos = new List<Vector3>();

        foreach (Transform t in pathList)
            movePos.Add(t.position);

        // make sure we end exactly at endPos
        movePos.Add(endPos);

        // speed of movement

        // go through each waypoint
        for (int i = 0; i < movePos.Count; i++)
        {
            Vector3 target = movePos[i];

            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        onMove = false;
        // optional: trigger interaction when finished
        if (interactObject != null)
        {
            interactObject.Interact();
        }
    }
}
