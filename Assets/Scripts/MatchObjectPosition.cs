using UnityEngine;

public class MatchObjectPosition : MonoBehaviour
{
    [SerializeField] Transform genHouseParent;
    [SerializeField] Vector3 offSet;
    [SerializeField] float setDelay;
    [SerializeField] int matchIndex = 0;

    Transform matchedObject; public Transform MatchedObject => matchedObject;

    void Start()
    {
        Invoke(nameof(setPosition), setDelay);
    }

    void setPosition()
    {
        Vector3 matchPos = genHouseParent.transform.GetChild(0).GetChild(matchIndex).position;
        matchedObject = genHouseParent.transform.GetChild(0).GetChild(matchIndex);

        this.transform.position = matchPos + offSet;

    }


}
