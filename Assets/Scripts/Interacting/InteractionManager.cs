using UnityEngine;
using static IInteractible;

public class InteractionManager : MonoBehaviour
{
    public Camera cam;
    public LayerMask interactableLayer;

    private IInteractable currentInteractable;

    void Update()
    {
        HandleHover();
        HandleInput();
    }

    void HandleHover()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 20f, ~0, QueryTriggerInteraction.Ignore))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    InteractionUI.Instance.Show(interactable);
                }
                return;
            }
        }

        ClearHover();
    }

    void HandleInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    void ClearHover()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            InteractionUI.Instance.Hide();
        }
    }
}