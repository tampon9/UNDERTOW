using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.2f;
    public SpriteRenderer spriteRenderer;

    [Tooltip("Сюда перетащить Animator персонажа — управляет параметром Speed")]
    public Animator animator;

    [Tooltip("Не обязательно: если задано, движение блокируется, пока персонаж рыбачит")]
    public FishingManager fishingManager;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool locked = (GameFlowManager.Instance != null && GameFlowManager.Instance.inputLocked)
                      || (fishingManager != null && fishingManager.State != FishingState.Idle);

        if (locked)
        {
            moveInput = Vector2.zero;
            animator?.SetFloat("Speed", 0f);
            return;
        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        if (Mathf.Abs(moveInput.x) > 0.01f && spriteRenderer != null)
            spriteRenderer.flipX = moveInput.x < 0f;

        animator?.SetFloat("Speed", moveInput.magnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}