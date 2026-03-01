using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float sensitivity, movespeed, stamina, fov, sprintIncreaser, staminaDecreaser, staminaIncreaser;
    public bool isSprinting, regenerator;
    public GameObject mainCamera;
    
    private CharacterController controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        float fov = mainCamera.GetComponent<Camera>().fieldOfView; isSprinting = false;
        regenerator = true;
        stamina = 100;
        sprintIncreaser = 1.75f;
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Vector3 velocity = transform.TransformDirection(move) * movespeed;
        controller.Move(velocity * Time.deltaTime);

        if (stamina > 20)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                movespeed *= sprintIncreaser;
                isSprinting = true;
                regenerator = false;
                mainCamera.GetComponent<Camera>().fieldOfView = 75f;
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
        mainCamera.GetComponent<Camera>().fieldOfView = 60f;
    }
}
