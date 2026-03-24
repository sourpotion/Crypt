using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlrMovement : MonoBehaviour
{
    public GameObject mainCamera;
    public HumanoidBody humBody;

    [Header("SFX")]
    public AudioSource runningSfx;
    public AudioSource staminaZeroSfx;

    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public float stamina = 100;
    [HideInInspector] public float maxStamina = 100; //debug
    [HideInInspector] public int selectedIndex = 0;

    private GameMangeren gameMangeren;
    private float defaultMaxStamina = 100;
    private float currentMoveSpeed;
    private float runSpeed = 14f;
    private float defaultMovespeed = 8f;
    private float fov;
    private float staminaDecreaser = 30f;
    private float staminaIncreaser = 7f;
    private float sensitivity = 1f;
    private float gravity = 0;
    private float gravityDecreaser = 9f;
    private float timeToUseSprintAgain = 1f;
    private bool canUseSprint = true;
    private Vector3 delta = Vector3.zero; //momento
    private float accIncrease = 1; // how fast it is gonna to the goal
    private float friction = 1; //how fast to stop
    private CharacterController cc;

    //robbie script* syntax is all him

    void Start()
    {
        currentMoveSpeed = defaultMovespeed;
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        fov = mainCamera.GetComponent<Camera>().fieldOfView;
        gameMangeren = GameMangeren.Instance;
        maxStamina = defaultMaxStamina;
    }

    void Update()
    {
        //debug
        if (gameMangeren.plrDied || gameMangeren.gameIsPause) {return;}

        Gravity();
        Move();

        if (Input.GetKey(KeyCode.LeftShift) && canUseSprint) {StartRun();}
        else if (canUseSprint) {StopRunning();}

        if (isSprinting == true) {stamina -= staminaDecreaser * Time.deltaTime;}

        if (stamina < 0)
        {
            if (isSprinting == true)
            {   
                currentMoveSpeed = defaultMovespeed;
                stamina = 0;
                FovEffectReset();
            }

            isSprinting = false;
            runningSfx.Stop();
            staminaZeroSfx.Play();
        }

        if (!isSprinting) {stamina += (staminaIncreaser - humBody.maxStaminaRegNerf ) * Time.deltaTime;}

        if (stamina > maxStamina) {stamina = maxStamina;}

        maxStamina = defaultMaxStamina - humBody.currentMaxStaminaNerf;
    }

    void StopRunning()
    {
        if (isSprinting && stamina > 0)
        {
            StartCoroutine(SetSprintCooldown());
            currentMoveSpeed = defaultMovespeed;
            isSprinting = false;
            FovEffectReset();

            runningSfx.Stop();
        }
    }

    void StartRun()
    {   
        if (isSprinting) {return;}

        currentMoveSpeed = runSpeed;
        isSprinting = true;
        mainCamera.GetComponent<Camera>().fieldOfView = 75f;

        runningSfx.Play();
    }

    //make sure the plr can't spam it
    IEnumerator SetSprintCooldown()
    {
        canUseSprint = false;

        yield return new WaitForSeconds(timeToUseSprintAgain);

        canUseSprint = true;
    }

    void Move()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        
        //set the value
        float currentSpeedFrame = currentMoveSpeed - humBody.currentSpeedNerf;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 wishVelocity = (transform.right  * move.x + transform.forward * move.z).normalized * currentSpeedFrame;

        if (wishVelocity == Vector3.zero) {delta = Vector3.Lerp(delta, Vector3.zero, friction);}

        delta = delta += wishVelocity * (accIncrease * Time.deltaTime);
        Vector3 gravityVelocity = transform.up * gravity;

        if (delta.magnitude > currentSpeedFrame) {delta = delta.normalized * currentSpeedFrame;}

        cc.Move(delta * Time.deltaTime);
        cc.Move(gravityVelocity * Time.deltaTime);
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
