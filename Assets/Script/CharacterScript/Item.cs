using UnityEngine;

public abstract class Item
{
    public string Name { get; set; }
    public string Description { get; set; }

    public RARITY RARITY { get; set; }

    /// <summary>
    /// The spriteID determines the appearance of the object
    /// </summary>
    public int spriteID { get; set; }

    public Item() { }

    public Item(string name, string description, int id)
    {
        Name = name;
        Description = description;
        spriteID = id;
    }

    public override string ToString()
    {
        return $"{Name}: {Description}";
    }


    /// <summary>
    /// Given a character with a level, typically the main charater,
    /// Calculates and returns a Rarity
    /// </summary>
    /// <returns></returns>
    public static RARITY GenerateRarity(Humanoid target) 
    {
        float common_chance = (0.7f - ((target.Level - 1) * 0.012245f));
        float uncommon_chance = (0.2f);
        float rare_chance = (0.06f - ((target.Level - 1) * 0.00693f));
        float epic_chance = (0.03f - ((target.Level - 1) * 0.00448f));
        float legendary_chance = (0.01f - ((target.Level - 1) * 0.00081f));

        float RandomFloat = Random.Range(0, 1f);

        if (RandomFloat <= legendary_chance)    //  Legendary 
        {
            return RARITY.Legendary;
        }
        else if (RandomFloat <= legendary_chance + epic_chance)  //  Epic 
        {
            return RARITY.Epic;
        }
        else if (RandomFloat <= legendary_chance + epic_chance + rare_chance)  //  Rare 
        {
            return RARITY.Rare;
        }
        else if (RandomFloat <= legendary_chance + epic_chance + rare_chance + uncommon_chance)  //  Uncommon 
        {
            return RARITY.Uncommon;
        }

        return RARITY.Common;
    }
}


/// <summary>
/// The different rarities associated with the Item
/// Used to determine the border color, and the stats of any
/// equipments falling under this catagory
/// 
/// 
/// </summary>
public enum RARITY { Common, Uncommon, Rare, Epic, Legendary }