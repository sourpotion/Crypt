using NUnit.Framework;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float sensitivity, movespeed, stamina, fov, sprintIncreaser, staminaDecreaser, staminaIncreaser;
    public bool isSprinting, regenerator;
    public GameObject mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        float fov = mainCamera.GetComponent<Camera>().fieldOfView; isSprinting = false;
        regenerator = true;
        stamina = 100;
        sprintIncreaser = 1.75f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        transform.Translate(move * movespeed * Time.deltaTime);


        if (stamina > 20)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                movespeed *= sprintIncreaser;
                isSprinting = true;
                regenerator = false;
                Camera.main.fieldOfView = 75f;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (isSprinting == true)
            {
                movespeed /= sprintIncreaser;
                isSprinting = false;
                regenerator = true;
                FovEffectReset();
            }
        }

        if (isSprinting == true)
        {
            stamina -= staminaDecreaser;
        }

        if (stamina < 0)
        {
            if (isSprinting == true)
            {   
                movespeed /= sprintIncreaser;
                stamina = 0;
                FovEffectReset();
            }
            isSprinting = false;
            regenerator = true;
        }

        if (regenerator == true)
        {
            stamina += staminaIncreaser;
        }

        if (stamina > 100)
        {
            stamina = 100;
            regenerator = false;
        }
    }

    public void FovEffectReset()
    {
        Camera.main.fieldOfView = 60f;
    }
}
