using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerHaveDash = false;
    public bool playerWallJump = false;

    static public GameManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }
    public void ActivateDash()
    {
        playerHaveDash = true;
    }
    public void ActivateWallJump()
    {
        playerWallJump = true;
    }
}
