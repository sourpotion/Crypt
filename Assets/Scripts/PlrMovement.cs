using System.Runtime.InteropServices;
using UnityEngine;

public class PlrController : MonoBehaviour
{
    public GameObject mainCamera;

    private bool isSprinting = false;
    private float currentMoveSpeed;
    private float runSpeed = 14f;
    private float defaultMovespeed = 8f;
    private float stamina = 100;
    private float fov;
    private float staminaDecreaser = 10f;
    private float staminaIncreaser = 7f;
    private float sensitivity = 1f;
    private float gravity = 0;
    private float gravityDecreaser = 5f;
    private CharacterController cc;
    private GameMangeren gameMangeren;

    //robbie script* syntax is all him

    void Start()
    {
        gameMangeren = GameMangeren.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        float fov = mainCamera.GetComponent<Camera>().fieldOfView;
    }

    void Update()
    {
        if (gameMangeren.plrHiding || gameMangeren.plrDied || gameMangeren.gameIsPause)

        if (Input.GetKeyDown(KeyCode.LeftShift)){StopRunning();}
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {StartRun();}

        if (isSprinting == true) {stamina -= staminaDecreaser;}

        if (stamina < 0)
        {
            if (isSprinting == true)
            {   
                currentMoveSpeed = defaultMovespeed;
                stamina = 0;
                FovEffectReset();
            }
            isSprinting = false;
        }

        if (!isSprinting) {stamina += staminaIncreaser;}

        if (stamina > 100) {stamina = 100;}

        Gravity();
        Move();

    }

    void StartRun()
    {
        if (isSprinting && stamina > 0)
        {
            currentMoveSpeed = defaultMovespeed;
            isSprinting = false;
            FovEffectReset();
        }
    }

    void StopRunning()
    {
        currentMoveSpeed = runSpeed;
        isSprinting = true;
        mainCamera.GetComponent<Camera>().fieldOfView = 75f;
    }

    void Move()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 velocity = transform.right  * move.x + transform.forward * move.z + -transform.up * gravity;
        cc.Move(velocity * Time.deltaTime);
    }

    void Gravity()
    {
        if (cc.isGrounded) {gravity = 0; return;}

        gravity -= gravityDecreaser * Time.deltaTime;
    }

    public void FovEffectReset()
    {
        mainCamera.GetComponent<Camera>().fieldOfView = 60f;
    }
}