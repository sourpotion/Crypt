using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    [Header("Must")]
    public AudioSource chaseSound;

    [Header("Settings")]
    
    public GameObject[] patrols; //sooon folders but for now //so u don't have to do this 100 times just the areaFolders
    public GameObject target; //the plr

    private string targetTag = "Player";
    [SerializeField] private string state = "Idle"; //debug
    private bool isAnger;
    private float currentTimerOfAnger;
    
    private NavMeshAgent agent;

    //stats / settings
    private float viewDisant = 100f; //disant to see the plr
    private float viewRadius = 45; //viewRadiuos to see the plr
    //private float viewRadiusY = 10; //viewRadiuos of up and down to see the plr
    private float walkSpeed = 3.5f; //speed while walking
    private float runSpeed = 8f; //speed while running
    private float angerTime = 4f; //time before losing u
    private int layerMaskRaycast; //thign where raycast can't past through



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMaskRaycast = ~LayerMask.GetMask("Ignore RayCast"); //setup
        agent = GetComponent<NavMeshAgent>();

        //settings
        agent.speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (SeeThePlr())
        {
            if (!isAnger)
            {
                isAnger = true;
                agent.speed = runSpeed;
                chaseSound.Play();
                state = "Chasing";
            }

            currentTimerOfAnger = 0f;
        }

        if (isAnger)
        {
            if (currentTimerOfAnger > angerTime) 
            {
                isAnger = false;
                chaseSound.Stop();
                state = "looking for plr last pos";
            }

            currentTimerOfAnger += Time.deltaTime;

            ChaseTarget();
        }
        else
        {
            if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)) //ai *
            {
                
            }
        }
    }

    void ChaseTarget()
    {
        agent.SetDestination(target.transform.position);
    }

    bool SeeThePlr()
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

    void OnCollisionEnter(Collision collision)
    {
        string hitTag = collision.transform.tag;

        if (hitTag == targetTag) {print("plr got hit");}
    }
}
