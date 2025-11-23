using UnityEngine;

public class InteractScript : MonoBehaviour
{
    private bool facingRight = true;
    [SerializeField] private float directionThreshold = 0.01f; // Para evitar falsos positivos

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Detecta cambio de dirección
        if (horizontalInput > directionThreshold && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < -directionThreshold && facingRight)
        {
            Flip();
        }
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
