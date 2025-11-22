using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isDash = false;
    private bool canDash = true;
    [SerializeField]
    private GameObject cuadradoInteraccion;
    [SerializeField]
    private Transform groundCheck;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f);

        if (hit.collider != null && hit.collider.CompareTag("GROUND"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        // Movimiento horizontal
        if (isGrounded && !isDash)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Saltar
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Voltear personaje
        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PICKUP"))
        {
            cuadradoInteraccion.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PICKUP"))
        {
            cuadradoInteraccion.SetActive(false);
        }
    }

    // Detectar suelo mediante TAG
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GROUND"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GROUND"))
        {
            isGrounded = false;
        }
    }
    private System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDash = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // evita caída durante el dash

        // dirección según input actual (si está quieto, usa la última dirección)
        float dashDirection = transform.localScale.x > 0 ? 1 : -1;
        if (moveInput != 0) dashDirection = moveInput;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDash = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}