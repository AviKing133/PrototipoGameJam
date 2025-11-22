using UnityEngine;

public class PickupController : MonoBehaviour
{
    private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (!GameManager.instance.playerHaveDash)
            {
                GameManager.instance.ActivateDash();
            }
            else
            {
                GameManager.instance.ActivateWallJump();
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PLAYER"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PLAYER"))
        {
            playerInside = false;
        }
    }
}
