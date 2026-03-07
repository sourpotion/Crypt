using UnityEngine;

public class DoorInteracion : MonoBehaviour
{
    private bool isOpen = false;

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

        isOpen = false;
    }

    void OpenDoor()
    {
        print("an pls do a close an");
    }

    void CloseDoor()
    {
        print("an pls do a close an");
    }

    public string GetPromptText()
    {
        return isOpen ? "E to close" : "E to open"; //if isopen then ... else ...
    }
}
