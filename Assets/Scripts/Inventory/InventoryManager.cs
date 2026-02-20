using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;

    void Awake()
    {
        if (FindObjectsOfType<InventoryManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }


        inventory = new Inventory(3);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(inventory.slots.Count);
        }
    }
}
