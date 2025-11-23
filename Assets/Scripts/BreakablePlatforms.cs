using UnityEngine;

public class BreakablePlatforms : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PLAYER"))
        {
            StartCoroutine(BreakAndRespawn());
        }
    }

    private System.Collections.IEnumerator BreakAndRespawn()
    {
        // Espera 1 segundo antes de "romperse"
        yield return new WaitForSeconds(1f);

        spriteRenderer.enabled = false;
        collider2D.enabled = false;

        // Espera 5 segundos antes de volver a aparecer
        yield return new WaitForSeconds(5f);

        spriteRenderer.enabled = true;
        collider2D.enabled = true;
    }
}
