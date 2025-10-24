using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFindingManager : MonoBehaviour
{
    List<Transform> pathFindingDot = new List<Transform>();

    [SerializeField] LayerMask StreetPathLayer;

    private void Awake()
    {
        foreach (Transform transform in this.transform)
        {
            pathFindingDot.Add(transform);
        }
    }


    public List<Transform> GetPath(Vector3 startPosition, Vector3 endPosition)
    {
        if (startPosition == endPosition)
        {
            return new List<Transform>();
        }

        if (isStreetPath(endPosition)) return new List<Transform>();


        Transform closestStartDot = pathFindingDot
            .OrderBy(i => Vector2.Distance(startPosition, i.position))
            .FirstOrDefault();

        Transform endPositionDot = pathFindingDot
            .OrderBy(i => Vector2.Distance(endPosition, i.position))
            .FirstOrDefault();



        List<Transform> copyOfPath = new List<Transform>();


        Debug.Log($"Start Dot {closestStartDot.gameObject.name}");
        copyOfPath.Add(closestStartDot);


        Debug.Log($"End Dot {endPositionDot.gameObject.name}");

        copyOfPath.Add(endPositionDot);




        return copyOfPath;
    }

    bool isStreetPath(Vector3 endPosition)
    {
        // Perform a circle cast from position with 0 direction (since we only want overlap)
        RaycastHit2D[] hits = Physics2D.CircleCastAll(endPosition, 0.1f, Vector2.zero, 0f, StreetPathLayer);

        return hits.Length != 0;
    }

}
