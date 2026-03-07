using System.Reflection;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HumanoidBody : MonoBehaviour
{
    [Header("settings")]
    public float walkSpeed = 8;
    public float runSpeed = 12;

    [System.Serializable]
    private class Bodys
    {
        public string name;
        public Image imageOfBody;
        [HideInInspector] public bool broken = false;
    }

    [SerializeField] private Bodys[] bodys = new Bodys[6];
    
    [Header("Must")]
    public Volume volume;

    [Header("Nerfs")]
    public float downSpeedALegBroken = 2;
    public float maxStaminaNerf = 30;
    public float maxStaminaRegNerf = .5f;
    public float interactionNerf = 0.2f;

    [HideInInspector] public float currentSpeedNerf = 0;
    [HideInInspector] public float currentMaxStaminaNerf = 0;
    [HideInInspector] public float currentStaminaRegNerf = 0;
    [HideInInspector] public float currentInteractionNerf = 0;

    private DepthOfField dof;
    private float colorTweenTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume.profile.TryGet<DepthOfField>(out dof);

        //debug
        foreach (Bodys body in bodys)
        {
            if (body.name == null || body.imageOfBody == null)
            {
                Debug.LogWarning("add in body the info");
            }
        }
    }

    public void BreakPart(string partName)
    {
        //get the body part
        Bodys bodyPart = GetBody(partName);

        if (bodyPart == null || bodyPart.broken) {return;} //so it is not gonna be buggy
        bodyPart.broken = true; //deboounce
        bodyPart.imageOfBody.DOColor(Color.red, colorTweenTime);

        //Fire the Function
        string methodName = partName + "Broken";
        MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (method != null)
        {
            method.Invoke(this, new object[] {bodyPart});
        }
        else
        {
            Debug.LogWarning(methodName + " is not a funtion");
        }
        
    }

    public void HealPart(string partName)
    {
        //get the body part
        Bodys bodyPart = GetBody(partName);

        if (bodyPart == null || !bodyPart.broken) {return;} //so it is not gonna be buggy
        bodyPart.broken = false; //debounce
        bodyPart.imageOfBody.DOColor(Color.black, colorTweenTime);

        //Fire the Function
        string methodName = partName + "Healed";
        MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (method != null)
        {
            method.Invoke(this, new object[] {bodyPart});
        }
        else
        {
            Debug.LogWarning(methodName + " is not a funtion");
        }
    }

    void HeadBroken(Bodys bodyPart)
    {
        dof.active = true;
    }

    void LeftArmBroken(Bodys bodyPart)
    {
        currentInteractionNerf += interactionNerf;
        
        //debug
        currentInteractionNerf = math.clamp(currentInteractionNerf, 0, interactionNerf * 2);
    }

    void RightArmBroken(Bodys bodyPart)
    {
        currentInteractionNerf += interactionNerf;
        
        //debug
        currentInteractionNerf = math.clamp(currentInteractionNerf, 0, interactionNerf * 2);
    }

    void LeftArmHealed(Bodys bodyPart)
    {
        currentInteractionNerf -= interactionNerf;
        
        //debug
        currentInteractionNerf = math.clamp(currentInteractionNerf, 0, interactionNerf * 2);
    }

    void RightArmHealed(Bodys bodyPart)
    {
        currentInteractionNerf -= interactionNerf;
        
        //debug
        currentInteractionNerf = math.clamp(currentInteractionNerf, 0, interactionNerf * 2);
    }

    void HeadHealed(Bodys bodyPart)
    {
        dof.active = false;
    }

    void TorsoBroken(Bodys bodyPart)
    {
        currentMaxStaminaNerf += maxStaminaNerf;
        currentStaminaRegNerf += maxStaminaRegNerf;

        //debug
        currentMaxStaminaNerf = math.clamp(currentMaxStaminaNerf, 0, maxStaminaNerf);
        currentMaxStaminaNerf = math.clamp(currentMaxStaminaNerf, 0, maxStaminaRegNerf);
    }

    void TorsoHealed(Bodys bodyPart)
    {
        currentMaxStaminaNerf -= maxStaminaNerf;
        currentStaminaRegNerf = maxStaminaRegNerf;

        //debug
        currentMaxStaminaNerf = math.clamp(currentMaxStaminaNerf, 0, maxStaminaNerf);
        currentMaxStaminaNerf = math.clamp(currentMaxStaminaNerf, 0, maxStaminaRegNerf);
    }

    void LeftLegBroken(Bodys bodyPart)
    {
        currentSpeedNerf += downSpeedALegBroken;
        currentSpeedNerf = math.clamp(currentSpeedNerf, 0, downSpeedALegBroken * 2); //if it every go at sametime u never go slower then the max slowness
    }

    void LeftLegHealed(Bodys bodyPart)
    {
        currentSpeedNerf -= downSpeedALegBroken;
        currentSpeedNerf = math.clamp(currentSpeedNerf, 0, downSpeedALegBroken * 2); //if it every go at sametime u never go slower then the max slowness
    }

    void RightLegBroken(Bodys bodyPart)
    {
        currentSpeedNerf += downSpeedALegBroken;
        currentSpeedNerf = math.clamp(currentSpeedNerf, 0, downSpeedALegBroken * 2); //if it every go at sametime u never go slower then the max slowness
    }

    void RightLegHealed(Bodys bodyPart)
    {
        currentSpeedNerf -= downSpeedALegBroken;
        currentSpeedNerf = math.clamp(currentSpeedNerf, 0, downSpeedALegBroken * 2); //if it every go at sametime u never go slower then the max slowness
    }

    Bodys GetBody(string name)
    {
        foreach(Bodys body in bodys) //loop through every part
        {
            if (body.name == name) {return body;}
        }

        Debug.LogWarning(name + " is not a valid name");
        return null;
    }

    public bool IsBroken(string name)
    {
        Bodys bodyPart = GetBody(name);
        if (bodyPart == null) {Debug.LogWarning("name is invalid: " + name); return false;}

        return bodyPart.broken;
    }
}
