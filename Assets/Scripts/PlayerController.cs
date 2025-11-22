using UnityEngine;
using UnityEngine.U2D.IK;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    // VARIABLES PARA ESCALERA DE MANO
    private bool isUnderPlatform = false,
    isCloseToLadder = false,
    climbHeld = false, 
    hasStartedClimb = false;
    private Transform ladder;
    private float vertical = 0f;
    private float climbSpeed = 0.2f;

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

        // LADDER MOVEMENT
        /*
        vertical = Input.GetAxisRaw("Vertical") * climbSpeed;
        if (isOnGround() && horizontal.Equals(0) && !isCloseToLadder && (crouchHeld || isUnderPlatform))
            GetComponent<Animator>().Play("CharacterCrouchIdle");
        else if (isOnGround() && !isCloseToLadder && (horizontal > 0 || horizontal < 0) && (crouchHeld || isUnderPlatform))
            GetComponent<Animator>().Play("CharacterCrouch");
        else if (isOnGround() && !hasStartedClimb && horizontal.Equals(0))
            GetComponent<Animator>().Play("CharacterIdle");
        else if (isOnGround() && !hasStartedClimb && (horizontal > 0 || horizontal < 0))
            GetComponent<Animator>().Play("CharacterWalk");
        */
        // Evalua una condicio i li assigna true si és verdader, false si és fals
        climbHeld = (isCloseToLadder && Input.GetButton("Climb")) ? true : false;

        if (climbHeld)
        {
            if (!hasStartedClimb) hasStartedClimb = true;
        }
        else
        {
            if (hasStartedClimb)
            {
                GetComponent<Animator>().Play("CharacterClimbIdle");
            }
        }
    }
    /*
    void FixedUpdate()
    {
    // Climbing
    if (hasStartedClimb && !climbHeld)
        {
            if (horizontal > 0 || horizontal < 0) ResetClimbing();
        }
        else if (hasStartedClimb && climbHeld)
        {
            float height = GetComponent<SpriteRenderer>().size.y;
            float topHandlerY = Half(ladder.transform.GetChild(0).transform.position.y + height);
            float bottomHandlerY = Half(ladder.transform.GetChild(1).transform.position.y + height);
            float transformY = Half(transform.position.y);
            float transformVY = transformY + vertical;

            if (transformVY > topHandlerY || transformVY < bottomHandlerY)
            {
                ResetClimbing();
            }
            else if (transformY <= topHandlerY && transformY >= bottomHandlerY)
            {
                rigidBody2D.bodyType = RigidbodyType2D.Kinematic;
                if (!transform.position.x.Equals(ladder.transform.position.x))
                    transform.position = new Vector3(ladder.transform.position.x, transform.position.y, transform.position.z);

                GetComponent<Animator>().Play("CharacterClimb");
                Vector3 forwardDirection = new Vector3(0, transformVY, 0);
                Vector3 newPos = Vector3.zero;
                if (vertical > 0)
                    newPos = transform.position + forwardDirection * Time.deltaTime * climbSpeed;
                else if (vertical < 0)
                    newPos = transform.position - forwardDirection * Time.deltaTime * climbSpeed;
                if (newPos != Vector3.zero) rigidBody2D.MovePosition(newPos);
            }
        }
    */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PICKUP"))
        {
            cuadradoInteraccion.SetActive(true);
        }
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            isCloseToLadder = true;
            this.ladder = collision.transform;
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