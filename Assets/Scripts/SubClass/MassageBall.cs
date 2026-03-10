using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class MassageBall: Enemie
{
    [Header("MassageBallOnly")]
    public AudioSource hitSound;

    private HumanoidBody humBody;
    private string[] bodyNames = new string[] { "LeftLeg", "RightLeg"};
    private float minPitch = .9f;
    private float maxPitch = 1.1f;
    private bool attackDebounce = false;


    protected override void Start()
    {
        humBody = target.GetComponent<HumanoidBody>();
        spawnPos = transform.position;

        //old one
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
    }

    protected override void Update()
    {
        //rest don't need it
        if (!isLookingAround) {Think();}
    }

    protected override void GoToTarget(Vector3 targetPos)
    {
        //teleport to it
        transform.position = targetPos;
    }

    protected override void Attack()
    {
        //debug
        if (attackDebounce) {return;}
        attackDebounce =  true;

        //attack
        StartCoroutine(WaitAttack());
    }

    IEnumerator WaitAttack()
    {
       //get random numbers needed
        int rngNumber = UnityEngine.Random.Range(0, bodyNames.Length);
        float pitchSound = UnityEngine.Random.Range(minPitch, maxPitch);

        //soundEffect
        hitSound.pitch = pitchSound;
        hitSound.Play();

        while (hitSound.isPlaying)
        {
            yield return null;
        }
        
        //break it
        humBody.BreakPart(bodyNames[rngNumber]); //sson random
        transform.position = new Vector3(0, 1000, 0);
        attackDebounce = false; 
    }

    public override void Respawn()
    {
        transform.position = spawnPos;
    }
}
