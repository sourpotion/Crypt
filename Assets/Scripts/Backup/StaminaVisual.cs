using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StaminaVisual : MonoBehaviour
{
    [Header("Must")]
    public PlrMovement plrMovement;
    public Slider mainStamina;
    public GameObject runStaminaManger;
    public TextMeshProUGUI runStamina;

    // Update is called once per frame
    void Update()
    {
        UpdateRunStamina();
        UpdateMainStamina();
    }

    void UpdateMainStamina()
    {
        mainStamina.value = plrMovement.stamina / plrMovement.maxStamina;
    }

    void UpdateRunStamina()
    {
        if (!plrMovement.isSprinting)
        {
            runStaminaManger.SetActive(false);
            return;
        }

        runStamina.text = math.ceil(plrMovement.stamina) + ""; //why the f#ck is this working

        if (!runStaminaManger.activeSelf) {runStaminaManger.SetActive(true);}
    }
}
