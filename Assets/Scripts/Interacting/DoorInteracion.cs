using DG.Tweening;
using UnityEngine;
using static IInteractible;

public class DoorInteracion : MonoBehaviour, IInteractable
{
    //system door open to the -x local 
    
    [Header("Sfx")]
    public AudioSource openSfx;
    public AudioSource closeSfx;

    [Header("interaction surport")]
    public Transform promptAnchor;

    private bool isOpen = false;
    [HideInInspector] public float timeToOpen = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (isOpen) {CloseDoor();}
        else {OpenDoor();}
    }

    public void OpenDoor()
    {
        isOpen = true;

        transform.DORotate(new Vector3(0, 90, 0), timeToOpen);
        openSfx.Play();
    }

    public void CloseDoor()
    {
        isOpen = false;

        transform.DORotate(new Vector3(0, 0, 0), timeToOpen);
        closeSfx.Play();
    }

    public string GetPromptText()
    {
        return isOpen ? "E to close" : "E to open"; //if isopen then ... else ...
    }

    public Transform GetPromptAnchor() //uuuuuhm i ma just public now for the transform okiedokie
    {
        return promptAnchor;
    }
}
