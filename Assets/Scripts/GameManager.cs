using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerHaveDash = false;
    public bool playerWallJump = false;

    public static GameManager instance;

    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip wallJumpClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip pickupClip;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateDash()
    {
        playerHaveDash = true;
        PlaySound(dashClip);
    }

    public void ActivateWallJump()
    {
        playerWallJump = true;
        PlaySound(wallJumpClip);
    }

    public void PlayerJump()
    {
        PlaySound(jumpClip);
    }

    public void PlayerDamage()
    {
        PlaySound(damageClip);
    }

    public void PlayerPickup()
    {
        PlaySound(pickupClip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
