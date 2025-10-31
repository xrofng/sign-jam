using UnityEngine;

public class PlayerSpriteOrder : MonoBehaviour
{
    [SerializeField] SpriteRenderer MainSpriteRenderer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MainSpriteRenderer.sortingOrder = (int)Mathf.Abs(transform.position.y * 100);
    }
}
