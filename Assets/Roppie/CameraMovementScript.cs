using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public float sensitivity, maxUpAngle = 90f, xRotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxUpAngle, maxUpAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
