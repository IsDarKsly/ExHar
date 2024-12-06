using UnityEngine;
[System.Serializable]
public class Consumable : Item
{
    public int HealthRestore { get; set; } // How much health it restores
    public int ManaRestore { get; set; } // How much mana it restores
    public int Duration { get; set; } // Duration of effects, if any (in seconds)

    public Consumable(string name, string description, int id,int healthRestore, int manaRestore, int duration)
        : base(name, description, id)
    {
        HealthRestore = healthRestore;
        ManaRestore = manaRestore;
        Duration = duration;
    }

    public void Use(Humanoid target)
    {
        // Logic for using the consumable
        Debug.Log($"{Name} used on {target.Name}.");
        if (HealthRestore > 0)
        {
            // Restore health logic
            Debug.Log($"{target.Name} restored {HealthRestore} health.");
        }
        if (ManaRestore > 0)
        {
            // Restore mana logic
            Debug.Log($"{target.Name} restored {ManaRestore} mana.");
        }
    }
}
