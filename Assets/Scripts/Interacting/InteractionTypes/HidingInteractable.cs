using UnityEngine;
using static IInteractible;

public class HidoingInteractable : MonoBehaviour, IInteractable
{
    public Transform promptAnchor;
    public GameObject Player;
    public GameObject Hidepos;
    public GameObject unHidepos;
    public bool isHidden;
    private float interactCooldown = 1f;

    private float lastInteractTime;


    public void Interact()
    {
        if (Time.time < lastInteractTime + interactCooldown)
            return;

        lastInteractTime = Time.time;

        if (!isHidden)
        {
            isHidden = true;
            Debug.Log("Hidden.");
            Player.transform.position = Hidepos.transform.position;
        }
        else
        {
            isHidden = false;
            Debug.Log("Left hiding spot.");
            Player.transform.position = unHidepos.transform.position;
        }

    }

    public string GetPromptText()
    {
        return "E - hide";
    }

    public Transform GetPromptAnchor()
    {
        return promptAnchor != null ? promptAnchor : transform;
    }
}
