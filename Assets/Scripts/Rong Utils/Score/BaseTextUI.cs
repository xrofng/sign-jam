using TMPro;
using UnityEngine;

public class BaseTextUI : BaseFadePanel
{
    [SerializeField] TextMeshProUGUI TextMesh;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TextMesh.text = GetText();
    }

    protected virtual string GetText()
    {
        return TextMesh.text;
    }
}