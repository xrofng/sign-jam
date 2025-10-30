using UnityEngine;

public class WarpMatchedObject : MonoBehaviour
{
    [SerializeField] Vector3 offSet;
    MatchObjectPosition matchObjectPosition;

    private void Awake()
    {
        this.transform.parent.TryGetComponent(out matchObjectPosition);
    }

    public void WarpNow()
    {
        matchObjectPosition.MatchedObject.GetComponent<SpriteRenderer>().flipX = true;
        matchObjectPosition.MatchedObject.transform.position = matchObjectPosition.MatchedObject.transform.position + offSet;
    }

    public void BackToNormal()
    {
        matchObjectPosition.MatchedObject.GetComponent<SpriteRenderer>().flipX = false;
        matchObjectPosition.MatchedObject.transform.position = matchObjectPosition.MatchedObject.transform.position - offSet;
    }
}
