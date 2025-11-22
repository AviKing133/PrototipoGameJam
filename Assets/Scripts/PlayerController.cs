using UnityEngine;
using UnityEngine.U2D.IK;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    private bool isOnWall = false;
    private int wallDirection = 0; // -1 si la pared está a la izquierda, 1 si está a la derecha

    [Header("Wall Jump")]
    public float wallJumpForce = 10f;
    public float wallJumpHorizontalForce = 8f;
    [Header("Jump")]
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

    // VARIABLES PARA ESCALERA DE MANO
    private float vertical;
    private bool isClimbing = false;
    private bool isLadder = false;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f);
        if (!isGrounded && isOnWall && Input.GetButtonDown("Jump") && GameManager.instance.playerWallJump)
        {
            WallJump();
        }
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
        else if(!isLadder)
        {
            isClimbing = false;
        }

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
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && GameManager.instance.playerHaveDash)
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

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * moveSpeed);
        }
        if (!isClimbing && rb.gravityScale == 0f)
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LADDER"))
        {
            if(collision.CompareTag("LADDER"))
            {
                LadderScript script = collision.GetComponent<LadderScript>();
                if (script != null)
                {
                    script.DesactivarColiderExit();
                }
                isLadder = true;
            }
        }
        
        if (collision.CompareTag("LADDEREXIT"))
        {
            LadderScript script = collision.GetComponentInParent<LadderScript>();
            if (script != null)
            {
                script.ActivarColiderExit();
            }
            isClimbing = false;
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PICKUP"))
        {
            cuadradoInteraccion.SetActive(true);
        }
        if (collision.gameObject.CompareTag("LADDEREXIT") && Input.GetKeyDown(KeyCode.S))
        {
            LadderScript script = collision.GetComponentInParent<LadderScript>();
            if (script != null)
            {
                script.DesactivarColiderExit();
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PICKUP"))
        {
            cuadradoInteraccion.SetActive(false);
        }
        if(collision.gameObject.CompareTag("LADDER"))
        {
            isLadder = false;
            isClimbing = false;
            rb.gravityScale = 1f; 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        }
    }

    // Detectar suelo mediante TAG
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            isOnWall = true;

            if (other.transform.position.x > transform.position.x)
                wallDirection = 1;
            else
                wallDirection = -1;
        }
        if (other.gameObject.CompareTag("GROUND"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            isOnWall = false;
        }

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
    private void WallJump()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(-wallDirection * wallJumpHorizontalForce, wallJumpForce), ForceMode2D.Impulse);
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

}