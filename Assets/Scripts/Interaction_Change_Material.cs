using UnityEngine;
public class Interaction_Change_Material : InteractAction
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    [SerializeField] Material NormalMat;
    [SerializeField] Material SelectMat;


    private void Awake()
    {
        if (this.transform.parent.TryGetComponent(out MatchObjectPosition matchObj))
        {
            matchObj.OnSetMatchObj += SetmatchObj;
        }
    }

    void SetmatchObj(Transform matchObj)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = matchObj.gameObject.GetComponent<SpriteRenderer>();

    }



    protected override void OnDeselect()
    {
        Debug.Log("Change mat to Normal");
        _spriteRenderer.material = NormalMat;
    }

    protected override void OnDoingAction()
    {


    }

    protected override void OnSelect()
    {
        Debug.Log("Change mat to Select");

        _spriteRenderer.material = SelectMat;
    }


}
