using UnityEngine;

public class DDDTestingBrokenParts : MonoBehaviour
{
    public HumanoidBody humanoidBody;
    public string brokenName;
    public string fixName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            humanoidBody.BreakPart(brokenName);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            humanoidBody.HealPart(fixName);
        }
    }
}
