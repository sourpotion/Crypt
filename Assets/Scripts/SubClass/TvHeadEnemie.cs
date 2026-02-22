using System.Collections.Generic;
using UnityEngine;

public class TvHeadEnemie: Enemie
{
    [Header("Stats")]
    public float timeBeforeUsingAbility = 1f;

    private List<Cam> activeCams = new List<Cam>();
    private List<Cam> unActiveCams = new List<Cam>();
    private float timeWithoutSeeingThePlr = 0f;

    protected override void Start()
    {
        base.Start();

        //set the all of the cam to camActive
        int ttCams = camFolder.transform.childCount;

        for (int i = 0; i < ttCams; i++)
        {
            Cam camScript = camFolder.transform.GetChild(i).gameObject.GetComponent<Cam>();
            unActiveCams.Add(camScript);
        }

        SecondAbility();
    }

    protected override void Update()
    {
        timeWithoutSeeingThePlr += Time.deltaTime;

        base.Update();

        if (timeWithoutSeeingThePlr >= timeBeforeUsingAbility)
        {
            Ability();
        }
    }

    void Ability()
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

}
