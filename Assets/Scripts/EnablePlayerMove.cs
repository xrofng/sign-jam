using UnityEngine;

public class EnablePlayerMove : MonoBehaviour
{
    PlayerController controller;
    private void Awake()
    {
        controller = FindAnyObjectByType<PlayerController>();
    }

    public void EnableMove()
    {
        controller.CanMove = true;
    }

    public void DisableMove()
    {
        controller.CanMove = false;

    }
}
