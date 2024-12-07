using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// status effects include buffs and debuffs,
/// These effects can give simple flat bonuses to certain stats, or
/// they can restore different resources, they can also add damage to attacks.
/// Any status effect with a stun should be a standalone
/// </summary>
public class StatusEffect
{
    public Dictionary<STATS, int> stat_change = new Dictionary<STATS, int>();
    public Dictionary<DamageType, Dictionary<DamageSubType, int>> damage_change = new Dictionary<DamageType, Dictionary<DamageSubType, int>>();
    public Dictionary<DamageType, int> defense_change = new Dictionary<DamageType, int>();
    public Dictionary<DamageSubType, float> resistance_change = new Dictionary<DamageSubType, float>();
    public Dictionary<RESOURCES, Dictionary<DamageSubType, int>> resource_change = new Dictionary<RESOURCES, Dictionary<DamageSubType, int>>();

    /// <summary>
    /// Defines whether this stat needs to offset a resource each turn
    /// </summary>
    public bool resourceboost = false;

    /// <summary>
    /// Defines whether this status causes a target to be stunned
    /// Only one stun can exist on a target at any given time
    /// </summary>
    public bool stun = false;

    /// <summary>
    /// Defines whether 
    /// </summary>
    public bool stunres = false;

    /// <summary>
    /// The identifier is used to detect multiple same buffs
    /// </summary>
    public int identifier;

    /// <summary>
    /// Identifier for the number of stacks, really only applicable to 
    /// </summary>
    public int stacks = 1;

    public int Duration;


    public StatusEffect() 
    {
        damage_change[DamageType.Physical] = new Dictionary<DamageSubType, int>();
        damage_change[DamageType.Magical] = new Dictionary<DamageSubType, int>();
    }

    /// <summary>
    /// Ticks a buff on a character
    /// </summary>
    /// <param name="human"></param>
    public void Tick(Humanoid human) 
    {
        if (resourceboost) Restore(human);

        Duration--; //  Duration ticks down
        if (Duration == 0) 
        {
            if (stun)   //  This was a stun 
            {
                var stunRes = new StatusEffect()
                {
                    identifier = 1,
                    stunres = true,
                    Duration = 2
                };

                human.AddStatus(stunRes);
            }

            human.RemoveStatus(this); //  Remove if duration 
        }    
    }

    /// <summary>
    /// Returns the int for a given stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public int GetStatChange(STATS stat) 
    {
        return stat_change.ContainsKey(stat) ? stat_change[stat] : 0;
    }

    /// <summary>
    /// Returns a damage struct if this status provides a bonus to damage
    /// </summary>
    /// <param name="type"></param>
    /// <param name="s_type"></param>
    /// <returns></returns>
    public Damage GetDamageChange(DamageType type, DamageSubType s_type)
    {
        if (!damage_change[type].ContainsKey(s_type)) return new Damage(null, null);
        Dictionary<DamageType, int> t_dam = new Dictionary<DamageType, int>();
        t_dam[type] = damage_change[type][s_type];
        Dictionary<DamageSubType, float> t_per = new Dictionary<DamageSubType, float>();
        t_per[s_type] = 1.0f;

        return new Damage(t_dam, t_per);
    }

    /// <summary>
    /// Returns the int for resistance change
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public int GetDefenseChange(DamageType type)
    {
        return defense_change.ContainsKey(type) ? defense_change[type] : 0;
    }

    /// <summary>
    /// Returns the float for resistance change, this should be
    /// additive with other similar effects
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetResistanceChange(DamageSubType type)
    {
        return resistance_change.ContainsKey(type) ? resistance_change[type] : 0f;
    }


    /// <summary>
    /// Restores the target resource
    /// </summary>
    /// <param name="human"></param>
    public void Restore(Humanoid human) 
    {
        foreach (var res in resource_change) 
        {
            foreach (var type in resource_change[res.Key]) 
            {
                human.CalculateHealingTaken(resource_change[res.Key][type.Key], type.Key, res.Key);
            }
        }
    }

}
