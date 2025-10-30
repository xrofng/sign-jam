using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectWithSprite
{
    [SerializeField] MouseInput mouseInput;
    [SerializeField] float moveSpeed;
    [SerializeField] PathFindingManager pathFindingManager;
    [SerializeField] bool useSemiRealPathfinding = false;


    public bool CanMove = true;
    public bool isMoving { get; private set; }
    private List<Vector3> movePos;

    protected override void Start()
    {
        base.Start();
        mouseInput.OnClickEvent += movePlayerCharacter;
    }
    public void StopMove()
    {
        if (isMoving)
        {
            StopAllCoroutines();
            isMoving = false;
        }
    }


    void movePlayerCharacter(Vector3 mousePosition, InteractObject interactObject)
    {
        if (CanMove == false) return;

        if (isMoving)
        {
            StopAllCoroutines();
            isMoving = false;
        }

        isMoving = true;
        if (interactObject == null)
        {
            StartCoroutine(Move(mousePosition, null));
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
        if (MainSpriteRenderer.transform.position.x > endPos.x)
        {
            MainSpriteRenderer.flipX = false;
        }
        else
        {
            MainSpriteRenderer.flipX = true;

        }


        List<Transform> pathList = pathFindingManager.GetPath(transform.position, endPos);
        movePos = new List<Vector3>();

        if (useSemiRealPathfinding)
        {
            foreach (Transform t in pathList)
                movePos.Add(t.position);
        }


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
        isMoving = false;
        // optional: trigger interaction when finished
        if (interactObject != null)
        {
            interactObject.Interact();
        }
    }
}
