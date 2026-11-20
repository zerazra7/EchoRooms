using UnityEngine;

// first mouse look script (the game now uses the FPSController's own mouse look)
public class NewMonoBehaviourScript : MonoBehaviour
{
    [Range(50, 500)]
    public float sens;

    public Transform body;

    float xRot = 0f;

    // lock the mouse to the middle of the screen
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // mouse up/down turns the camera (max 80 degrees), left/right turns the body
    private void Update()
    {
        float rotX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float rotY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;

        xRot -= rotY;
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        body.Rotate(Vector3.up * rotX);
    }
}
