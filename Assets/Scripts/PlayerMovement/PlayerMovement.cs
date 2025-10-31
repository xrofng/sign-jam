using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Animator animator;
    PlayerController playerController;
    Rigidbody2D rb;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;


        Debug.Log($"move Direction : {direction}");

        if (direction == Vector2.zero)
        {
            if (playerController.isMoving == false)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                return;
            }
        }

        move(direction);


    }

    void move(Vector2 direction)
    {
        playerController.StopMove();
        rb.linearVelocity = direction * moveSpeed;
        if (direction.magnitude > 0)
        {
            animator.SetFloat("f_speed", 1);
        }
    }




}
