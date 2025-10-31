using UnityEngine;

public class MatchObjectPosition : BetterMonoBehaviour
{
    [SerializeField] Transform genHouseParent;
    [SerializeField] Vector3 offSet;
    [SerializeField] float setDelay;
    [SerializeField] int matchIndex = 0;

    Transform matchedObject; public Transform MatchedObject => matchedObject;


    public event System.Action<Transform> OnSetMatchObj;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(setPosition), setDelay);
    }

    void setPosition()
    {
        Vector3 matchPos = genHouseParent.transform.GetChild(0).GetChild(matchIndex).position;
        matchedObject = genHouseParent.transform.GetChild(0).GetChild(matchIndex);
        OnSetMatchObj?.Invoke(matchedObject);
        this.transform.position = matchPos + offSet;

    }


}
