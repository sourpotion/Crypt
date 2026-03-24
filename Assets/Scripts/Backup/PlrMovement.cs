using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlrMovement : MonoBehaviour
{
    [Header("must")]
    public GameObject mainCamera;
    public HumanoidBody humBody;
    public Volume volume;

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
    private float runSpeed = 10f;
    private float defaultMovespeed = 6f;
    private float staminaDecreaser = 30f;
    private float staminaIncreaser = 7f;
    private float sensitivity = 1f;
    private float gravity = 0;
    private float gravityDecreaser = 9.98f;
    private float timeToUseSprintAgain = 1f;
    private bool canUseSprint = true;
    private Vector3 delta = Vector3.zero; //momento
    private float accIncrease = 4; // how fast it is gonna to the goal
    private float friction = 4; //how fast to stop
    private CharacterController cc;
    private LensDistortion lensDistortion;
    private Vector3 move = Vector3.zero; //so when not moving u can't lose stamina

    //robbie script* syntax is all him

    void Start()
    {
        currentMoveSpeed = defaultMovespeed;
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        gameMangeren = GameMangeren.Instance;
        maxStamina = defaultMaxStamina;
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    void Update()
    {
        //debug
        if (gameMangeren.plrHiding || gameMangeren.plrDied || gameMangeren.gameIsPause) {return;}

        Gravity();
        Move();

        if (Input.GetKey(KeyCode.LeftShift) && canUseSprint) {StartRun();}
        else if (canUseSprint) {StopRunning();}

        if (isSprinting == true) 
        {
            if (move.magnitude < .1f) {runningSfx.Stop();}
            else
            {
                stamina -= staminaDecreaser * Time.deltaTime;

                if (!runningSfx.isPlaying) {runningSfx.Play();}
            }
        }

        if (stamina < 0)
        {
            if (isSprinting == true)
            {   
                currentMoveSpeed = defaultMovespeed;
                lensDistortion.active = false;
                stamina = 0;
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
            lensDistortion.active = false;

            runningSfx.Stop();
        }
    }

    void StartRun()
    {   
        if (isSprinting) {return;}

        currentMoveSpeed = runSpeed;
        isSprinting = true;
        lensDistortion.active = true;

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
        ///*
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        
        //set the value
        float currentSpeedFrame = currentMoveSpeed - humBody.currentSpeedNerf;
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 wishVelocity = (transform.right  * move.x + transform.forward * move.z).normalized * currentSpeedFrame;

        if (wishVelocity == Vector3.zero) {delta = Vector3.Lerp(delta, Vector3.zero, friction * Time.deltaTime);}

        delta += wishVelocity * (accIncrease * Time.deltaTime);
        Vector3 gravityVelocity = transform.up * gravity;

        if (delta.magnitude > currentSpeedFrame) {delta = delta.normalized * currentSpeedFrame;}

        cc.Move(delta * Time.deltaTime);
        cc.Move(gravityVelocity * Time.deltaTime);
        //*/

        /*
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        
        //set the value
        float currentSpeedFrame = currentMoveSpeed - humBody.currentSpeedNerf;
        Vector3 move = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
        Vector3 gravityVelocity = transform.up * gravity;

        cc.Move(move * currentMoveSpeed * Time.deltaTime);
        cc.Move(gravityVelocity * Time.deltaTime);
        */
    }

    void Gravity()
    {
        if (cc.isGrounded) {gravity = 0; return;}

        gravity -= gravityDecreaser * Time.deltaTime;
    }
}
