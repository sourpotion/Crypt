using UnityEngine;
using static IInteractible;

public class ExitInteractable : MonoBehaviour, IInteractable
{
    public Transform promptAnchor;
    public GameObject ExitTrigger;
    public ExitScript ExitScript;

    public void Interact()
    {
        ExitScript.GetExit = true;
        Debug.Log("Escaped");

    }

    public string GetPromptText()
    {
        return "E-Escape";
    }

    public Transform GetPromptAnchor()
    {
        return promptAnchor != null ? promptAnchor : transform;
    }
}
