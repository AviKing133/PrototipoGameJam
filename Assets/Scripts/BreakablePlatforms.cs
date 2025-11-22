using UnityEngine;

public class BreakablePlatforms : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PLAYER"))
        {
            Destroy(gameObject, 1.5f);
        }
    }
}
