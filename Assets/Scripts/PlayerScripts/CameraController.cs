using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraPos;
    private Transform playerPos;
    private Vector3 offset = new Vector3(0, 0, -5f);
    // Start is called before the first frame update
    void Start()
    {
        cameraPos = GetComponent <Transform>();
        playerPos = GameObject.Find("Player").GetComponent <Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       cameraPos.position = playerPos.position + offset;
    }
}
