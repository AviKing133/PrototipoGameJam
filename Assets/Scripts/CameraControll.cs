using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private Transform player;  
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float yOffset = 0f; 

    private float fixedX;
    private float fixedZ;

    void Start()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = new Vector3(fixedX, player.position.y + yOffset, fixedZ);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}