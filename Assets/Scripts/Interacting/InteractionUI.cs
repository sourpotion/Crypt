using TMPro;
using UnityEngine;
using static IInteractible;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    public GameObject root;
    public TMP_Text promptText;
    public Vector3 offset = Vector3.up;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(IInteractable interactable)
    {
        root.SetActive(true);
        promptText.text = interactable.GetPromptText();

        Vector3 screenPos =
            Camera.main.WorldToScreenPoint(interactable.GetPromptAnchor().position);

        RectTransform rect = root.GetComponent<RectTransform>();
        rect.position = screenPos;
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    void LateUpdate()
    {
        // Billboard toward camera
        transform.forward = Camera.main.transform.forward;
    }
}