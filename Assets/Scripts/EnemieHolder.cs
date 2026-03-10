using UnityEngine;

public class EnemieHolder : MonoBehaviour
{
    public static EnemieHolder Instance { get; private set; } //make it global

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void EnableEnemie(bool enableState)
    {
        //loop through everyEnemie and (dis)enable them
        foreach (Transform enemie in transform)
        {
            enemie.gameObject.SetActive(enableState);
        }
    }

    public void ResetEnemie()
    {
        foreach (Transform enemie in transform)
        {
            enemie.GetComponent<Enemie>().Respawn();
        }
    }

    public void DeleteAllEnemie()
    {
        //loop through eneimies and delete it
        foreach (Transform enemie in transform)
        {
            Destroy(enemie.gameObject);
        }
    }
}
