using UnityEngine;
[System.Serializable]
public abstract class Item
{
    public string Name { get; set; }
    public string Description { get; set; }

    public bool Stackable { get; set; } = false;    //  Whether this item is stackable. Defaults to false

    public int StackCount { get; set; } = 1;    //  How many of this item exists in this stack, also defaults to one

    public RARITY RARITY { get; set; }

    public ItemType ItemType { get; set; }

    public Item() { }

    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// This should really only be used for debugging
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Name}: {Description}";
    }

    /// <summary>
    /// The usage of an item is determined by each individual item.
    /// </summary>
    public abstract bool Use(Humanoid target);
    
    

    /// <summary>
    /// Returns the translated name for an item
    /// </summary>
    /// <returns></returns>
    public string GetName() { return LocalizationManager.Instance.ReadUIDictionary(Name); }

    /// <summary>
    /// Returns the translated name for an item
    /// </summary>
    /// <returns></returns>
    public string GetDescription() { return LocalizationManager.Instance.ReadUIDictionary(Description); }

}

/// <summary>
/// The item type of an item helps defines what should happen when it is used
/// </summary>
public enum ItemType { EQUIPMENT, CONSUMABLE, REUSABLE }

/// <summary>
/// The different rarities associated with the Item
/// Used to determine the border color, and the stats of any
/// equipments falling under this catagory
/// 
/// 
/// </summary>
public enum RARITY { Common, Uncommon, Rare, Epic, Legendary }