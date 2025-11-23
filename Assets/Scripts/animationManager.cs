using System.Threading;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    [SerializeField] Animator anim1;
    [SerializeField] GameObject elOtro;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject image;

    private float timer;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);

        if(timer >= 3f)
        {
            anim1.SetInteger("state", 1);
        }
        if(timer >= 5f)
        {
            //Instantiate(elOtro, enemy.transform);
            elOtro.SetActive(true);
        }
        if(timer >= 7f)
        {
            Rigidbody2D rb = elOtro.GetComponent<Rigidbody2D>();    
            rb.AddForce(Vector2.right * 15);
        }
        if(timer >= 10f)
        {
            image.SetActive(true);
        }
        if(timer >= 11f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
}
