using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public Character Character;

    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float minY = -2f, maxY = 2f;

    Rigidbody2D rb;
    Animator animator;
    Vector2 input;
    float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Character != null && Character.isGameOver) return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        bool shift = Input.GetKey(KeyCode.LeftShift);

        currentSpeed = shift ? runSpeed : walkSpeed;
        input = input.normalized * currentSpeed;

        bool isMoving = input.magnitude > 0f;
        bool isRunning = shift && isMoving;
        bool isWalking = !shift && isMoving;

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsWalking", isWalking);

        if (input.x > 0)
            transform.localScale = new Vector3(3, 3, 1);
        else if (input.x < 0)
            transform.localScale = new Vector3(-3, 3, 1);
    }

    void FixedUpdate()
    {
        if (Character != null && Character.isGameOver) return;

        Vector2 newPos = rb.position + input * Time.fixedDeltaTime;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }
}
