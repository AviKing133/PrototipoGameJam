using UnityEngine;

public class PickupController : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PLAYER") && Input.GetKeyDown(KeyCode.E) && !GameManager.instance.playerHaveDash)
        {
            Destroy(gameObject);
            GameManager.instance.ActivateDash();
        }
        if (collision.CompareTag("PLAYER") && Input.GetKeyDown(KeyCode.E) && GameManager.instance.playerHaveDash)
        {
            Destroy(gameObject);
            GameManager.instance.ActivateWallJump();
        }
    }
}
