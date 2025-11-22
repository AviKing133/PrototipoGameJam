using UnityEngine;

public class LadderScript : MonoBehaviour
{
    GameObject coliderExit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coliderExit = transform.GetChild(1).gameObject;
        coliderExit.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarColiderExit()
    {
        Debug.Log("ActivarColiderExit");
        coliderExit.GetComponent<Collider2D>().enabled = true;
    }
    public void DesactivarColiderExit()
    {
        Debug.Log("ActivarColiderExit");
        coliderExit.GetComponent<Collider2D>().enabled = false;
    }

}
