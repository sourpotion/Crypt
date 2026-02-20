using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class Enemie : MonoBehaviour
{
    [Header("Must")]
    public AudioSource chaseSound;

    [Header("Stats")]
    
    public float viewDisant = 100f; //disant to see the plr
    public float viewRadius = 45; //viewRadiuos to see the plr
    //public float viewRadiusY = 10; //viewRadiuos of up and down to see the plr
    public float walkSpeed = 3.5f; //speed while walking
    public float runSpeed = 8f; //speed while running
    public float angerTime = 4f; //time before losing u
    public float lookingAroundDuration = 1f; //time that it look Around
    public float timeBeforeUsingAbility = 1f;

    [System.Serializable]
    public class PartrolsArea
    {
        public GameObject patrolsFolders; //so u don't have to do this 100 times just the areaFolders
        public GameObject[] areaTrigger; //area that say u there
        [HideInInspector] public GameObject[] partrols; //partrolls where he can walk
        [HideInInspector] public int ttPartrols; // totale partrolls there are 
    } 

    public PartrolsArea[] partrolsAreas; //sooon folders but for now //so u don't have to do this 100 times just the areaFolders
    public GameObject camFolder;
    public GameObject target; //the plr
    public GameObject audiosCamsFolder;
    [HideInInspector] public int areaId = 0;

    [Header("Debug")]
    private string targetTag = "Player";
    [SerializeField] private string state = "Idle"; //debug
    private bool isAnger;
    private bool alrHaveLookAround = false; //so the think can't be active
    private bool isLookingAround = false; //debounce
    private float currentTimerOfAnger;
    private NavMeshAgent agent;
    private int layerMaskRaycast; //thign where raycast can't past through
    private List<Cam> activeCams = new List<Cam>();
    private List<Cam> unActiveCams = new List<Cam>();
    private float timeWithoutSeeingThePlr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    protected virtual void Setup() 
    {
        layerMaskRaycast = ~LayerMask.GetMask("Ignore RayCast"); //setup
        agent = GetComponent<NavMeshAgent>();

        for (int id = 0; id < partrolsAreas.Length; id++)
        {
            PartrolsArea partrolInfo = partrolsAreas[id];

            //give the script to the area to see the plr
            foreach (GameObject area in partrolInfo.areaTrigger)
            {
                InWichAreaIsThePlr newScript = area.AddComponent<InWichAreaIsThePlr>(); //add script
            
                //fix the value of the script
                newScript.areaId = id;
                newScript.enemieScript = gameObject.GetComponent<Enemie>();
            }

            //look up the totale partrolls there are
            partrolInfo.ttPartrols = partrolInfo.patrolsFolders.transform.childCount; 

            //get the childs of the folder and put it in partrolls
            partrolInfo.partrols = new GameObject[partrolInfo.ttPartrols];

            for (int i = 0; i < partrolInfo.ttPartrols; i++)
            {
                GameObject newPartroll = partrolInfo.patrolsFolders.transform.GetChild(i).gameObject;
                partrolInfo.partrols[i] = newPartroll;
            }
        }

        //set the all of the cam to camActive
        int ttCams = camFolder.transform.childCount;

        for (int i = 0; i < ttCams; i++)
        {
            Cam camScript = camFolder.transform.GetChild(i).gameObject.GetComponent<Cam>();
            unActiveCams.Add(camScript);
        }

        SecondAbility();

        //settings
        agent.speed = walkSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timeWithoutSeeingThePlr += Time.deltaTime;

        if (SeeThePlr())
        {
            GetAnger();
        }

        if (isAnger)
        {
            if (currentTimerOfAnger > angerTime) 
            {
                //stop knowing where plr is
                isAnger = false; //is not anger anymore
                chaseSound.Stop(); //stop the chase music
                agent.speed = walkSpeed; //soon fix
                alrHaveLookAround = false; //go lookaround when got to the last trace
                state = "looking for plr last pos";
            }
            else //optimaseren
            {
                currentTimerOfAnger += Time.deltaTime; 
            }

            GoToTarget(target.transform.position);
        }
        else
        {
            if (!isLookingAround && !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)) //chatgpt *
            {
                Think();
            }
        }

        if (timeWithoutSeeingThePlr >= timeBeforeUsingAbility)
        {
            Ability();
        }
    }

    protected virtual void Think() //for now always id0  for testing
    {
        if (!alrHaveLookAround)
        {
            StartCoroutine(LookAround());
            
            alrHaveLookAround = true; //so the other go
            return;
        }

        //reset value
        alrHaveLookAround = false;
        state = "partrolling";

        //getRngTargetPoint
        PartrolsArea partrolInfo = partrolsAreas[areaId];
        int rngNumber = UnityEngine.Random.Range(0, partrolInfo.ttPartrols - 1);
        GameObject walkTarget = partrolInfo.partrols[rngNumber];
        
        //go to the point
        GoToTarget(walkTarget.transform.position);
    }

    protected virtual void Ability()
    {
        if (unActiveCams.Count == 0) {timeWithoutSeeingThePlr = 0; return;}

        int rngNumber = UnityEngine.Random.Range(0, unActiveCams.Count);
        Cam camToActive = unActiveCams[rngNumber];
        unActiveCams.Remove(camToActive);

        activeCams.Add(camToActive);
        camToActive.SetEnemieOnCam(target);
        camToActive.seePlr += GetAnger;

        timeWithoutSeeingThePlr = 0;
    }

    protected virtual IEnumerator LookAround()
    {
        isLookingAround = true;
        state = "looking around";

        yield return new WaitForSeconds(lookingAroundDuration);

        isLookingAround = false;
    }

    protected virtual void GetAnger()
    {
        if (!isAnger)
        {
            isAnger = true;
            agent.speed = runSpeed;
            chaseSound.Play();
            state = "Chasing";

            for (int i = 0; i < activeCams.Count; i++)
            {
                Cam oldCam = activeCams[i];
                oldCam.TurnOffCam();

                activeCams.RemoveAt(i);
                unActiveCams.Add(oldCam);
            }
        }

        currentTimerOfAnger = 0f;
        timeWithoutSeeingThePlr = 0f;
    }

    protected virtual void GoToTarget(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }

    protected virtual void SecondAbility()
    {
        //get all child then add the event
        foreach (Transform audioCam in audiosCamsFolder.transform)
        {
            MicrophoneCam microphoneCam = audioCam.GetComponent<MicrophoneCam>();
            microphoneCam.onEnemieHear += GetAnger;
        }
    }

    protected virtual void Attack()
    {
        print("Attack*");
    }

    protected virtual bool SeeThePlr()
    {
        Vector3 targetPos = target.transform.position;

        //get the radious
        Vector3 directionToTarget = targetPos - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        //check of he can see the plr
        if (angle > viewRadius) {return false;}

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(transform.position, directionToTarget, out hit, viewDisant, layerMaskRaycast);

        if (!hitSomething || hit.transform.tag != targetTag) {return false;}

        return true;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        string hitTag = collision.transform.tag;

        if (hitTag == targetTag) {Attack();}
    }
}
