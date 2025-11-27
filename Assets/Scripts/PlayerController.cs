using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isOnWall = false;
    private int wallDirection = 0;

    [SerializeField] public GameObject fin;

    Animator animator;

    [Header("Wall Jump")]
    public float wallJumpForce = 10f;
    public float wallJumpHorizontalForce = 8f;
    public float wallJumpDisableTime = 0.15f;

    [Header("Jump")]
    private float coyoteTimeCounter = 0;
    private float coyoteTime = 0.2f;
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
    float dashDirection = 0f;

    int counter = 0;

    // VARIABLES PARA ESCALERA DE MANO
    private float vertical;
    private bool isClimbing = false;
    private bool isLadder = false;
    private int counterMusicNoReply = 0;
    private int moveDirection = 0;

    private bool wallDetectionEnabled = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.instance.Playlvl1();
        // Asegurarnos de que facingRight refleja el scale actual
        facingRight = transform.localScale.x > 0f;
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        // Movimiento horizontal
        if (isGrounded && !isDash)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            if(moveInput > 0)
            {
                moveDirection = 1;
            }
            else if(moveInput < 0)
            {
                moveDirection = -1;
            }
            else
            {
                counter++;
                if(counter > 9)
                {
                    counter = 0;
                    moveDirection = 0;
                }
            }
            animator.SetInteger("moveX", moveDirection);
        }
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            canDash = true;
            coyoteTimeCounter -= Time.deltaTime;
        }
        vertical = Input.GetAxis("Vertical");
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f);
        if (!isGrounded && isOnWall && Input.GetButtonDown("Jump") && GameManager.instance.playerWallJump && wallDetectionEnabled)
        {
            WallJump();
        }
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            animator.SetBool("climbing", true);
            isClimbing = true;
        }
        else if (!isLadder)
        {
            animator.SetBool("climbing", false);
            isClimbing = false;
        }

        

        if (hit.collider != null && hit.collider.CompareTag("GROUND"))
        {

            animator.SetBool("jumping", false);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("jumping", true);
            isGrounded = false;
        }
        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && GameManager.instance.playerHaveDash)
        {
            StartCoroutine(Dash());
        }
        // Voltear personaje seg�n input horizontal (s�lo si no est� bloqueado por otras cosas)
        if (!facingRight && moveInput > 0 && isGrounded)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0 && isGrounded)
        {
            Flip();
        }   
        // Saltar (s�lo desde suelo)
        if (coyoteTimeCounter > 0 && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
            if (collision.CompareTag("LADDER"))
            {
                LadderScript script = collision.GetComponent<LadderScript>();
                if (script != null)
                {
                    script.DesactivarColiderExit();
                }
                isLadder = true;
            }
        }
        if (collision.CompareTag("FIN"))
        {
            fin.SetActive(true);
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

        

        if(collision.gameObject.CompareTag("MUSICLVL2"))
        {
            
            GameManager.instance.StopMusic();
            GameManager.instance.Playlvl2();
        }
        if(collision.gameObject.CompareTag("MUSICLVL3") && counterMusicNoReply == 0)
        {
            counterMusicNoReply++;
            GameManager.instance.StopMusic();
            GameManager.instance.Playlvl3();
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
        if (collision.gameObject.CompareTag("LADDER"))
        {
            isLadder = false;
            isClimbing = false;
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!wallDetectionEnabled) return;

        if (other.gameObject.CompareTag("WALL"))
        {
            float sumX = 0f;
            int count = 0;
            foreach (ContactPoint2D c in other.contacts)
            {
                sumX += c.point.x;
                count++;
            }

            if (count > 0)
            {
                float avgContactX = sumX / count;
                if (avgContactX > transform.position.x)
                {
                    wallDirection = 1;
                }
                else
                {
                    wallDirection = -1;
                }

                isOnWall = true;
            }
            if (other.gameObject.CompareTag("GROUND"))
            {
                isGrounded = true;
            }
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
        animator.SetBool("dashing", true);
        isDash = true;

        rb.linearVelocity = Vector2.zero;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        dashDirection = transform.localScale.x > 0 ? 1 : -1;
        if (moveInput != 0 && isGrounded) dashDirection = moveInput;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        animator.SetBool("dashing", false);
        isDash = false;

        yield return new WaitForSeconds(dashCooldown);
    }

    private void WallJump()
    {
        rb.linearVelocity = Vector2.zero;
        Flip();
        int jumpDir = -wallDirection;
        dashDirection = jumpDir;
        rb.AddForce(new Vector2(jumpDir * wallJumpHorizontalForce, wallJumpForce), ForceMode2D.Impulse);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        Debug.Log("Flip: " + facingRight);
    }
}