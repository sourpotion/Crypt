using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Item 
{
    public string itemName;

    public virtual void Use()
    {
        Debug.Log("Used"+itemName);
    }
}
