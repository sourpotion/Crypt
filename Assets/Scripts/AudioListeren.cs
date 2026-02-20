using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AudioListeren : MonoBehaviour
{
    protected float amplutideNerfAtHit = 1.5f;
    protected float amplutideNerfAUnit = 1.7f;
    protected float minAmplutideToHear = 10f;

    private GameMangeren gameMangeren;
    private float maxSizeOfPassing = .1f;
    private int layerMaskRaycast; //thign where raycast can't past through

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        gameMangeren = GameMangeren.Instance;
        layerMaskRaycast = ~LayerMask.GetMask("Ignore RayCast");
        gameMangeren.onMakingNoise += NoiseMaked;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual bool CouldHear(Vector3 soundLocation, float amplutide)
    {
        float disant = (transform.position - soundLocation).magnitude;
        float minAmplutideOff = disant * amplutideNerfAUnit;
        
        return amplutide - minAmplutideOff > minAmplutideToHear;
    }

    protected virtual (bool hearSomething, float amplutideHeHeard) CanHear(Vector3 soundLocation, float amplutide)
    {
        //set var
        Vector3 dirToListeren = (transform.position - soundLocation).normalized;
        float currentAmplutide = amplutide;
        Vector3 currentPos = soundLocation;

        // find the scale vers of the dir
        float biggestVector = Mathf.Max(Mathf.Abs(dirToListeren.x), Mathf.Abs(dirToListeren.y), Mathf.Abs(dirToListeren.z));
        float scale = maxSizeOfPassing / biggestVector;
        Vector3 passingThroughOffset = dirToListeren * scale;

        //loop until it is by finnish redo every time it hit
        while (true)
        {
            RaycastHit hit;
            float disantToListeren = (transform.position - currentPos).magnitude;
            float disantTravel;

            Debug.DrawLine(currentPos, transform.position, Color.blue, 60f);
            Debug.DrawRay(currentPos, dirToListeren * disantToListeren, Color.red, 60f);
            bool hitSomething = Physics.Raycast(currentPos, dirToListeren, out hit, disantToListeren, layerMaskRaycast);

            //set the value for the new round and check the disant
            if (hitSomething)
            {
                disantTravel = (currentPos - hit.point).magnitude;
                currentPos = hit.point + passingThroughOffset;
                currentAmplutide = (currentAmplutide - disantTravel * amplutideNerfAUnit) / amplutideNerfAtHit;
            }
            else
            {
                disantTravel = (currentPos - transform.position).magnitude;
                currentAmplutide = currentAmplutide - disantTravel * amplutideNerfAUnit;
            }

            //check of it shoulld loop again
            if (currentAmplutide < minAmplutideToHear) {return(false, 0);}

            if (!hitSomething) {return(true, currentAmplutide);}
        }
    }

    protected virtual void NoiseMaked(Vector3 soundLocation, float amplutide)
    {

        //check or we must run raycast
        if (!CouldHear(soundLocation, amplutide)) {return;}

        (bool hearSomething, float amplutideHeHeard) hearInfo = CanHear(soundLocation, amplutide);
        if (!hearInfo.hearSomething) {return;}

        OnHeardSomething(hearInfo.amplutideHeHeard);
    }

    protected virtual void OnHeardSomething(float amplutide)
    {
        print(amplutide);
        Debug.LogWarning("pls put a sub-class");
    }

}
