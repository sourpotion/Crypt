using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
    public List<InventorySlot> slots;

    public Inventory(int size)
{
    slots = new List<InventorySlot>();

    for (int i = 0; i < size; i++)
    {
        slots.Add(new InventorySlot());
    }
}

}
