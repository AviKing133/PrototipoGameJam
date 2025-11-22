using UnityEngine;

public class LadderScript : MonoBehaviour
{
    private void Awake()
    {
        float height = GetComponent<SpriteRenderer>().size.y;
        float width = GetComponent<SpriteRenderer>().size.x;
        Transform topHandler = transform.GetChild(0).transform;
        Transform bottomHandler = transform.GetChild(1).transform;
        topHandler.position = new Vector3(transform.position.x, transform.position.y + (height / 2), 0);
        bottomHandler.position = new Vector3(transform.position.x, transform.position.y - (height / 2), 0);
        GetComponent<BoxCollider2D>().offset = Vector2.zero;
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
