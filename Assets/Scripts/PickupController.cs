using UnityEngine;

public class PickupController : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PLAYER") && Input.GetKeyDown(KeyCode.E) && !GameManager.instance.playerHaveDash)
        {
            GameManager.instance.ActivateDash();
            Destroy(gameObject);
        }
        if (collision.CompareTag("PLAYER") && Input.GetKeyDown(KeyCode.E) && GameManager.instance.playerHaveDash)
        {
            GameManager.instance.ActivateWallJump();
            Destroy(gameObject);
        }
    }
}
