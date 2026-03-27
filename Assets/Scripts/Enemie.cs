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
    [Header("Stats")]
    
    public float viewDisant = 100f; //disant to see the plr
    public float viewRadius = 45; //viewRadiuos to see the plr
    //public float viewRadiusY = 10; //viewRadiuos of up and down to see the plr
    public float walkSpeed = 3.5f; //speed while walking
    public float runSpeed = 8f; //speed while running
    public float angerTime = 4f; //time before losing u
    public float lookingAroundDuration = 1f; //time that it look Around
    public float attackAngle = 90f;

    [Header("Must")]
    public Transform head;

    [System.Serializable]
    protected class PartrolsArea
    {
        public GameObject patrolsFolders; //so u don't have to do this 100 times just the areaFolders
        public GameObject[] areaTrigger; //area that say u there
        [HideInInspector] public GameObject[] partrols; //partrolls where he can walk
        [HideInInspector] public int ttPartrols; // totale partrolls there are 
    } 

    [SerializeField] protected PartrolsArea[] partrolsAreas; //sooon folders but for now //so u don't have to do this 100 times just the areaFolders
    public GameObject target; //the plr
    [HideInInspector] public int areaId = 0;

    [Header("Debug")]
    protected string targetTag = "Player";
    [SerializeField] protected string state = "Idle"; //debug
    protected bool isAnger;
    protected bool alrHaveLookAround = false; //so the think can't be active
    protected bool isLookingAround = false; //debounce
    protected float currentTimerOfAnger;
    protected NavMeshAgent agent;
    protected int layerMaskRaycast; //thign where raycast can't past through
    protected Vector3 spawnPos;
    protected GameMangeren gameMangeren;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        layerMaskRaycast = ~LayerMask.GetMask("Ignore RayCast"); //setup
        agent = GetComponent<NavMeshAgent>();
        spawnPos = transform.position;
        gameMangeren = GameMangeren.Instance;

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

        //settings
        agent.speed = walkSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (SeeThePlr())
        {
            GetAnger();
        }

        if (isAnger) {InsideUpdate_Anger();}
        else
        {
            if (!isLookingAround && !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)) //chatgpt *
            {
                Think();
            }
        }
    }

    protected virtual void OnUnAnger()
    {
        //stop knowing where plr is
        isAnger = false; //is not anger anymore
        agent.speed = walkSpeed; //soon fix
        alrHaveLookAround = false; //go lookaround when got to the last trace
        state = "looking for plr last pos";
    }

    protected virtual void InsideUpdate_Anger()
    {
        
        if (currentTimerOfAnger > angerTime) {OnUnAnger();}
        else 
        {
            currentTimerOfAnger += Time.deltaTime; 
        }

        GoToTarget(target.transform.position);
    }

    protected virtual void Think()
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

    protected virtual IEnumerator LookAround()
    {
        isLookingAround = true;
        state = "looking around";

        yield return new WaitForSeconds(lookingAroundDuration);

        isLookingAround = false;
    }

    protected virtual void GetAnger()
    {
        if (!isAnger) {OnFirstGetAnger();}

        currentTimerOfAnger = 0f;
    }

    protected virtual void OnFirstGetAnger()
    {
        isAnger = true;
        agent.speed = runSpeed;
        state = "Chasing";
    }

    protected virtual void GoToTarget(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }
    
    protected bool PlrCanBeHit()
    {
        //debug
        if (gameMangeren.plrHiding) {return false;} //plr is hiding

        Vector3 targetPos = target.transform.position;

        //get angle
        Vector3 directionToTarget = targetPos - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        //check if he can attack
        if (angle > attackAngle) {return false;}

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(transform.position, directionToTarget, out hit, viewDisant, layerMaskRaycast, QueryTriggerInteraction.Ignore);

        if (!hitSomething || hit.transform.tag != targetTag) {return false;}

        return true;
    }

    protected virtual void Attack()
    {
        if (!PlrCanBeHit()) {return;}

        print("Attack*");
        GameMangeren.Instance.PlrDied();
    }

    protected virtual bool SeeThePlr()
    {
        if (gameMangeren.plrHiding) {return false;} //plr is hiding

        Vector3 targetPos = target.transform.position;

        //get the radious
        Vector3 directionToTarget = targetPos - head.position;
        float angle = Vector3.Angle(head.forward, directionToTarget);

        //check of he can see the plr
        if (angle > viewRadius) {return false;}

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(head.position, directionToTarget, out hit, viewDisant, layerMaskRaycast, QueryTriggerInteraction.Ignore);

        if (!hitSomething || hit.transform.tag != targetTag) {return false;}

        return true;
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        string hitTag = collision.transform.tag;

        if (hitTag == targetTag) {Attack(); return;}
    }

    public virtual void Respawn()
    {
        agent.Warp(spawnPos);

        //default seettings
        isAnger = false;
    }
}
