using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameOverUi : MonoBehaviour
{
    public static GameOverUi Instance { get; private set; } //make it global

    [Header("Must")]
    public GameObject gameOverScreen;
    public CharacterController plrCc;

    private GameMangeren gameMangeren;
    private AudioSource gameOverSfx;
    private bool debounce = false;
    private float respawnTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        gameMangeren = GameMangeren.Instance;
        gameOverSfx = gameOverScreen.GetComponent<AudioSource>();
    }

    public void Respawn()
    {
        if (debounce) {return;}
        debounce = true;

        gameMangeren.LoadGameFile();
        StartCoroutine(RespawnWait());

        //mouse invisble and locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator RespawnWait()
    {
        yield return new WaitForSeconds(respawnTime);

        gameOverScreen.SetActive(false);
        EnemieHolder.Instance.EnableEnemie(true);

        plrCc.enabled = true;
        gameMangeren.plrDied = false;
        debounce = false;
    }

    public void PlrDied()
    {
        gameMangeren.plrDied = true;
        plrCc.enabled = false;
        gameOverScreen.SetActive(true);

        gameOverSfx.Play();
    }
}
