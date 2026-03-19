using UnityEngine;

public class PlrCam : MonoBehaviour
{
    private GameMangeren gameMangeren;
    private float sensitivity = 1f;
    private float maxUpAngle = 90f;
    private float xRotation = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameMangeren = GameMangeren.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMangeren.gameIsPause) {return;}

        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxUpAngle, maxUpAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
