using System;
using System.Collections;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [Header("Must")]
    public GameObject camLightHitbox;
    [HideInInspector] public Action seePlr; //so when cam see the plr it can send a message back
    [HideInInspector] public GameObject target; //not proof multiple target but then i have to rewrite if that is so this then //also we get this because we don't want to u knw do this 500 times on public or i should ask the server for the target
    
    private bool enemieOnIt = false;
    private float timeBeforeSeeingIframe = 1;
    private float angleOfCam;
    private float camSeeDisant = 100f;
    private LayerMask layerMaskRaycast; //thign where raycast can't past through
    private Light camLightHitboxRedLight;
    private GameMangeren gameMangeren;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameMangeren = GameMangeren.Instance;
        layerMaskRaycast = ~LayerMask.GetMask("Ignore Raycast");
        camLightHitboxRedLight = camLightHitbox.GetComponent<Light>();
        angleOfCam = camLightHitboxRedLight.innerSpotAngle / 2;
        //camSeeDisant = camLightHitboxRedLight.range;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemieOnIt && target && CanSeePlr())
        {
            print("see plr");

            //send mesage it see plr
            seePlr.Invoke();

            //set it to normal
            TurnOffCam();
        }
    }

    bool CanSeePlr()
    {
        if (gameMangeren.gameIsPause || gameMangeren.plrHiding || gameMangeren.plrDied) {return false;}

        Vector3 targetPos = target.transform.position;

        //get the radious
        Vector3 directionToTarget = (targetPos - camLightHitbox.transform.position).normalized;
        float angle = Vector3.Angle(camLightHitbox.transform.forward, directionToTarget);
        float disant = Vector3.Distance(camLightHitbox.transform.position, targetPos);

        //check of he can see the plr
        if (angle > angleOfCam || disant > camSeeDisant) {return false;}

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(camLightHitbox.transform.position, directionToTarget, out hit, camSeeDisant, layerMaskRaycast, QueryTriggerInteraction.Ignore);
        
        Debug.DrawLine(camLightHitbox.transform.position, hit.point, Color.red, 20f);

        if (!hitSomething || hit.transform.gameObject != target) {return false;}

        return true;
    }

    IEnumerator TurnOnCam()
    {
        camLightHitboxRedLight.enabled = true;
        yield return new WaitForSeconds(timeBeforeSeeingIframe);
        enemieOnIt = true;
    }

    public void TurnOffCam()
    {
        camLightHitboxRedLight.enabled = false;
        enemieOnIt = false;
    }

    public void SetEnemieOnCam() //if frame perfect moment it is mabye a bug
    {

        if (!enemieOnIt)
        {
            
            StartCoroutine(TurnOnCam());
        }
    }
}
