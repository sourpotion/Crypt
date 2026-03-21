using System.Collections.Generic;
using UnityEngine;

public class TvHeadEnemie: Enemie
{
    [Header("Stats")]
    public float timeBeforeUsingAbility = 1f;

    [Header("Must")]
    public GameObject camFolder;
    public GameObject audiosCamsFolder;

    private Cam activeCams;
    private List<Cam> unActiveCams = new List<Cam>();
    private float timeWithoutSeeingThePlr = 0f;

    protected override void Start()
    {
        base.Start();

        AbilitySetup();
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

        if (activeCams) {activeCams.TurnOffCam();}

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
        base.GetAnger();

        timeWithoutSeeingThePlr = 0f;
    }

}
