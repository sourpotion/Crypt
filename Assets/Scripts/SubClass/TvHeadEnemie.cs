using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvHeadEnemie: Enemie
{
    [Header("Stats")]
    public float timeBeforeUsingAbility = 10f;

    [Header("Must")]
    public GameObject camFolder;
    public GameObject audiosCamsFolder;
    public AudioSource chaseSound;
    public AudioSource angerSound;
    public AudioSource closeSfx;
    public AudioSource killSfx;
    public Camera deathCam;

    private Cam activeCams;
    private List<Cam> unActiveCams = new List<Cam>();
    private float timeWithoutSeeingThePlr = 0f;
    private bool canMove = true;
    private Animator animator;
    private MeshRenderer plrMesh;
    private bool isAttacking = false;
    private string doorTag = "Door";
    private bool openingDoor = false;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        AbilitySetup();
        SecondAbility();
        plrMesh = target.GetComponentInChildren<MeshRenderer>();
    }

    protected override void Update()
    {
        timeWithoutSeeingThePlr += Time.deltaTime;

        if ( isAttacking) { return; }
        base.Update();

        if (timeWithoutSeeingThePlr >= timeBeforeUsingAbility)
        {
            Ability();
        }
    }

    void Ability()
    {
        if (unActiveCams.Count == 0) {timeWithoutSeeingThePlr = 0; return;}

        if (activeCams != null) {activeCams.TurnOffCam();}

        int rngNumber = UnityEngine.Random.Range(0, unActiveCams.Count);
        Cam camToActive = unActiveCams[rngNumber];

        activeCams = camToActive;
        camToActive.SetEnemieOnCam();

        timeWithoutSeeingThePlr = 0;
    }

    void AbilitySetup()
    {
        //set the all of the cam to camActive
        int ttCams = camFolder.transform.childCount;

        for (int i = 0; i < ttCams; i++)
        {
            Cam camScript = camFolder.transform.GetChild(i).gameObject.GetComponent<Cam>();
            unActiveCams.Add(camScript);
        }

        foreach (Cam cam in unActiveCams)
        {
            cam.seePlr += GetAnger;
            cam.target = target;
        }
    }

    void SecondAbility()
    {
        //get all child then add the event
        foreach (Transform audioCam in audiosCamsFolder.transform)
        {
            MicrophoneCam microphoneCam = audioCam.GetComponent<MicrophoneCam>();
            microphoneCam.onEnemieHear += GetAnger;
        }
    }

    protected override void GetAnger()
    {
        if (isAttacking) { return; }

        base.GetAnger();

        timeWithoutSeeingThePlr = 0f;
    }

    protected override void OnFirstGetAnger()
    {
        base.OnFirstGetAnger();

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        canMove = false;
        animator.SetTrigger("GotAnger");
        animator.SetBool("IdleState", false);
        animator.SetBool("isRunning", true);

        angerSound.Play();
    }

    protected override IEnumerator LookAround()
    {
        animator.SetBool("IdleState", true);

        yield return base.LookAround();

        animator.SetBool("IdleState", false);
    }

    protected override void InsideUpdate_Anger()
    {
        if (!canMove)
        {
            return;
        }

        base.InsideUpdate_Anger();
    }

    protected override void OnUnAnger()
    {
        base.OnUnAnger();

        animator.SetBool("isRunning", false);
        chaseSound.Stop();
    }

    public void OnAngerFinished()
    {

        canMove = true;
        agent.isStopped = false;
        chaseSound.Play();
    }

    protected override void Attack()
    {
        if (isAttacking) { return;}

        isAttacking = true;
        gameMangeren.plrStun = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        deathCam.enabled = true;
        plrMesh.enabled = false;
        animator.SetTrigger("attack");
        chaseSound.Stop();
        closeSfx.Stop();
        killSfx.Play();
    }

    public void OnAttackFinished()
    {
        base.Attack();
        deathCam.enabled = false;
        isAttacking = false;
        gameMangeren.plrStun = false;
        plrMesh.enabled = true;
    }

    public override void Respawn()
     {
        agent.ResetPath();
        base.Respawn();

        if (activeCams != null) {activeCams.TurnOffCam();}
        if (chaseSound && chaseSound.isPlaying) {chaseSound.Stop();}
        if (angerSound && angerSound.isPlaying) {angerSound.Stop();}
        if (killSfx && killSfx.isPlaying) {killSfx.Stop();}

        timeWithoutSeeingThePlr = 0f;
        closeSfx.Play();
    }

    protected IEnumerator OpenDoor(Collider door)
    {
        if (openingDoor) {yield break;}

        openingDoor = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        DoorInteracion doorScript = door.GetComponent<DoorInteracion>();

        doorScript.OpenDoor();
        yield return new WaitForSeconds(doorScript.timeToOpen + .2f);

        agent.isStopped = false;

        yield return new WaitForSeconds(3f);

        doorScript.CloseDoor();
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        
        if (collision.transform.tag == doorTag)
        {
            StartCoroutine(OpenDoor(collision));
        }
    }
}
