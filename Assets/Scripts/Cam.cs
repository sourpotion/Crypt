using System;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [Header("Must")]
    public GameObject camLightHitbox;
    [HideInInspector] public Action seePlr; //so when cam see the plr it can send a message back
    
    private GameObject target; //not proof multiple target but then i have to rewrite if that is so this then //also we get this because we don't want to u knw do this 500 times on public or i should ask the server for the target
    private bool enemieOnIt = false;
    private float angleOfCam;
    private float camSeeDisant;
    private int layerMaskRaycast; //thign where raycast can't past through
    private Light camLightHitboxRedLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        layerMaskRaycast = ~LayerMask.GetMask("Ignore RayCast");
        camLightHitboxRedLight = camLightHitbox.GetComponent<Light>();
        angleOfCam = camLightHitboxRedLight.innerSpotAngle / 2;
        camSeeDisant = camLightHitboxRedLight.range;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemieOnIt && target && CanSeePlr())
        {

            //send mesage it see plr
            seePlr.Invoke();
            seePlr = null;

            //set it to normal
            enemieOnIt = false;
            TurnOffCam();
        }
    }

    bool CanSeePlr()
    {
        
        Vector3 targetPos = target.transform.position;

        //get the radious
        Vector3 directionToTarget = targetPos - camLightHitbox.transform.position;
        float angle = Vector3.Angle(camLightHitbox.transform.forward, directionToTarget);

        //check of he can see the plr
        if (angle > angleOfCam) {return false;}

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(camLightHitbox.transform.position, directionToTarget, out hit, camSeeDisant, layerMaskRaycast);

        if (!hitSomething || hit.transform.gameObject != target) {return false;}

        return true;
    }

    void TurnOnCam()
    {
        camLightHitboxRedLight.enabled = true;
    }

    public void TurnOffCam()
    {
        camLightHitboxRedLight.enabled = false;
    }

    public void SetEnemieOnCam(GameObject enemieTarget) //if frame perfect moment it is mabye a bug
    {

        if (!enemieOnIt)
        {
            
            TurnOnCam();
            enemieOnIt = true;
        }

        if (target == null) {target = enemieTarget;}
    }
}
