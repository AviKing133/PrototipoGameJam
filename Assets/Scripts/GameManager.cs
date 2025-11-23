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
    [SerializeField] private AudioClip lvl1;
    [SerializeField] private AudioClip lvl2;
    [SerializeField] private AudioClip lvl3;

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

    public void Playlvl1()
    {
        PlayMusic(lvl1);
    }
    public void Playlvl2()
    {
        PlayMusic(lvl2);
    }
    public void Playlvl3()
    {
        PlayMusic(lvl3);
    }

    public void ActivateDash()
    {
        playerHaveDash = true;
        PlayEffect(dashClip);
    }

    public void ActivateWallJump()
    {
        playerWallJump = true;
        PlayEffect(wallJumpClip);
    }

    public void PlayerJump()
    {
        PlayEffect(jumpClip);
    }

    public void PlayerDamage()
    {
        PlayEffect(damageClip);
    }

    public void PlayerPickup()
    {
        PlayEffect(pickupClip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
    public void PlayEffect(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

}
