using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public float sensitivity, maxUpAngle = 90f, xRotation = 0f;
    public RaycastHit hit;
    public float distance;
    public TaskScript taskScript;
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


        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                taskScript.Task1Checker(hit.transform.gameObject);
                if (taskScript.taskNumber == 2)
                {
                    taskScript.Task2Checker(hit.transform.gameObject);
                }
                else if (taskScript.taskNumber == 3)
                {
                    taskScript.Task3Checker(hit.transform.gameObject);
                }
                else if (taskScript.taskNumber == 4)
                {
                    taskScript.Task4Checker(hit.transform.gameObject);
                }
            }
        }
    }
}
