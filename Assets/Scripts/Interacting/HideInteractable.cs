using UnityEngine;
using static IInteractible;

public class HideInteractable : MonoBehaviour, IInteractable
{
    public Transform promptAnchor;

    public void Interact()
    {
        Debug.Log("hidden.");
    }

    public string GetPromptText()
    {
        return "Press E to hide";
    }

    public Transform GetPromptAnchor()
    {
        return promptAnchor != null ? promptAnchor : transform;
    }
}
