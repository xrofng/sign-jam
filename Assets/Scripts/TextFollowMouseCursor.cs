using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class TextFollowMouseCursor : MMSingleton<TextFollowMouseCursor>
{
    [SerializeField] TextMeshPro tmp_Text;
    [SerializeField] Vector3 _offSet;

    void Update()
    {
        this.transform.position = getMousePositon() + _offSet;
    }

    public void SetUpText(string text)
    {
        tmp_Text.text = text;
    }

    Vector3 getMousePositon()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert to world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Set Z to 0 for 2D games (since camera depth affects conversion)
        mouseWorldPosition.z = 0f;

        return mouseWorldPosition;
    }
}
