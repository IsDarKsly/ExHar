using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
       //Debug.Log($"{item.Name} added to inventory.");
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
           //Debug.Log($"{item.Name} removed from inventory.");
        }
        else
        {
           //Debug.Log($"{item.Name} not found in inventory.");
        }
    }

    public void PrintInventory()
    {
       //Debug.Log("Inventory:");
        foreach (var item in items)
        {
           //Debug.Log(item.ToString());
        }
    }

    public List<Item> GetInventory() 
    {
        return items;
    }
}


