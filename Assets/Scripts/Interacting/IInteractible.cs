using UnityEngine;

public class IInteractible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public interface IInteractable
    {
        void Interact();
        string GetPromptText();
        Transform GetPromptAnchor();
    }
}