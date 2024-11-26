using UnityEngine;

/// <summary>
/// The inventory system will display any item that a player has equipped along with the full list
/// of items in the players inventory. Items should be filtered by catagories of all, weapons, armor, jewellery,
/// and consumables. The player should be able to enter the name of the item to find what they are looking for within
/// those restrictions
/// </summary>
public class InventoryMenu : MonoBehaviour
{

    /// <summary>
    /// Sets the gameobject to either active or not
    /// </summary>
    /// <param name="b"></param>
    public void SetActive(bool b) 
    {
        gameObject.SetActive(b);
    }

    /// <summary>
    /// This will populate the inventory with all the items a character has
    /// Done after loading a save file
    /// </summary>
    public void Load() 
    {
    
    }

}
